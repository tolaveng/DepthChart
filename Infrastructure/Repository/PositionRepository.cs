using Application.IRepository;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly DbSet<Position> _db;
        private readonly AppDbContext _dbContext;
        public PositionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _db = dbContext.Set<Position>();
        }

        public async Task<Position> GetAsync(string position)
        {
            return await _db.SingleOrDefaultAsync(x => x.Id.ToUpper() == position.ToUpper());
        }

        public async Task<bool> InsertAsync(Position position)
        {
            try
            {
                position.Id = position.Id.ToUpper();
                var result = await _db.AddAsync(position);
                var added = result.State == EntityState.Added;
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(added);

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
