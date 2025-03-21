using RealEstate.Models;
using RealEStateProject.ViewModels.Property;

namespace RealEstate.Repositories
{
    public interface IPropertyRepository : IBaseRepository<Property>
    {
        Task<int> AddAsync(Property entity);
        Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId);
        Task<IEnumerable<Property>> SearchAsync(PropertySearchFilterViewModel filter);
    }
}