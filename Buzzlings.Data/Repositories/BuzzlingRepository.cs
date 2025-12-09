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

        public void Update(Buzzling buzzling)
        {
            _dbContext.Entry(buzzling).CurrentValues.SetValues(buzzling);
        }

        public async Task BulkUpdate(ICollection<Buzzling> buzzlings)
        {
            await _dbContext.BulkUpdateAsync(buzzlings);
        }
    }
}
