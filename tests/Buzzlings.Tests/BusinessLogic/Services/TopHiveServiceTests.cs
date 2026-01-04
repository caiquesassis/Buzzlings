using Buzzlings.BusinessLogic.Services.TopHive;
using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories;
using Buzzlings.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Buzzlings.Tests.BusinessLogic.Services
{
    public class TopHiveServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly Microsoft.Data.Sqlite.SqliteConnection _connection;

        private readonly ITopHiveService _systemUnderTest;

        public TopHiveServiceTests()
        {
            _connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
            _dbContext = GetDbContext(_connection);
            _unitOfWork = new UnitOfWork(_dbContext);

            _systemUnderTest = new TopHiveService(_unitOfWork);
        }

        [Theory]
        [InlineData(31, true)]
        [InlineData(12, true)]
        [InlineData(11, false)] //Last entry according to the seeding logic has hive age as 11, but logic says the earlier ID stays in case of a draw
        [InlineData(10, false)]
        public async Task AddTopHiveAsync_WhenHiveAgeIsWithinTopHives_ShouldCreateEntryWithCorrectUserAndAge(int hiveAge, bool shouldEnterTopEntries)
        {
            //Arrange

            int entries = _unitOfWork.TopHiveRepository.GetTableBufferSize();

            //Fill the DB with seed data for the testing purposes
            if (await _dbContext.TopHives.CountAsync() is 0)
            {
                for (int i = 0; i < entries; i++)
                {
                    _dbContext.Users.Add(new User() 
                    { 
                        Id = (i + 1).ToString(),
                        UserName = "User " + i
                    });

                    _dbContext.TopHives.Add(new TopHive()
                    {
                        Id = i + 1,
                        UserId = (i + 1).ToString(),
                        HiveName = "Hive " + (i + 1),
                        HiveAge = (entries + 10) - i
                    });
                }

                await _dbContext.SaveChangesAsync();
            }

            //Ensure User actually exists in the DB
            User user = new User() 
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "New Top User"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            //Act
            await _systemUnderTest.AddTopHiveAsync(user.Id, "New Top Hive", hiveAge);

            //Assert
            bool isNewHiveInTopEntries = _dbContext.TopHives.Select(topHive => topHive.HiveName).Contains("New Top Hive");

            Assert.Equal(shouldEnterTopEntries, isNewHiveInTopEntries);
            Assert.Equal(entries, _dbContext.TopHives.Count());
        }

        //This method runs automatically after every [Fact] or [Theory]
        [Fact]
        public void Dispose()
        {
            //Dispose the context (closes tracking)
            _dbContext.Dispose();

            //Close and Dispose the connection (destroys the In-Memory DB)
            _connection.Close();
            _connection.Dispose();
        }

        private ApplicationDbContext GetDbContext(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            //We need to use SQLite instead of an in-memory database because
            //the in-memory database does not support the ExecuteDelete and ExecuteDeleteAsync methods
            //which we're using in the TopHiveRepository...but SQLite does
            //SQLite needs a connection to be kept open to maintain the database
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}
