using RealEstate.Models;

namespace RealEstate.Repositories
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<Favorite> GetByIdAsync(int id);
        Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId);
        Task<bool> IsFavoriteAsync(int propertyId, string userId);
        Task<int> AddAsync(Favorite favorite);
        Task DeleteAsync(int propertyId, string userId);
    }
}