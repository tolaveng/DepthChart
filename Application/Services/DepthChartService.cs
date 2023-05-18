using Application.IRepository;
using Application.Models;
using Domain.Contants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DepthChartService : IDepthChartService
    {
        private readonly IPlayerRepository playerRepo;
        private readonly IPositionRepository positionRepo;
        private readonly IChartRepository chartRepo;

        public DepthChartService(IPlayerRepository playerRepository, IPositionRepository positionRepository, IChartRepository chartRepository)
        {
            playerRepo = playerRepository;
            positionRepo = positionRepository;
            chartRepo = chartRepository;
        }

        public async Task<bool> AddPlayerToDepthChart(string position, PlayerDto playerDto, int? depth)
        {
            var existingChart = await chartRepo.GetByPlayerAndPositionAsync(playerDto.Number, position, DefaultConstants.DefaultGroup);
            if (existingChart != null) return true; // Assume for idempotent

            var pos = await positionRepo.GetAsync(position);
            if (pos == null)
            {
                var inserted = await positionRepo.InsertAsync(new Position {
                    Id = position, Name = position
                });
                
                if (!inserted)
                {
                    return false;
                }
            }

            var player = await playerRepo.GetAsync(playerDto.Number);
            if (player == null)
            {
                player = new Player
                {
                    Number = playerDto.Number,
                    Name = playerDto.Name,
                    TeamId = 1
                };
                var inserted = await playerRepo.InsertAsync(player);
                if (!inserted)
                {
                    return false;
                }
            } // Assumption, update not implement

            var newChart = new Chart {
                Id = Guid.NewGuid(),
                PlayerNumber = player.Number,
                PositionId = position,
                Group = DefaultConstants.DefaultGroup
            };

            // if position depth is not defined, add to the end of the depth
            if (!depth.HasValue)
            {
                var lastPos = await chartRepo.GetLastPositionAsync(position, DefaultConstants.DefaultGroup);
                newChart.Depth = lastPos.Depth + 1;
            } else
            {
                newChart.Depth = depth.Value;
                await chartRepo.ShiftDepthAsync(position, DefaultConstants.DefaultGroup, depth.Value);
            }

            var result = await chartRepo.InsertAsync(newChart);
            return result;
        }

        public Task<IEnumerable<PlayerDto>> GetBackups(string position)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PlayerDto>> GetFullDepthChart()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemovePlayerFromDepthChart(string position, PlayerDto player)
        {
            throw new NotImplementedException();
        }
    }
}
