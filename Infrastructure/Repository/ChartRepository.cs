using Application.IRepository;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ChartRepository : IChartRepository
    {
        private readonly DbSet<Chart> _chartDb;
        private readonly AppDbContext _dbContext;

        public ChartRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _chartDb = dbContext.Set<Chart>();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var chart = await _chartDb.SingleOrDefaultAsync(x => x.Id == id);
            if (chart == null) return false;
            try
            {
                _chartDb.Remove(chart);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Chart>> GetAllAsync()
        {
            return await _chartDb.Include(x => x.Player)
                .AsNoTracking()
                .OrderBy(x => x.PositionId)
                .ThenBy(x => x.Depth)
                .ToListAsync();
        }

        public async Task<Chart> GetAsync(Guid id)
        {
            return await _chartDb.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Chart>> GetBackupsAsync(string position, int playerNumber, string group)
        {
            var chart = await _chartDb.FirstOrDefaultAsync(x => x.PositionId.ToUpper() == position.ToUpper() &&
                x.PlayerNumber == playerNumber && x.Group == group);
            if (chart == null) return Enumerable.Empty<Chart>();

            var charts = await _chartDb.Include(x => x.Player)
                .Where(x => x.PositionId.ToUpper() == position.ToUpper() &&
                x.Group == group && x.Depth > chart.Depth)
                .OrderBy(x => x.Depth)
                .ToListAsync();

            return charts;
        }

        public async Task<Chart> GetByPlayerAndPositionAsync(int playerNumber, string position, string group)
        {
            return await _chartDb.Include(x => x.Player).FirstOrDefaultAsync(x => 
                x.PositionId.ToUpper() == position.ToUpper() &&
                x.PlayerNumber == playerNumber &&
                x.Group == group);
        }

        public async Task<Chart> GetByPlayerNumberAsync(int playerNumber)
        {
            return await _chartDb.FirstOrDefaultAsync(x => x.PlayerNumber == playerNumber);
        }

        public async Task<Chart> GetLastPositionAsync(string position, string group)
        {
            return await _chartDb.Where(x => x.PositionId.ToUpper() == position.ToUpper() && x.Group == group)
                .OrderByDescending(x => x.Depth).FirstAsync();
        }

        public async Task<bool> InsertAsync(Chart chart)
        {
            try
            {
                chart.PositionId = chart.PositionId.ToUpper();
                var result = await _chartDb.AddAsync(chart);
                var added = result.State == EntityState.Added;
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(added);
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveAllAsync()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Charts]");
            return true;
        }

        public async Task ShiftDepthAsync(string position, string group, int depth)
        {
            var charts = await _chartDb
                .Where(x => x.Group == group && x.PositionId.ToUpper() == position.ToUpper())
                .OrderBy(x => x.Depth).ToArrayAsync();

            if (!charts.Any()) return;

            if (!charts.Any(x => x.Depth == depth)) return;

            var shiftChart = charts.Where(x => x.Depth >= depth);

            foreach (var chart in shiftChart)
            {
                chart.Depth += 1;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Chart chart)
        {
            try
            {
                _chartDb.Attach(chart);
                _dbContext.Entry(chart).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
