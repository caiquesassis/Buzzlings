using Buzzlings.Data.Contexts;
using Buzzlings.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Buzzlings.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IHiveRepository HiveRepository { get; private set; }
        public IBuzzlingRepository BuzzlingRepository { get; private set; }
        public IBuzzlingRoleRepository BuzzlingRoleRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            HiveRepository = new HiveRepository(_dbContext);
            BuzzlingRepository = new BuzzlingRepository(_dbContext);
            BuzzlingRoleRepository = new BuzzlingRoleRepository(_dbContext);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        /*
         * This ensures that the EF Core Change Tracker "forgets" about these entities,
         * preventing it from trying to delete them a second time during SaveAsync.
         * By setting the state to Detached, you are telling EF: "Do not track this object,
         * do not check its version, and do not generate any SQL for it."
         */
        public void DetachRange(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
            {
                var entry = _dbContext.Entry(entity);

                if (entry is not null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }
    }
}
