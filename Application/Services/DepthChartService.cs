using Application.IRepository;
using Application.Models;
using AutoMapper;
using Domain.Contants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DepthChartService : IDepthChartService
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IPositionRepository _positionRepo;
        private readonly IChartRepository _chartRepo;
        private readonly IMapper _mapper;
        public DepthChartService(IPlayerRepository _playerRepository,
            IPositionRepository positionRepository,
            IChartRepository chartRepository,
            IMapper mapper)
        {
            _playerRepo = _playerRepository;
            _positionRepo = positionRepository;
            _chartRepo = chartRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddPlayerToDepthChartAsync(string position, PlayerDto playerDto, int? depth)
        {
            var existingChart = await _chartRepo.GetByPlayerAndPositionAsync(playerDto.Number, position, DefaultConstants.DefaultGroup);
            if (existingChart != null) return true; // Assume for idempotent

            var pos = await _positionRepo.GetAsync(position);
            if (pos == null)
            {
                var inserted = await _positionRepo.InsertAsync(new Position {
                    Id = position, Name = position
                });
                
                if (!inserted)
                {
                    return false;
                }
            }

            var player = await _playerRepo.GetAsync(playerDto.Number);
            if (player == null)
            {
                player = new Player
                {
                    Number = playerDto.Number,
                    Name = playerDto.Name,
                    TeamId = 1
                };
                var inserted = await _playerRepo.InsertAsync(player);
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
                var lastPos = await _chartRepo.GetLastPositionAsync(position, DefaultConstants.DefaultGroup);
                newChart.Depth = lastPos.Depth + 1;
            } else
            {
                newChart.Depth = depth.Value;
                await _chartRepo.ShiftDepthAsync(position, DefaultConstants.DefaultGroup, depth.Value);
            }

            var result = await _chartRepo.InsertAsync(newChart);
            return result;
        }

        public async Task<IEnumerable<ChartDto>> GetBackupsAsync(string position, PlayerDto player)
        {
            var charts = await _chartRepo.GetBackupsAsync(position, player.Number, DefaultConstants.DefaultGroup);
            return _mapper.Map<IEnumerable<ChartDto>>(charts);
        }

        public async Task<IEnumerable<ChartDto>> GetFullDepthChartAsync()
        {
            var charts = await _chartRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ChartDto>>(charts);
        }

        public async Task<PlayerDto> RemovePlayerFromDepthChartAsync(string position, PlayerDto player)
        {
            var chart = await _chartRepo.GetByPlayerAndPositionAsync(player.Number, position, DefaultConstants.DefaultGroup);
            if (chart == null) return null;

            var result = await _chartRepo.DeleteAsync(chart.Id);
            return result ? player : null;
        }
    }
}
