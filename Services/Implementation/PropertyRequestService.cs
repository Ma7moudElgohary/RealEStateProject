using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;
using RealEstate.Repositories;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.Services.Implementation
{
    public class PropertyRequestService : IPropertyRequestService
    {
        #region Inject

        private readonly IPropertyRequestRepository _propertyRequestRepository;
        private readonly IMapper _mapper;

        public PropertyRequestService(IPropertyRequestRepository propertyRequestRepository, IMapper mapper)
        {
            _propertyRequestRepository = propertyRequestRepository;
            _mapper = mapper;
        }
        #endregion

        private void ValidatePropertyRequest(PropertyRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Property request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.UserId))
                throw new ArgumentException("UserId is required.");

            if (request.PropertyId <= 0)
                throw new ArgumentException("Invalid PropertyId.");
        }

        public async Task<int> AddRequestAsync(PropertyRequest request)
        {
            ValidatePropertyRequest(request);
            await _propertyRequestRepository.AddAsync(request);
            return request.Id;
        }

        public async Task<int> CountAsync()
        {
            return await _propertyRequestRepository.CountAsync();
        }

        public async Task<PropertyRequestViewModel> CreateAsync(PropertyRequestViewModel viewModel)
        {
            var request = _mapper.Map<PropertyRequest>(viewModel);
            await _propertyRequestRepository.AddAsync(request);
            return _mapper.Map<PropertyRequestViewModel>(request);
        }

        public async Task DeleteAsync(object id)
        {
            await _propertyRequestRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(object id)
        {
            return await _propertyRequestRepository.ExistsAsync(r => r.Id == (int)id);
        }

        public async Task<IEnumerable<PropertyRequestViewModel>> GetAllAsync()
        {
            var requests = await _propertyRequestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PropertyRequestViewModel>>(requests);
        }

        public async Task<PropertyRequestViewModel> GetByIdAsync(object id)
        {
            var request = await _propertyRequestRepository.GetByIdAsync((int)id);
            return _mapper.Map<PropertyRequestViewModel>(request);
        }

        public async Task<IEnumerable<PropertyRequestViewModel>> GetRequestsByAgentIdAsync(string agentId)
        {
            var requests = await _propertyRequestRepository.GetByAgentIdAsync(agentId);
            return _mapper.Map<IEnumerable<PropertyRequestViewModel>>(requests);
        }

        public async Task<IEnumerable<PropertyRequestViewModel>> GetRequestsByUserIdAsync(string userId)
        {
            var requests = await _propertyRequestRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PropertyRequestViewModel>>(requests);
        }

        public async Task UpdateAsync(PropertyRequestViewModel viewModel)
        {
            var request = await _propertyRequestRepository.GetByIdAsync(viewModel.Id);
            if (request == null)
                throw new KeyNotFoundException("Property request not found.");

            _mapper.Map(viewModel, request);
            await _propertyRequestRepository.UpdateAsync(request);
        }

        public async Task UpdateRequestStatusAsync(int requestId, RequestStatus status, string agentId)
        {
            var request = await _propertyRequestRepository.GetByIdAsync(requestId);
            if (request == null)
                throw new KeyNotFoundException("Property request not found.");

            request.Status = status;
            request.UserId = agentId;
            await _propertyRequestRepository.UpdateAsync(request);
        }
    }
}
