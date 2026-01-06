using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface ITopHiveRepository : IRepository<TopHive>
    {
        Task<IEnumerable<TopHive>?> GetTopHivesAsync();
        Task TrimTopHiveEntriesAsync();
        void Update(TopHive topHive);
        void BulkUpdate(ICollection<TopHive> topHives);
        int GetTableBufferSize();
    }
}
