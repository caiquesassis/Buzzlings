using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using NSubstitute;
using System.Linq.Expressions;

namespace Buzzlings.Tests.BusinessLogic.Services
{
    public class HiveServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBuzzlingService _buzzlingService;
        private readonly IHiveService _systemUnderTest;

        public HiveServiceTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _buzzlingService = Substitute.For<IBuzzlingService>();

            _systemUnderTest = new HiveService(_unitOfWork, _buzzlingService);
        }

        [Fact]
        public async Task CreateBuzzlingAndAddToHiveAsync_WhenHiveIsNull_ShouldFail()
        {
            //Arrange
            //Since HiveService is our SUT, even if it has wrappers to Repository calls,
            //we need to mock unit of work (which gets called inside the method anyway)
            //instead of mocking the very system we're testing
            _unitOfWork.HiveRepository.GetAsync(Arg.Any<Expression<Func<Hive, bool>>>())
                    .Returns((Hive?)null);

            //Act
            var (result, message) = await _systemUnderTest.CreateBuzzlingAndAddToHiveAsync("Buzzling", 1, 100, 1);

            //Assert
            Assert.False(result);
            Assert.Equal("Hive not found.", message);

            //Verify the save wasn't called (since it's an early return)
            await _unitOfWork.DidNotReceive().SaveAsync();
        }

        [Fact]
        public async Task CreateBuzzlingAndAddToHiveAsync_WhenHiveIsValid_ShouldAddBuzzlingAndSave()
        {
            //Arrange
            int hiveId = 1;
            Hive hive = new Hive { Id = hiveId, Buzzlings = new List<Buzzling>() };

            _unitOfWork.HiveRepository.GetAsync(Arg.Any<Expression<Func<Hive, bool>>>()).Returns(hive);
            _unitOfWork.BuzzlingRoleRepository.GetAsync(Arg.Any<Expression<Func<BuzzlingRole, bool>>>())
                .Returns(new BuzzlingRole { Id = 1, Name = "Worker" });

            //Act
            var (result, message) = await _systemUnderTest.CreateBuzzlingAndAddToHiveAsync("Buzzling", 1, 100, hiveId);

            //Assert
            Assert.True(result);
            Assert.Single(hive.Buzzlings); //Check if buzzling was added (single because we initialized an empty list)

            //Verify dependencies were called correctly
            await _buzzlingService.Received(1).CreateBuzzlingAsync(Arg.Is<Buzzling>(b => b.Name == "Buzzling"));
            //The code actually receives 2...because SaveAsync is also called in CreateBuzzlingAsync
            //However, our BuzzlingService mock is not using the same UOW as our SUT,
            //because we cannot pass constructor arguments when mocking interfaces...
            //Thus, it only gets called once.
            await _unitOfWork.Received(1).SaveAsync(); //Once in UpdateHive
        }

        [Fact]
        public async Task UpdateHiveNameAsync_ShouldUpdateFirstEventLogEntry()
        {
            //Arrange
            Hive hive = new Hive { Name = "Old Name", EventLog = new List<string> { "🍯 Old Name 🍯 is born!" } };

            //Act
            await _systemUnderTest.UpdateHiveNameAsync(hive, "New Name");

            //Assert
            Assert.Equal("New Name", hive.Name);
            Assert.Equal("🍯 New Name 🍯 is born!", hive.EventLog[0]);

            await _unitOfWork.Received(1).SaveAsync();
        }
    }
}
