using Domain.Contants;
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
    public class ChartSeeder : IEntityTypeConfiguration<Chart>
    {
        public void Configure(EntityTypeBuilder<Chart> builder)
        {
            var charts = new List<Chart>()
            {
                new Chart
                {
                    Id = Guid.NewGuid(),
                    PlayerNumber = 1, Depth = 0, PositionId = "OLB",
                    Group = DefaultConstants.DefaultGroup
                }
            };
            builder.HasData(charts);
        }
    }
}
