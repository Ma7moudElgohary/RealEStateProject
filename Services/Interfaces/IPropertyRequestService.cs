using RealEstate.Models;
using RealEStateProject.ViewModels.Property;

namespace RealEstate.Services
{
    public interface IPropertyRequestService : IBaseService<PropertyRequest, PropertyRequestViewModel>
    {
        Task<IEnumerable<PropertyRequestViewModel>> GetRequestsByUserIdAsync(string userId);
        Task<IEnumerable<PropertyRequestViewModel>> GetRequestsByAgentIdAsync(string agentId);
        Task<int> AddRequestAsync(PropertyRequest request);
        Task UpdateRequestStatusAsync(int requestId, RequestStatus status, string agentId);
    }
}