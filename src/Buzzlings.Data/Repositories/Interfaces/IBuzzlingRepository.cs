using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IBuzzlingRepository : IRepository<Buzzling>
    {
        void UpdateAsync(Buzzling buzzling);
        void BulkUpdateAsync(ICollection<Buzzling> buzzlings);
    }
}
