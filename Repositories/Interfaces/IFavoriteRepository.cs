using RealEstate.Models;

namespace RealEstate.Repositories
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId);
        Task<bool> IsFavoriteAsync(int propertyId, string userId);
        Task DeleteAsync(int propertyId, string userId);
    }
}