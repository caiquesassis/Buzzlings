using Buzzlings.BusinessLogic.Dtos;
using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.Simulation;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.BusinessLogic.Simulation.Interfaces;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using NSubstitute;

namespace Buzzlings.Tests.BusinessLogic.Services
{
    public class SimulationServiceTests
    {
        private readonly IUserService _userService;
        private readonly IHiveService _hiveService;
        private readonly IBuzzlingService _buzzlingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimulationEventHandler _simulationEventHandler;
        private readonly ISimulationService _systemUnderTest;

        public SimulationServiceTests()
        {
            _userService = Substitute.For<IUserService>();
            _hiveService = Substitute.For<IHiveService>();
            _buzzlingService = Substitute.For<IBuzzlingService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _simulationEventHandler = Substitute.For<ISimulationEventHandler>();

            _systemUnderTest = new SimulationService(_userService, _hiveService, _buzzlingService, _unitOfWork, _simulationEventHandler);
        }

        [Fact]
        public async Task ProcessSimulationAsync_WhenDisasterEvent_UpdatesStateAndTriggersDeletions()
        {
            // Arrange
            User user = new User 
            { 
                Id = "1",
                Hive = new Hive 
                { 
                    EventLog = ["Event Log"],
                    Buzzlings = new List<Buzzling> 
                    {
                        new Buzzling 
                        { 
                            Id = 1,
                            Name = "Buzzling",
                            RoleId = 1,
                            Mood = 100,
                        } 
                    }
                }
            };

            SimulationEventDto simulationEvent = new
            (
                log: "Disaster log",
                happinessImpact: -10,
                buzzlingsToDelete: 1
            );

            _userService.GetUserByIdAsync(user.Id, true, true, true).Returns(user);

            _simulationEventHandler.GenerateEvent(Arg.Any<List<Buzzling>>(), Arg.Any<string>())
                .Returns(simulationEvent);

            //Act
            int result = await _systemUnderTest.ProcessSimulationAsync(user.Id);

            //Assert

            //Database Integrity (Behavior)
            //It's best to check if the method was called instead of checking the list size
            //because we're testing behaviour, not state. Besides, checking if the methods that
            //call to the DB are called is more important than the size of an in-memory list in this case
            _unitOfWork.Received(1).DetachRange(Arg.Any<IEnumerable<object>>());
            await _buzzlingService.Received(1).DeleteBuzzlingsRangeAsync(Arg.Any<ICollection<Buzzling>>());

            //Business Logic (State)
            Assert.Equal(90, result);
            Assert.Contains("Disaster log", user.Hive.EventLog!);
        }

        [Theory]
        [InlineData(0, 1)] // Happiness 0, Age stays 1
        [InlineData(10, 2)] // Happiness > 0, Age increments to 2
        public async Task IncrementHiveAge_WhenVaryingHappiness_ShouldHandleAgeCorrecty(int happiness, int expectedAge)
        {
            //Arrange
            User user = new User
            {
                Id = "1",
                Hive = new Hive
                {
                    Age = 1,
                    Happiness = happiness
                }
            };

            _userService.GetUserByIdAsync(user.Id, true).Returns(user);

            //Act
            await _systemUnderTest.IncrementHiveAge(user.Id);

            //Assert
            Assert.Equal(expectedAge, user.Hive.Age);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(3, 2)]
        [InlineData(5, 0)]
        public async Task GetLatestEventLogs_WhenVaryingLastLogIndex_ShouldReturnCorrectTrimmedLogs(int lastLogIndex, int expectedLogCount)
        {
            //Arrange
            User user = new User
            {
                Id = "1",
                Hive = new Hive
                {
                    EventLog = ["1", "2", "3", "4", "5"]
                }
            };

            _userService.GetUserByIdAsync(user.Id, true).Returns(user);

            //Act
            var (newLogs, updatedLogIndex) = await _systemUnderTest.GetLatestEventLogs(user.Id, lastLogIndex);

            //Assert
            Assert.Equal(expectedLogCount, newLogs.Count);
            Assert.Equal(5, updatedLogIndex);

            if (expectedLogCount > 0)
            {
                //Important to check because if the logic was inversed, we could have the
                //assertion above passing, but the content could be wrong
                //Verify we actually got the LATEST ones (the end of the list)
                Assert.Equal("5", newLogs.Last());
            }
        }
    }
}
