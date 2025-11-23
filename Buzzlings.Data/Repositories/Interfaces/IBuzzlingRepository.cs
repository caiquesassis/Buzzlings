using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IBuzzlingRepository : IRepository<Buzzling>
    {
        void Update(Buzzling buzzling);
    }
}
