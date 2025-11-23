using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;

namespace Buzzlings.Data.Repositories
{
    public class HiveRepository : Repository<Hive>, IHiveRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HiveRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Hive hive)
        {
            _dbContext.Entry(hive).CurrentValues.SetValues(hive);
        }
    }
}
