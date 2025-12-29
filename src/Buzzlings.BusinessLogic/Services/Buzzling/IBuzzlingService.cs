namespace Buzzlings.BusinessLogic.Services.Buzzling
{
    public interface IBuzzlingService
    {
        Task CreateBuzzlingAsync(Data.Models.Buzzling buzzling);
        Task<IEnumerable<Data.Models.Buzzling?>> GetAllBuzzlingsAsync();
        Task<Data.Models.Buzzling?> GetBuzzlingByIdAsync(int id);
        Task UpdateBuzzlingAsync(Data.Models.Buzzling buzzling);
        Task BulkUpdateBuzzlingsAsync(ICollection<Data.Models.Buzzling> buzzlings);
        Task DeleteBuzzlingAsync(Data.Models.Buzzling buzzling);
        Task DeleteBuzzlingByIdAsync(int id);

        Task DeleteBuzzlingsRangeAsync(ICollection<Data.Models.Buzzling> buzzlings);
    }
}
