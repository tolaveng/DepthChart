using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IPlayerRepository
    {
        Task<Player> GetAsync(int playerNumber);
        Task<IEnumerable<Player>> GetAllAsync();
        Task<bool> InsertAsync(Player player);
        Task<bool> UpdateAsync(Player player);
        Task<bool> DeleteAsync(int playerNumber);

        Task<bool> RemoveAllAsync();
    }
}
