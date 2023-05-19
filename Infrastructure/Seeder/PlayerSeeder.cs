using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeder
{
    public class PlayerSeeder : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            var players = new List<Player>()
            {
                new Player
                {
                    Number = 1, Name = "Test Player"
                }
            };
            builder.HasData(players);
        }
    }
}
