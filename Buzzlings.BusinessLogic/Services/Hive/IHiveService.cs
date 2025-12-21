using System.Linq.Expressions;

namespace Buzzlings.BusinessLogic.Services.Hive
{
    public interface IHiveService
    {
        Task CreateHiveAsync(Data.Models.Hive hive);
        Task<(bool, string?)> CreateBuzzlingAndAddToHiveAsync(string buzzlingName, int buzzlingRoleId, int buzzlingMood, int hiveId);
        Task<IEnumerable<Data.Models.Hive?>> GetAllHivesAsync();
        Task<Data.Models.Hive?> GetHiveAsync(Expression<Func<Data.Models.Hive, bool>> filter, string? includeProperties = null);
        Task<Data.Models.Hive?> GetHiveWithBuzzlingsAndRolesAsync(Expression<Func<Data.Models.Hive, bool>> filter);
        Task<Data.Models.Hive?> GetHiveByIdAsync(int id);
        Task UpdateHiveAsync(Data.Models.Hive hive);
        Task UpdateHiveNameAsync(Data.Models.Hive hive, string newName);
        Task DeleteHiveAsync(Data.Models.Hive hive);
    }
}
