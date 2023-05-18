using Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IDepthChartService
    {
        Task<bool> AddPlayerToDepthChartAsync(string position, PlayerDto player, int? depth);
        Task<PlayerDto> RemovePlayerFromDepthChartAsync(string position, PlayerDto player);
        Task<IEnumerable<ChartDto>> GetBackupsAsync(string position, PlayerDto player);
        Task<IEnumerable<ChartDto>> GetFullDepthChartAsync();
    }
}
