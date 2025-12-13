using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IBuzzlingRoleRepository : IRepository<BuzzlingRole>
    {
        Task UpdateAsync(BuzzlingRole role);
    }
}
