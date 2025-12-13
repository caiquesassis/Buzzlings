using Buzzlings.Data.Models;
using System.Linq.Expressions;

namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IHiveRepository : IRepository<Hive>
    {
        Task<Hive?> GetWithBuzzlingsAndRolesAsync(Expression<Func<Hive, bool>> filter);
        Task UpdateAsync(Hive hive);
    }
}
