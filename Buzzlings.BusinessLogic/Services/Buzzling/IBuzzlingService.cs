namespace Buzzlings.BusinessLogic.Services.Buzzling
{
    public interface IBuzzlingService
    {
        Task CreateAsync(Data.Models.Buzzling buzzling);
        Task<IEnumerable<Data.Models.Buzzling?>> GetAllAsync();
        Task<Data.Models.Buzzling?> GetByIdAsync(int id);
        Task UpdateAsync(Data.Models.Buzzling buzzling);
        Task BulkUpdateAsync(ICollection<Data.Models.Buzzling> buzzlings);
        Task DeleteAsync(Data.Models.Buzzling buzzling);

        Task DeleteRangeAsync(ICollection<Data.Models.Buzzling> buzzlings);
    }
}
