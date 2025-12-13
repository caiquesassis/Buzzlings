using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.BusinessLogic.Services.Buzzling
{
    public class BuzzlingService : IBuzzlingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuzzlingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Data.Models.Buzzling buzzling)
        {
            await _unitOfWork.BuzzlingRepository.AddAsync(buzzling);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Data.Models.Buzzling?>> GetAllAsync()
        {
            return await _unitOfWork.BuzzlingRepository.GetAllAsync();
        }

        public async Task<Data.Models.Buzzling?> GetByIdAsync(int id)
        {
            return await _unitOfWork.BuzzlingRepository.GetAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Data.Models.Buzzling buzzling)
        {
            await _unitOfWork.BuzzlingRepository.UpdateAsync(buzzling);
            await _unitOfWork.SaveAsync();
        }

        public async Task BulkUpdateAsync(ICollection<Data.Models.Buzzling> buzzlings)
        {
            await _unitOfWork.BuzzlingRepository.BulkUpdateAsync(buzzlings);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Data.Models.Buzzling buzzling)
        {
            await _unitOfWork.BuzzlingRepository.DeleteAsync(buzzling);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteRangeAsync(ICollection<Data.Models.Buzzling> buzzlings)
        {
            await _unitOfWork.BuzzlingRepository.DeleteRangeAsync(buzzlings);
            await _unitOfWork.SaveAsync();
        }
    }
}
