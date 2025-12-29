namespace Buzzlings.BusinessLogic.Services.TopHive
{
    public interface ITopHiveService
    {
        Task AddTopHiveAsync(string? userId, string hiveName, int hiveAge);
        Task<IEnumerable<Data.Models.TopHive>?> GetTopHivesAsync();
        Task DeleteTopHivesRangeAsync(ICollection<Data.Models.TopHive> topHives);
    }
}