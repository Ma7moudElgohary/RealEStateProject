using RealEstate.Models;
using RealEStateProject.ViewModels.Property;

namespace RealEstate.Services
{
    public interface IPropertyService : IBaseService<Property, PropertyViewModel>
    {
        Task<PropertyViewModel> GetPropertyByIdAsync(int id, string userId);
        Task<IEnumerable<PropertyViewModel>> GetAllPropertiesAsync(string userId);
        Task<IEnumerable<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId, string userId);
        Task<PropertySearchViewModel> SearchPropertiesAsync(PropertySearchFilterViewModel filter, string userId, int page, int pageSize);


        Task<int> AddPropertyAsync(PropertyViewModel property, string agentId);
        Task<int> UpdatePropertyAsync(PropertyViewModel model, string userId);
        Task DeletePropertyAsync(int id, string agentId);


        Task UpdatePropertyImagesAsync(PropertyImagesViewModel viewModel);
        Task DeletePropertyImageAsync(int propertyId, string imageUrl);

        Task<PropertyViewModel> GetPropertyViewModelByIdAsync(int id);

    }
}