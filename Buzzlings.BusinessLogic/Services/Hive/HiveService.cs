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

        public async Task Create(Data.Models.Hive hive)
        {
            _unitOfWork.HiveRepository.Add(hive);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Data.Models.Hive>> GetAll()
        {
            return await _unitOfWork.HiveRepository.GetAll();
        }

        public async Task<Data.Models.Hive> Get(Expression<Func<Data.Models.Hive, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.HiveRepository.Get(filter, includeProperties);
        }

        public async Task<Data.Models.Hive> GetById(int id)
        {
            return await _unitOfWork.HiveRepository.Get(h => h.Id == id);
        }

        public async Task Update(Data.Models.Hive hive)
        {
            _unitOfWork.HiveRepository.Update(hive);
            await _unitOfWork.Save();
        }

        public async Task Delete(Data.Models.Hive hive)
        {
            _unitOfWork.HiveRepository.Remove(hive);
            await _unitOfWork.Save();
        }
    }
}
