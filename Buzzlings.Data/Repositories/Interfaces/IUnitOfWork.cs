namespace Buzzlings.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IHiveRepository HiveRepository { get; }
        IBuzzlingRepository BuzzlingRepository { get; }
        IBuzzlingRoleRepository BuzzlingRoleRepository { get; }
        Task SaveAsync();
        void DetachRange(IEnumerable<object> entities);
    }
}
