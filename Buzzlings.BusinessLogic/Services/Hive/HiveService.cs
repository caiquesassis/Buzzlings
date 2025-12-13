using Buzzlings.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Buzzlings.BusinessLogic.Services.Hive
{
    public class HiveService : IHiveService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HiveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Data.Models.Hive hive)
        {
            await _unitOfWork.HiveRepository.AddAsync(hive);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Data.Models.Hive?>> GetAllAsync()
        {
            return await _unitOfWork.HiveRepository.GetAllAsync();
        }

        public async Task<Data.Models.Hive?> GetAsync(Expression<Func<Data.Models.Hive, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.HiveRepository.GetAsync(filter, includeProperties);
        }

        public async Task<Data.Models.Hive?> GetWithBuzzlingsAndRolesAsync(Expression<Func<Data.Models.Hive, bool>> filter)
        {
            return await _unitOfWork.HiveRepository.GetWithBuzzlingsAndRolesAsync(filter);
        }

        public async Task<Data.Models.Hive?> GetByIdAsync(int id)
        {
            return await _unitOfWork.HiveRepository.GetAsync(h => h.Id == id);
        }

        public async Task UpdateAsync(Data.Models.Hive hive)
        {
            await _unitOfWork.HiveRepository.UpdateAsync(hive);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Data.Models.Hive hive)
        {
            await _unitOfWork.HiveRepository.DeleteAsync(hive);
            await _unitOfWork.SaveAsync();
        }
    }
}
