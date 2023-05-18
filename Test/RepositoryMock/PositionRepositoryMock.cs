using Application.IRepository;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.RepositoryMock
{
    public static class PositionRepositoryMock
    {
        public static Mock<IPositionRepository> GetRepository()
        {
            var positions = new List<Position>
            {
                new Position
                {
                    Id = "QB", Name = "QB"
                }
            };

            var repo = new Mock<IPositionRepository>();

            repo.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string n) => {
                    return positions.FirstOrDefault(p => p.Id == n);
                });

            repo.Setup(x => x.InsertAsync(It.IsAny<Position>()))
                .ReturnsAsync((Position pos) =>
                {
                    positions.Add(pos);
                    return true;
                });

            return repo;
        }
    }
}
