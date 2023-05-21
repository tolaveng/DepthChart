using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IChartRepository
    {
        Task<Chart> GetAsync(Guid id);
        Task<Chart> GetByPlayerNumberAsync(int playerNumber);
        Task<IEnumerable<Chart>> GetAllAsync();
        Task<bool> InsertAsync(Chart chart);
        Task<bool> UpdateAsync(Chart chart);
        Task<bool> DeleteAsync(Guid id);
        Task<Chart> GetLastPositionAsync(string position, string group);
        Task ShiftDepthAsync(string position, string group, int depth);
        Task<IEnumerable<Chart>> GetBackupsAsync(string position, int playerNumber, string group);
        Task<Chart> GetByPlayerAndPositionAsync(int playerNumber, string position, string group);

        Task<bool> RemoveAllAsync();
    }
}
