using RealEstate.Models;

namespace RealEstate.Repositories
{
    public interface IPropertyImageRepository : IBaseRepository<PropertyImage>
    {
        Task<PropertyImage> GetByUrlAsync(int propertyId, string imageUrl);
        Task DeletePropertyImagesAsync(int propertyId);
    }
}