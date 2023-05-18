using Application.IRepository;
using Domain.Entities;
using Infrastructure.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.RepositoryMock
{
    public static class PlayerRepositoryMock
    {
        public static Mock<IPlayerRepository> GetRepository()
        {
            var players = new List<Player>
            {
                new Player
                {
                    Number = 1,
                    Name = "Tola",
                    TeamId = 1
                }
            };

            var repo = new Mock<IPlayerRepository>();

            repo.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((int n) => {
                    return players.FirstOrDefault(p => p.Number == n);
                });

            repo.Setup(x => x.InsertAsync(It.IsAny<Player>()))
                .ReturnsAsync((Player player) =>
                {
                    players.Add(player);
                    return true;
                });

            return repo;
        }
    }
}
