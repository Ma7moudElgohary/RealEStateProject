using AutoMapper;
using RealEstate.Models;
using RealEstate.Repositories;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;
using RealEStateProject.Repositories.Implementation;
using RealEStateProject.Models;
using RealEStateProject.ViewModels.Agent;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEStateProject.Services.Implementation
{

    public class FavoriteService : BaseService<Favorite, PropertyViewModel>, IFavoriteService
    {
        /// <summary>
        /// /mapper
        /// </summary>
        private readonly IFavoriteRepository _repository;
        private readonly IMapper _mapper;
        public FavoriteService(IFavoriteRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task AddFavoriteAsync(int propertyId, string userId)
        {
            // Create a new Favorite object
            var favorite = new Favorite
            {
                PropertyId = propertyId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            // Add the favorite to the repository
            await _repository.AddAsync(favorite);

            // SaveChanges is a separate operation, use 'await' to complete it
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PropertyFavoriteViewModel>> GetFavoritesByUserIdAsync(string userId)
        {
            var favorites = await _repository.GetAllByUserIdAsync(userId);

            var result = new List<PropertyFavoriteViewModel>();
            foreach (var favorite in favorites)
            {
                result.Add(FromFavorite(favorite));
            }
            return result;
        }

        // In FavoriteService.cs
        public async Task<bool> IsFavoriteAsync(int propertyId, string userId)
        {
            // Input validation
            if (propertyId <= 0)
            {
                throw new ArgumentException("Invalid property ID", nameof(propertyId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                return false; // Cannot be a favorite if user ID is null or empty
            }

            try
            {
                // Make sure _favoriteRepository is not null before using it
                if (_repository == null)
                {
                    throw new InvalidOperationException("Favorite repository is not initialized");
                }

                // Call the repository method to check if the property is a favorite
                return await _repository.IsFavoriteAsync(propertyId, userId);
            }
            catch (Exception ex)
            {
                // Log the exception (replace with your actual logging mechanism)
                // _logger.LogError(ex, $"Error checking favorite status for property {propertyId} and user {userId}");

                // Return false as a fallback instead of propagating the exception
                return false;
            }
        }

        public async Task RemoveFavoriteAsync(int propertyId, string userId)
        {
            await _repository.DeleteAsync(propertyId, userId);
        }


        public static PropertyFavoriteViewModel FromFavorite(Favorite favorite)
        {
            var property = favorite.Property;
            return new PropertyFavoriteViewModel
            {
                FavoriteId = favorite.Id,
                UserId = favorite.UserId,
                DateSaved = favorite.CreatedAt,

                PropertyId = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,

                Address = property.Address,
                City = property.City,
                State = property.State,
                ZipCode = property.ZipCode,

                SquareFeet = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,

                Type = property.Type,
                Status = property.Status,
                Cover = property.FeaturedImage,

            };
        }
    }
}
