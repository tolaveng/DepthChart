using Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IDepthChartService
    {
        Task<bool> AddPlayerToDepthChart(string position, PlayerDto player, int? depth);
        Task<bool> RemovePlayerFromDepthChart(string position, PlayerDto player);
        Task<IEnumerable<PlayerDto>> GetBackups(string position);
        Task<IEnumerable<PlayerDto>> GetFullDepthChart();
    }
}
