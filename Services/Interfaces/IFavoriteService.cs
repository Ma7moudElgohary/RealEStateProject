using RealEstate.Models;
using RealEStateProject.ViewModels.Property;

namespace RealEstate.Services
{
    public interface IFavoriteService : IBaseService<Favorite, PropertyViewModel>
    {
        Task<IEnumerable<PropertyViewModel>> GetFavoritesByUserIdAsync(string userId);
        Task<bool> IsFavoriteAsync(int propertyId, string userId);
        Task AddFavoriteAsync(int propertyId, string userId);
        Task RemoveFavoriteAsync(int propertyId, string userId);
    }
}