using Domain.Entities;
using Infrastructure.Seeder;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Chart> Charts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chart>().HasOne(x => x.Player).WithMany(x => x.Charts).HasForeignKey(x => x.PlayerNumber);
            modelBuilder.Entity<Chart>().HasOne(x => x.Position).WithMany().HasForeignKey(x => x.PositionId);

            modelBuilder.ApplyConfiguration(new SportSeeder());
            modelBuilder.ApplyConfiguration(new TeamSeeder());
        }
    }
}
