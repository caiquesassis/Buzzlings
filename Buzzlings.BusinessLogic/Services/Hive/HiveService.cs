using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.Data.Constants;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Buzzlings.BusinessLogic.Services.Hive
{
    public class HiveService : IHiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBuzzlingService _buzzlingService;

        public HiveService(IUnitOfWork unitOfWork, IBuzzlingService buzzlingService)
        {
            _unitOfWork = unitOfWork;
            _buzzlingService = buzzlingService;
        }

        public async Task CreateHiveAsync(Data.Models.Hive hive)
        {
            await _unitOfWork.HiveRepository.AddAsync(hive);
            await _unitOfWork.SaveAsync();
        }

        public async Task<(bool, string?)> CreateBuzzlingAndAddToHiveAsync(string buzzlingName, int buzzlingRoleId, int buzzlingMood, int hiveId)
        {
            Data.Models.Hive? hive = await GetHiveByIdAsync(hiveId);

            if (hive is null)
            {
                return (false, "Hive not found.");
            }

            Data.Models.Buzzling buzzling = new()
            {
                Name = buzzlingName,
                Role = await _unitOfWork.BuzzlingRoleRepository.GetAsync((r) => r.Id == buzzlingRoleId),
                Mood = buzzlingMood,
                HiveId = hiveId
            };

            await _buzzlingService.CreateBuzzlingAsync(buzzling);

            hive.Buzzlings?.Add(buzzling);

            await UpdateHiveAsync(hive);

            return (true, null);
        }

        public async Task<IEnumerable<Data.Models.Hive?>> GetAllHivesAsync()
        {
            return await _unitOfWork.HiveRepository.GetAllAsync();
        }

        public async Task<Data.Models.Hive?> GetHiveAsync(Expression<Func<Data.Models.Hive, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.HiveRepository.GetAsync(filter, includeProperties);
        }

        public async Task<Data.Models.Hive?> GetHiveWithBuzzlingsAndRolesAsync(Expression<Func<Data.Models.Hive, bool>> filter)
        {
            return await _unitOfWork.HiveRepository.GetWithBuzzlingsAndRolesAsync(filter);
        }

        public async Task<Data.Models.Hive?> GetHiveByIdAsync(int id)
        {
            return await _unitOfWork.HiveRepository.GetAsync(h => h.Id == id);
        }

        public async Task UpdateHiveAsync(Data.Models.Hive hive)
        {
            await _unitOfWork.HiveRepository.UpdateAsync(hive);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateHiveNameAsync(Data.Models.Hive hive, string newName)
        {
            hive.Name = newName;

            if (hive.EventLog is not null)
            {
                hive.EventLog[0] = "🍯 " + hive.Name + " 🍯 is born!";
            }

            await UpdateHiveAsync(hive);
        }

        public async Task DeleteHiveAsync(Data.Models.Hive hive)
        {
            await _unitOfWork.HiveRepository.DeleteAsync(hive);
            await _unitOfWork.SaveAsync();
        }
    }
}
