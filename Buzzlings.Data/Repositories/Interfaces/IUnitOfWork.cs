namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IHiveRepository HiveRepository { get; }
        IBuzzlingRepository BuzzlingRepository { get; }
        IBuzzlingRoleRepository BuzzlingRoleRepository { get; }
        Task Save();
    }
}
