using RealEstate.Models;
using RealEstate.Repositories;
using RealEstate.Services;
using RealEStateProject.ViewModels.Common;
using RealEStateProject.ViewModels.Property;
using AutoMapper;

namespace RealEStateProject.Services.Implementation
{
    public class PropertyService : BaseService<Property, PropertyViewModel>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IFavoriteRepository favoriteRepository,
            IMapper mapper)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<int> AddPropertyAsync(PropertyViewModel propertyViewModel, string agentId)
        {
            var property = new Property
            {
                Title = propertyViewModel.Title,
                Description = propertyViewModel.Description,
                Price = propertyViewModel.Price,
                Area = propertyViewModel.SquareFeet,
                Bedrooms = propertyViewModel.Bedrooms,
                Bathrooms = propertyViewModel.Bathrooms,
                Address = propertyViewModel.Address,
                City = propertyViewModel.City,
                State = propertyViewModel.State,
                ZipCode = propertyViewModel.ZipCode,
                Latitude = propertyViewModel.Latitude,
                Longitude = propertyViewModel.Longitude,
                Type = propertyViewModel.Type,
                Status = propertyViewModel.Status,
                AgentId = agentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _propertyRepository.AddAsync(property);
        }

        public async Task<IEnumerable<PropertyViewModel>> GetAllPropertiesAsync(string userId)
        {
            var properties = await _propertyRepository.GetAllAsync();
            return await MapPropertiesToViewModels(properties, userId);
        }

        public async Task<PropertyViewModel> GetPropertyByIdAsync(int id, string userId)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
            {
                return null;
            }

            bool isFavorite = false;
            if (!string.IsNullOrEmpty(userId))
            {
                isFavorite = await _favoriteRepository.IsFavoriteAsync(id, userId);
            }

            return MapPropertyToViewModel(property, isFavorite);
        }

        public async Task<IEnumerable<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId, string userId)
        {
            var properties = await _propertyRepository.GetByAgentIdAsync(agentId);
            return await MapPropertiesToViewModels(properties, userId);
        }

        public async Task<PropertySearchViewModel> SearchPropertiesAsync(PropertySearchFilterViewModel filter, string userId, int page, int pageSize)
        {
            // Apply filters
            var properties = await _propertyRepository.SearchAsync(filter);

            // Calculate pagination
            int totalItems = properties.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Apply pagination
            var paginatedProperties = properties
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            // Map to view models and check favorites
            var propertyViewModels = await MapPropertiesToViewModels(paginatedProperties, userId);

            return new PropertySearchViewModel
            {
                Properties = propertyViewModels.ToList(),
                Filters = filter,
                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    PageSize = pageSize
                }
            };
        }

        public async Task UpdatePropertyAsync(PropertyViewModel propertyViewModel, string agentId)
        {
            var property = await _propertyRepository.GetByIdAsync(propertyViewModel.Id);
            if (property == null)
            {
                throw new KeyNotFoundException($"Property with ID {propertyViewModel.Id} not found.");
            }
            if (property.AgentId != agentId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this property.");
            }

            property.Title = propertyViewModel.Title;
            property.Description = propertyViewModel.Description;
            property.Price = propertyViewModel.Price;
            property.Area = propertyViewModel.SquareFeet;
            property.Bedrooms = propertyViewModel.Bedrooms;
            property.Bathrooms = propertyViewModel.Bathrooms;
            property.Address = propertyViewModel.Address;
            property.City = propertyViewModel.City;
            property.State = propertyViewModel.State;
            property.ZipCode = propertyViewModel.ZipCode;
            property.Type = propertyViewModel.Type;
            property.Status = propertyViewModel.Status;
            property.UpdatedAt = DateTime.UtcNow;

            await _propertyRepository.UpdateAsync(property);
        }

        public async Task DeletePropertyAsync(int id, string agentId)
        {
            var property = await _propertyRepository.GetByIdAsync(id);

            if (property == null)
            {
                throw new KeyNotFoundException($"Property with ID {id} not found.");
            }

            if (property.AgentId != agentId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this property.");
            }

            await _propertyRepository.DeleteAsync(property);
        }

        #region Private Helper Methods
        private PropertyViewModel MapPropertyToViewModel(Property property, bool isFavorite)
        {
            return new PropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                SquareFeet = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                Address = property.Address,
                City = property.City,
                State = property.State,
                ZipCode = property.ZipCode,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                Type = property.Type,
                Status = property.Status,
                CreatedAt = property.CreatedAt,
                AgentName = $"{property.Agent?.FirstName} {property.Agent?.LastName}".Trim(),
                IsFavorite = isFavorite,
                ImageUrls = property.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
                FeaturedImageUrl = property.Images?.FirstOrDefault(i => i.IsFeatured)?.ImageUrl ?? property.Images?.FirstOrDefault()?.ImageUrl
            };
        }

        private async Task<List<PropertyViewModel>> MapPropertiesToViewModels(IEnumerable<Property> properties, string userId)
        {
            var propertyViewModels = new List<PropertyViewModel>();

            foreach (var property in properties)
            {
                bool isFavorite = false;
                if (!string.IsNullOrEmpty(userId))
                {
                    isFavorite = await _favoriteRepository.IsFavoriteAsync(property.Id, userId);
                }

                propertyViewModels.Add(MapPropertyToViewModel(property, isFavorite));
            }

            return propertyViewModels;
        }

        public Task UpdatePropertyImagesAsync(PropertyImagesViewModel viewModel)
        {
            throw new NotImplementedException();

        }

        public Task DeletePropertyImageAsync(int propertyId, string imageUrl)
        {
            throw new NotImplementedException();
        }
        #endregion




    }
}