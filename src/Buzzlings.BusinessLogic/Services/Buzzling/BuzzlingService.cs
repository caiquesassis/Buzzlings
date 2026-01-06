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

        public async Task CreateBuzzlingAsync(Data.Models.Buzzling buzzling)
        {
            await _unitOfWork.BuzzlingRepository.AddAsync(buzzling);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Data.Models.Buzzling?>> GetAllBuzzlingsAsync()
        {
            return await _unitOfWork.BuzzlingRepository.GetAllAsync();
        }

        public async Task<Data.Models.Buzzling?> GetBuzzlingByIdAsync(int id)
        {
            return await _unitOfWork.BuzzlingRepository.GetAsync(b => b.Id == id);
        }

        public async Task UpdateBuzzlingAsync(Data.Models.Buzzling buzzling)
        {
            _unitOfWork.BuzzlingRepository.UpdateAsync(buzzling);
            await _unitOfWork.SaveAsync();
        }

        public async Task BulkUpdateBuzzlingsAsync(ICollection<Data.Models.Buzzling> buzzlings)
        {
            _unitOfWork.BuzzlingRepository.BulkUpdateAsync(buzzlings);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteBuzzlingAsync(Data.Models.Buzzling buzzling)
        {
            _unitOfWork.BuzzlingRepository.Delete(buzzling);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteBuzzlingByIdAsync(int id)
        {
            Data.Models.Buzzling? buzzling = await GetBuzzlingByIdAsync(id);

            if (buzzling is not null)
            {
                await DeleteBuzzlingAsync(buzzling);
            }
        }

        public async Task DeleteBuzzlingsRangeAsync(ICollection<Data.Models.Buzzling> buzzlings)
        {
            _unitOfWork.BuzzlingRepository.DeleteRange(buzzlings);
            await _unitOfWork.SaveAsync();
        }
    }
}
