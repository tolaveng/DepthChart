using Application.IRepository;
using Application.Models;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.RepositoryMock;
using Xunit;

namespace Test
{
    public class DepthChartServiceTests
    {
        
        private readonly IDepthChartService _depthChartService;
        private readonly IPlayerRepository _playerRepo;
        private readonly IPositionRepository _positionRepo;
        private readonly IChartRepository _chartRepo;

        public DepthChartServiceTests()
        {
            _playerRepo = PlayerRepositoryMock.GetRepository().Object;
            _positionRepo = PositionRepositoryMock.GetRepository().Object;
            _chartRepo = ChartRepositoryMock.GetRepository().Object;
            _depthChartService = new DepthChartService(_playerRepo, _positionRepo, _chartRepo);
        }

        [Fact]
        public async void ChartService_Add_Player_Should_Last()
        {
            var tomBrady = new PlayerDto()
            {
                Number = 12, Name = "Tom Brady", TeamId = 0
            };

            var result = await _depthChartService.AddPlayerToDepthChart("QB", tomBrady, null);
            var allCharts = await _chartRepo.GetAllAsync();
            var lastDepth = allCharts.OrderBy(x => x.Depth).Last().Depth;

            Assert.True(result);
            Assert.Equal(3, lastDepth);
        }

        [Fact]
        public async void ChartService_Add_Player_Should_First()
        {
            var player = new PlayerDto()
            {
                Number = 111,
                Name = "First Player",
                TeamId = 0
            };

            var result = await _depthChartService.AddPlayerToDepthChart("QB", player, 0);
            var allCharts = (await _chartRepo.GetAllAsync()).OrderBy(x => x.Depth);
            var firstChart = allCharts.First();
            var lastChart = allCharts.Last();

            Assert.True(result);
            Assert.Equal(0, firstChart.Depth);
            Assert.Equal(111, firstChart.PlayerNumber);
            Assert.Equal(2, lastChart.Depth);
        }

        [Fact]
        public async void ChartService_Add_Player_Should_Shift_Depth()
        {
            var player = new PlayerDto()
            {
                Number = 22,
                Name = "John Test",
                TeamId = 1
            };

            var result = await _depthChartService.AddPlayerToDepthChart("QB", player, 2);
            var allCharts = await _chartRepo.GetAllAsync();
            var chart = allCharts.FirstOrDefault(x => x.PositionId == "QB" && x.PlayerNumber == 22);
            var lastDepth = allCharts.OrderBy(x => x.Depth).Last().Depth;

            Assert.True(result);
            Assert.NotNull(chart);
            Assert.Equal(2, chart.Depth);
            Assert.Equal(3, lastDepth);
        }
    }
}
