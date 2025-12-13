using System.Linq.Expressions;

namespace Buzzlings.BusinessLogic.Services.Hive
{
    public interface IHiveService
    {
        Task CreateAsync(Data.Models.Hive hive);
        Task<IEnumerable<Data.Models.Hive?>> GetAllAsync();
        Task<Data.Models.Hive?> GetAsync(Expression<Func<Data.Models.Hive, bool>> filter, string? includeProperties = null);
        Task<Data.Models.Hive?> GetWithBuzzlingsAndRolesAsync(Expression<Func<Data.Models.Hive, bool>> filter);
        Task<Data.Models.Hive?> GetByIdAsync(int id);
        Task UpdateAsync(Data.Models.Hive hive);
        Task DeleteAsync(Data.Models.Hive hive);
    }
}
