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

        public async Task Create(Data.Models.Buzzling buzzling)
        {
            _unitOfWork.BuzzlingRepository.Add(buzzling);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Data.Models.Buzzling>> GetAll()
        {
            return await _unitOfWork.BuzzlingRepository.GetAll();
        }

        public async Task<Data.Models.Buzzling> GetById(int id)
        {
            return await _unitOfWork.BuzzlingRepository.Get(b => b.Id == id);
        }

        public async Task Update(Data.Models.Buzzling buzzling)
        {
            _unitOfWork.BuzzlingRepository.Update(buzzling);
            await _unitOfWork.Save();
        }

        public async Task BulkUpdate(ICollection<Data.Models.Buzzling> buzzlings)
        {
            await _unitOfWork.BuzzlingRepository.BulkUpdate(buzzlings);
            await _unitOfWork.Save();
        }

        public async Task Delete(Data.Models.Buzzling buzzling)
        {
            _unitOfWork.BuzzlingRepository.Remove(buzzling);
            await _unitOfWork.Save();
        }

        public async Task DeleteRange(ICollection<Data.Models.Buzzling> buzzlings)
        {
            _unitOfWork.BuzzlingRepository.RemoveRange(buzzlings);
            await _unitOfWork.Save();
        }
    }
}
