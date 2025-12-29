using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IBuzzlingRepository : IRepository<Buzzling>
    {
        Task UpdateAsync(Buzzling buzzling);
        Task BulkUpdateAsync(ICollection<Buzzling> buzzlings);
    }
}
