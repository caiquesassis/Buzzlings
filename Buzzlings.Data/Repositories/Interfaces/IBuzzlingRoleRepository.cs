using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IBuzzlingRoleRepository : IRepository<BuzzlingRole>
    {
        void Update(BuzzlingRole role);
    }
}
