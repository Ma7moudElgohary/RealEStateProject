using RealEstate.Models;

namespace RealEstate.Repositories
{
    public interface IPropertyRequestRepository : IBaseRepository<PropertyRequest>
    {
        Task<PropertyRequest> GetByIdAsync(int id);
        Task<IEnumerable<PropertyRequest>> GetByUserIdAsync(string userId);
        Task<IEnumerable<PropertyRequest>> GetByPropertyIdAsync(int propertyId);
        Task<IEnumerable<PropertyRequest>> GetByAgentIdAsync(string agentId);
        Task<int> AddAsync(PropertyRequest request);
        Task UpdateAsync(PropertyRequest request);
    }
}