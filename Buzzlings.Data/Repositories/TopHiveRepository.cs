using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.Data.Repositories
{
    public class TopHiveRepository : Repository<TopHive>, ITopHiveRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TopHiveRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsync(TopHive topHive)
        {
            await _dbContext.SingleUpdateAsync(topHive);
        }

        public async Task BulkUpdateAsync(ICollection<TopHive> topHives)
        {
            await _dbContext.BulkUpdateAsync(topHives);
        }
    }
}
