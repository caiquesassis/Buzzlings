
using Buzzlings.BusinessLogic.Dtos;
using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.BusinessLogic.Simulation;
using Buzzlings.BusinessLogic.Utils;
using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.BusinessLogic.Services.Simulation
{
    public class SimulationService : ISimulationService
    {
        private readonly IUserService _userService;
        private readonly IHiveService _hiveService;
        private readonly IBuzzlingService _buzzlingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SimulationEventHandler _simulationEventHandler;

        public SimulationService(IUserService userService, IHiveService hiveService,
            IBuzzlingService buzzlingService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _hiveService = hiveService;
            _buzzlingService = buzzlingService;
            _unitOfWork = unitOfWork;

            _simulationEventHandler = new SimulationEventHandler();
        }

        public async Task<int> ProcessSimulationAsync(string? userId)
        {
            Data.Models.User? user = await _userService.GetUserByIdAsync(userId, true, true, true);

            if (user is null || user.Hive is null)
            {
                return 0;
            }

            SimulationEventDto simulationEvent =
                _simulationEventHandler.GenerateEvent(
                    user.Hive.Buzzlings is not null ? user.Hive.Buzzlings.ToList() : new List<Data.Models.Buzzling>(),
                    user.Hive.EventLog?.Last());

            if (user.Hive.Buzzlings is not null)
            {
                if (simulationEvent.buzzlingsToDelete > 0)
                {
                    List<Data.Models.Buzzling> buzzlingsToDelete = new List<Data.Models.Buzzling>();

                    for (int i = 0; i < simulationEvent.buzzlingsToDelete; i++)
                    {
                        Data.Models.Buzzling b = user.Hive.Buzzlings.ElementAt(RandomUtils.GetRandomRangeValue(0, user.Hive.Buzzlings.Count - 1));

                        buzzlingsToDelete.Add(b);

                        user.Hive.Buzzlings.Remove(b);
                    }

                    // This prevents the SaveAsync inside DeleteBuzzlingsRangeAsync from crashing
                    _unitOfWork.DetachRange(buzzlingsToDelete);

                    await _buzzlingService.DeleteBuzzlingsRangeAsync(buzzlingsToDelete);

                    await _hiveService.UpdateHiveAsync(user.Hive);
                }
            }

            user.Hive.Happiness += simulationEvent.happinessImpact;

            user.Hive.Happiness = Math.Clamp(user.Hive.Happiness!.Value, 0, 100);

            user.Hive.EventLog?.Add(simulationEvent.log);

            await _hiveService.UpdateHiveAsync(user.Hive);

            return user.Hive.Happiness.Value;
        }

        public async Task<int> IncrementHiveAge(string? userId)
        {
            Data.Models.User? user = await _userService.GetUserByIdAsync(userId, true);

            if (user is null || user.Hive is null)
            {
                return 0;
            }

            if (user.Hive!.Happiness > 0)
            {
                user.Hive.Age++;
            }

            await _hiveService.UpdateHiveAsync(user.Hive);

            return user.Hive.Age!.Value;
        }

        public async Task<(List<string>, int)> GetLatestEventLogs(string? userId, int lastLogIndex)
        {
            Data.Models.User? user = await _userService.GetUserByIdAsync(userId, true);

            if (user is null || user.Hive is null)
            {
                return (new List<string>(), 0);
            }

            if (user.Hive.EventLog is null)
            {
                user.Hive.EventLog = ["🍯 " + user.Hive.Name + " 🍯 is born!"];

                await _hiveService.UpdateHiveAsync(user.Hive);
            }

            List<string> newLogs = user.Hive.EventLog.Skip(lastLogIndex).ToList();

            int updatedLogIndex = user.Hive.EventLog.Count;

            return (newLogs, updatedLogIndex);
        }
    }
}
