using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public interface IDepthChart
    {
        Task<string> GetFullDepthChart();
        Task<string> GetBackups(string position, Player player);
        Task<bool> AddPlayerToDepthChart(string position, Player player, int? depth);
        Task<string> RemovePlayerFromDepthChart(string position, Player player);
    }
}
