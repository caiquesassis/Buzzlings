using Buzzlings.Data.Contexts;
using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IHiveRepository HiveRepository { get; private set; }
        public IBuzzlingRepository BuzzlingRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            HiveRepository = new HiveRepository(_dbContext);
            BuzzlingRepository = new BuzzlingRepository(_dbContext);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
