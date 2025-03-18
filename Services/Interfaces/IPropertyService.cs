using RealEstate.Models;
using RealEStateProject.ViewModels.Property;

namespace RealEstate.Services
{
    public interface IPropertyService : IBaseService<Property, PropertyViewModel>
    {
        //Task<PropertyViewModel> GetPropertyByIdAsync(int id, string userId);
        //Task<IEnumerable<PropertyViewModel>> GetAllPropertiesAsync(string userId);
        Task<IEnumerable<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId, string userId);
        Task<PropertySearchViewModel> SearchPropertiesAsync(PropertySearchFilterViewModel filter, string userId, int page, int pageSize);

<<<<<<< HEAD

        Task<int> AddPropertyAsync(PropertyViewModel property, string agentId);
        Task UpdatePropertyAsync(PropertyViewModel property, string agentId);
        Task DeletePropertyAsync(int id, string agentId);


        Task UpdatePropertyImagesAsync(PropertyImagesViewModel viewModel);
        Task DeletePropertyImageAsync(int propertyId, string imageUrl);
=======
        
        //Task<int> AddPropertyAsync(PropertyViewModel property, string agentId);
        //Task UpdatePropertyAsync(PropertyViewModel property, string agentId);
        //Task DeletePropertyAsync(int id, string agentId);
>>>>>>> 63037017522bc22c387cddd18a6a5aa07d66f4af
    }
}