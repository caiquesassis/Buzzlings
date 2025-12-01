using System.Linq.Expressions;

namespace Buzzlings.BusinessLogic.Services.Hive
{
    public interface IHiveService
    {
        Task Create(Data.Models.Hive hive);
        Task<IEnumerable<Data.Models.Hive>> GetAll();
        Task<Data.Models.Hive> Get(Expression<Func<Data.Models.Hive, bool>> filter, string? includeProperties = null);
        Task<Data.Models.Hive> GetById(int id);
        Task Update(Data.Models.Hive hive);
        Task Delete(Data.Models.Hive hive);
    }
}
