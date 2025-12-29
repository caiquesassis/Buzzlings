using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.Data.Repositories
{
    public class BuzzlingRepository : Repository<Buzzling>, IBuzzlingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BuzzlingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsync(Buzzling buzzling)
        {
            await _dbContext.SingleUpdateAsync(buzzling);
        }

        public async Task BulkUpdateAsync(ICollection<Buzzling> buzzlings)
        {
            await _dbContext.BulkUpdateAsync(buzzlings);
        }
    }
}
