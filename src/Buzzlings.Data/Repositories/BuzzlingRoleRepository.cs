using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.Data.Repositories
{
    public class BuzzlingRoleRepository : Repository<BuzzlingRole>, IBuzzlingRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BuzzlingRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsync(BuzzlingRole role)
        {
            await _dbContext.SingleUpdateAsync(role);
        }
    }
}
