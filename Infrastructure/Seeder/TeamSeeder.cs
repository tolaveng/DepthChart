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
    public class TeamSeeder : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            var teams = new List<Team>()
            {
                new Team
                {
                    Id = 1, Name = "Tampa Bay Buccaneers", SportId = 1
                }
            };
            builder.HasData(teams);
        }
    }
}
