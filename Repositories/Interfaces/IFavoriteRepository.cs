using RealEstate.Models;

namespace RealEstate.Repositories
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<Favorite> GetByUserIdAsync(string userId);
        Task<IEnumerable<Favorite>> GetAllByUserIdAsync(string userId);
        Task<bool> IsFavoriteAsync(int propertyId, string userId);
        Task DeleteAsync(int propertyId, string userId);
    }
}