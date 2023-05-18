using Domain.Entities;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IPositionRepository
    {
        Task<Position> GetAsync(string position);
        Task<bool> InsertAsync(Position position);
    }
}
