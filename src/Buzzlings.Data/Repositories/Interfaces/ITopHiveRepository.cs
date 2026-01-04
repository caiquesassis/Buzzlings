using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface ITopHiveRepository : IRepository<TopHive>
    {
        Task<IEnumerable<TopHive>?> GetTopHivesAsync();
        Task TrimTopHiveEntriesAsync();
        Task UpdateAsync(TopHive topHive);
        Task BulkUpdateAsync(ICollection<TopHive> topHives);
        int GetTableBufferSize();
    }
}
