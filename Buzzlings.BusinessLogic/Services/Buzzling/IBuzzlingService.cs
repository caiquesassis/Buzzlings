namespace Buzzlings.BusinessLogic.Services.Buzzling
{
    public interface IBuzzlingService
    {
        Task Create(Data.Models.Buzzling buzzling);
        Task<IEnumerable<Data.Models.Buzzling>> GetAll();
        Task<Data.Models.Buzzling> GetById(int id);
        Task Update(Data.Models.Buzzling buzzling);
        Task Delete(Data.Models.Buzzling buzzling);

        Task DeleteRange(ICollection<Data.Models.Buzzling> buzzlings);
    }
}
