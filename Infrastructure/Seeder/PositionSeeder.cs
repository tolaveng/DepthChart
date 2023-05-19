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
    public class PositionSeeder : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            var positions = new List<Position>()
            {
                new Position
                {
                    Id = "OLB", Name = "OLB"
                }
            };
            builder.HasData(positions);
        }
    }
}
