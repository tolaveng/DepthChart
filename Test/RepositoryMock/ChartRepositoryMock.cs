using Application.IRepository;
using Domain.Contants;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.RepositoryMock
{
    public static class ChartRepositoryMock
    {
        public static Mock<IChartRepository> GetRepository()
        {
            var charts = new List<Chart>
            {
                new Chart
                {
                    Id = Guid.Parse("5D7762D2-EC5B-46E1-AAE9-7206591E3CC2"),
                    PlayerNumber = 1,
                    PositionId = "QB",
                    Depth = 1,
                    Group = DefaultConstants.DefaultGroup
                },
                new Chart
                {
                    Id = Guid.Parse("CEF99BD2-4640-4CAF-BCEA-F7AEDE1A3051"),
                    PlayerNumber = 2,
                    PositionId = "QB",
                    Depth = 2,
                    Group = DefaultConstants.DefaultGroup
                }
            };

            var repo = new Mock<IChartRepository>();

            repo.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => {
                    return charts.FirstOrDefault(p => p.Id == id);
                });

            repo.Setup(x => x.GetAllAsync()).ReturnsAsync(charts);

            repo.Setup(x => x.InsertAsync(It.IsAny<Chart>()))
                .ReturnsAsync((Chart chart) =>
                {
                    charts.Add(chart);
                    return true;
                });

            repo.Setup(x => x.GetLastPositionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string position, string group) =>
                {
                    return charts.Where(x => x.PositionId == position && x.Group == group)
                    .OrderBy(x => x.Depth).LastOrDefault();
                });

            repo.Setup(x => x.ShiftDepthAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(async(string position, string group, int depth) =>
                {
                    var orderedCharts = charts
                        .Where(x => x.Group == group && x.PositionId == position)
                        .OrderBy(x => x.Depth);

                    if (!orderedCharts.Any()) return;
                    if (!orderedCharts.Any(x => x.Depth == depth)) return;

                    var shiftChart = orderedCharts.Where(x => x.Depth >= depth);
                    foreach (var chart in shiftChart)
                    {
                        chart.Depth += 1;
                    }
                    await Task.FromResult(0);
                });

            repo.Setup(x => x.GetByPlayerAndPositionAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((int playerNumber, string position, string group) =>
                {
                    return charts
                        .FirstOrDefault(x => x.Group == group && x.PositionId == position && x.PlayerNumber == playerNumber);
                });

            repo.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) =>
                {
                    var chart = charts.SingleOrDefault(x => x.Id == id);
                    if (chart == null) return false;
                    charts.Remove(chart);
                    return true;
                });

            return repo;
        }
    }
}
