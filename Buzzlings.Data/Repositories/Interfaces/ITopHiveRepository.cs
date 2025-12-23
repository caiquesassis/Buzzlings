using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface ITopHiveRepository : IRepository<TopHive>
    {
        Task UpdateAsync(TopHive topHive);
        Task BulkUpdateAsync(ICollection<TopHive> topHives);
    }
}
