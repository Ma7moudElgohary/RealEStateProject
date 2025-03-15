using RealEstate.Models;
using RealEStateProject.ViewModels.Property;

namespace RealEstate.Repositories
{
    public interface IPropertyRepository : IBaseRepository<Property>
    {
        Task<Property> GetByIdAsync(int id);
        Task<IEnumerable<Property>> GetAllAsync();
        Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId);
        Task<IEnumerable<Property>> SearchAsync(PropertySearchFilterViewModel filter);
        Task<int> AddAsync(Property property);
        Task UpdateAsync(Property property);
        Task DeleteAsync(int id);
    }
}