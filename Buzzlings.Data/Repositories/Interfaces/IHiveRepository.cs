using Buzzlings.Data.Models;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IHiveRepository : IRepository<Hive>
    {
        void Update(Hive hive);
    }
}
