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

        public void UpdateAsync(Buzzling buzzling)
        {
            _dbContext.Update(buzzling);
        }

        public void BulkUpdateAsync(ICollection<Buzzling> buzzlings)
        {
            _dbContext.UpdateRange(buzzlings);
        }
    }
}
