using Buzzlings.Data.Models;
using System.Linq.Expressions;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IHiveRepository : IRepository<Hive>
    {
        Task<Hive> GetWithBuzzlingsAndRoles(Expression<Func<Hive, bool>> filter);
        void Update(Hive hive);
    }
}
