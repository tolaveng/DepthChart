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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Chart>> GetAllAsync()
        {
            return await _chartDb.ToListAsync();
        }

        public async Task<Chart> GetAsync(Guid id)
        {
            return await _chartDb.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Chart> GetByPlayerNumberAsync(int playerNumber)
        {
            return await _chartDb.FirstOrDefaultAsync(x => x.PlayerNumber == playerNumber);
        }

        public async Task<Chart> GetLastPositionAsync(string position, string group)
        {
            return await _chartDb.Where(x => x.PositionId == position && x.Group == group)
                .OrderByDescending(x => x.Depth).FirstAsync();
        }

        public async Task<bool> InsertAsync(Chart chart)
        {
            try
            {
                var result = await _chartDb.AddAsync(chart);
                return await Task.FromResult(result.State == EntityState.Added);
            } catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> ShiftDepthAsync(string position, string group, int depth)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Chart chart)
        {
            try
            {
                _chartDb.Attach(chart);
                _dbContext.Entry(chart).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
