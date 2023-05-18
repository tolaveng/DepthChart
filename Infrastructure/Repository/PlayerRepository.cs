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
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DbSet<Player> _db;
        private readonly AppDbContext _dbContext;
        public PlayerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _db = dbContext.Set<Player>();
        }

        public async Task<bool> DeleteAsync(int playerNumber)
        {
            var player = await _db.SingleOrDefaultAsync(x => x.Number == playerNumber);
            if (player == null) return false;
            try
            {
                _db.Remove(player);
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _db.ToListAsync();
        }

        public async Task<Player> GetAsync(int playerNumber)
        {
            return await _db.SingleOrDefaultAsync(x => x.Number == playerNumber);
        }

        public async Task<bool> InsertAsync(Player player)
        {
            try
            {
                var result = await _db.AddAsync(player);
                return await Task.FromResult(result.State == EntityState.Added);

            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Player player)
        {
            try
            {
                _db.Attach(player);
                _dbContext.Entry(player).State = EntityState.Modified;
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}
