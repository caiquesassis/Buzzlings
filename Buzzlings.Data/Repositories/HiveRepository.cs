using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Buzzlings.Data.Repositories
{
    public class HiveRepository : Repository<Hive>, IHiveRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HiveRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Hive?> GetWithBuzzlingsAndRolesAsync(Expression<Func<Hive, bool>> filter)
        {
            return await _dbContext.Hives
                .Where(filter)
                .Include(hive => hive.Buzzlings!)
                .ThenInclude(buzzling => buzzling.Role)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Hive hive)
        {
            await _dbContext.SingleUpdateAsync(hive);
        }
    }
}
