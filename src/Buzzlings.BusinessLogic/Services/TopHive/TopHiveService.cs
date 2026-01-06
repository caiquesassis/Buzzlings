using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.BusinessLogic.Services.TopHive
{
    public class TopHiveService : ITopHiveService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopHiveService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddTopHiveAsync(string? userId, string hiveName, int hiveAge)
        {
            Data.Models.TopHive topHive = new() 
            { 
                UserId = userId,
                HiveName = hiveName,
                HiveAge = hiveAge 
            };

            await _unitOfWork.TopHiveRepository.AddAsync(topHive);

            //Order matters here. If we don't save first before trimming,
            //the newly added TopHive will not be in the DB, so the Trim won't work
            //and the TopHive will be added after the trim, we don't want that
            await _unitOfWork.SaveAsync();

            await _unitOfWork.TopHiveRepository.TrimTopHiveEntriesAsync();
        }

        public async Task<IEnumerable<Data.Models.TopHive>?> GetTopHivesAsync()
        {
            return await _unitOfWork.TopHiveRepository.GetTopHivesAsync();
        }

        public async Task DeleteTopHivesRangeAsync(ICollection<Data.Models.TopHive> topHives)
        {
            _unitOfWork.TopHiveRepository.DeleteRange(topHives);
            await _unitOfWork.SaveAsync();
        }
    }
}
