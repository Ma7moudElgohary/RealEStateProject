using AutoMapper;
using RealEstate.Models;
using RealEstate.Repositories;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.Services.Implementation
{
    public class PropertyService : IPropertyRequestService
    {


        #region Inject

        private readonly IPropertyRequestRepository _propertyRequestRepository;

        private readonly IMapper _mapper;

        public PropertyService(IPropertyRequestRepository _propertyRequestRepository, IMapper _mapper)
        {
            this._propertyRequestRepository = _propertyRequestRepository;

            this._mapper = _mapper;

        }
        #endregion


        public async Task<int> AddRequestAsync(PropertyRequest request)
        {
           await _propertyRequestRepository.AddAsync(request);   

            return request.Id;
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PropertyRequestViewModel> CreateAsync(PropertyRequestViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyRequestViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PropertyRequestViewModel> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyRequestViewModel>> GetRequestsByAgentIdAsync(string agentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyRequestViewModel>> GetRequestsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PropertyRequestViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRequestStatusAsync(int requestId, RequestStatus status, string agentId)
        {
            throw new NotImplementedException();
        }
    }
}
