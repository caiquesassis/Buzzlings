using Buzzlings.Data.Contexts;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Buzzlings.Data.Repositories
{
    public class TopHiveRepository : Repository<TopHive>, ITopHiveRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private const int TableBufferSize = 3;

        public TopHiveRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task TrimTopHiveEntriesAsync()
        {
            //The idea here is that whenever a User finishes the simulation,
            //they'll send their Hive data to be added to the Top Hives table.
            //Then, to pick the top 10 (but with a safety margin in case more entries
            //get added due to concurrency problems or something) we actually keep the
            //20 top entries here (after sorting them by highest age and earliest id).
            //And then we trim out whatever is after the 20 top entries.
            //Finally, in the view we only need to retrieve the actual top 10 entries,
            //which will already be sorted.

            //This translates to a single DELETE FROM ... WHERE Id NOT IN (SELECT TOP 20...)
            //Best performance and minimizes network traffic. No Lists to keep in memory.
            await _dbContext.TopHives
                //If Contains is False (the ID is NOT in the top 20), the row is deleted.
                .Where(h => _dbContext.TopHives //Where the current hive is not in the returned list of the ids below
                    .OrderByDescending(x => x.HiveAge) //Highest age first
                    .ThenBy(x => x.Id) //Draw: earliest ID stays on top
                    .Take(TableBufferSize) //Keep the top 20 (buffer)
                    .Select(x => x.Id) //Don't bring all the columns, only the ID (more performant)
                    .Contains(h.Id) == false) //Is the Id of the current TopHive being checked inside the list of Ids we just fetched?
                .ExecuteDeleteAsync(); //We don't need to SaveChangesAsync, this already takes care of it
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
