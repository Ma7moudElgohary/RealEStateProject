using RealEstate.Models;
using RealEstate.Repositories;
using RealEstate.Services;
using RealEStateProject.ViewModels.Common;
using RealEStateProject.ViewModels.Property;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AutoMapper.Features;
using RealEStateProject.Repositories.Interfaces;

namespace RealEStateProject.Services.Implementation
{
    public class PropertyService : BaseService<Property, PropertyViewModel>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IUserService _userService;
        private readonly IReviewRepository _reviewRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IFavoriteRepository favoriteRepository,
            IPropertyImageRepository propertyImageRepository,
            IWebHostEnvironment webHostEnvironment,
            IUserService userService,
            IReviewRepository reviewRepository,
            IMapper mapper)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _favoriteRepository = favoriteRepository;
            _propertyImageRepository = propertyImageRepository;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
            _reviewRepository = reviewRepository;
        }

        public async Task<int> AddPropertyAsync(PropertyViewModel propertyViewModel, string agentId)
        {
            // Create the property entity
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
                UpdatedAt = DateTime.UtcNow,
                Images = new List<PropertyImage>()
            };

            // Save featured image if provided
            if (propertyViewModel.ImageUpload != null)
            {
                string featuredImagePath = await SaveImageAsync(propertyViewModel.ImageUpload, 0); // Temp ID
                property.FeaturedImage = featuredImagePath;
            }

            // Save additional images if provided
            if (propertyViewModel.AdditionalImages != null && propertyViewModel.AdditionalImages.Count() > 0)
            {
                List<string> imagePaths = new List<string>();
                foreach (var image in propertyViewModel.AdditionalImages)
                {
                    // Save with temp ID for now
                    string imagePath = await SaveImageAsync(image, 0);
                    imagePaths.Add(imagePath);
                }

                // Save property first to get the ID
                int propertyId = await _propertyRepository.AddAsync(property);

                // Update the property's featured image location if needed
                if (!string.IsNullOrEmpty(property.FeaturedImage))
                {
                    // Move the image to the correct folder with the real property ID
                    string newPath = await MoveImageToCorrectFolder(property.FeaturedImage, propertyId);
                    property.FeaturedImage = newPath;
                }

                // Now create PropertyImage objects with the correct property ID
                foreach (var imagePath in imagePaths)
                {
                    // Move the image to the correct folder with the real property ID
                    string newPath = await MoveImageToCorrectFolder(imagePath, propertyId);

                    property.Images.Add(new PropertyImage
                    {
                        PropertyId = propertyId,
                        ImageUrl = newPath
                    });
                }

                // Update the property with the new images
                await _propertyRepository.UpdateAsync(property);

                return propertyId;
            }
            else
            {
                // If no additional images, just save the property
                return await _propertyRepository.AddAsync(property);
            }
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


        public async Task<int> UpdatePropertyAsync(PropertyViewModel model, string userId)
        {
            var property = await _propertyRepository.GetByIdAsync(model.Id);

            if (property == null || property.AgentId != userId)
            {
                return 0;
            }

            // Store original featured image
            string oldFeaturedImage = property.FeaturedImage;

            // Map the view model to the property entity
            property.Title = model.Title;
            property.Description = model.Description;
            property.Type = model.Type;
            property.Status = model.Status;
            property.Price = model.Price;
            property.Area = model.SquareFeet;
            property.Bedrooms = model.Bedrooms;
            property.Bathrooms = model.Bathrooms;
            property.Address = model.Address;
            property.City = model.City;
            property.State = model.State;
            property.ZipCode = model.ZipCode;
            property.Latitude = model.Latitude;
            property.Longitude = model.Longitude;
            property.Agent.FirstName = model.AgentName;
            property.Agent.PhoneNumber = model.AgentPhone;
            property.Agent.Email = model.AgentEmail;
            property.AgentId = userId;

            // Handle the featured image upload
            if (model.ImageUpload != null && model.ImageUpload.Length > 0)
            {
                // Delete old featured image
                if (!string.IsNullOrEmpty(oldFeaturedImage))
                {
                    DeleteImageFromServer(oldFeaturedImage);
                }
                // Save new featured image
                property.FeaturedImage = await SaveImageAsync(model.ImageUpload, property.Id);
            }
            else
            {
                property.FeaturedImage = oldFeaturedImage;
            }

            // Handle additional images upload
            if (model.AdditionalImages != null && model.AdditionalImages.Count() > 0)
            {
                foreach (var image in model.AdditionalImages)
                {
                    string imagePath = await SaveImageAsync(image, property.Id);
                    await _propertyImageRepository.AddAsync(new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImageUrl = imagePath
                    });
                }
            }

            await _propertyRepository.UpdateAsync(property);
            return property.Id;
        }

        // In your UpdatePropertyAsync method
        //public async Task UpdatePropertyAsync(PropertyViewModel model, string userId)
        //{
        //    var property = await _propertyRepository.GetByIdAsync(model.Id);

        //    if (property == null || property.AgentId != userId)
        //    {
        //        throw new Exception("Property not found or you don't have permission to edit it");
        //    }

        //    // Update property fields
        //    property.Title = model.Title;
        //    property.Description = model.Description;
        //    property.Type = model.Type;
        //    property.Status = model.Status;
        //    property.Price = model.Price;
        //    property.Area = model.SquareFeet;
        //    property.Bedrooms = model.Bedrooms;
        //    property.Bathrooms = model.Bathrooms;
        //    property.Address = model.Address;
        //    property.City = model.City;
        //    property.State = model.State;
        //    property.ZipCode = model.ZipCode;
        //    property.Latitude = model.Latitude;
        //    property.Longitude = model.Longitude;
        //    property.Agent.FirstName = model.AgentName;
        //    property.Agent.PhoneNumber = model.AgentPhone;
        //    property.Agent.Email = model.AgentEmail;

        //    // Process image if uploaded
        //    if (model.ImageUpload != null && model.ImageUpload.Length > 0)
        //    {
        //        if (!string.IsNullOrEmpty(property.FeaturedImage))
        //        {
        //            DeleteImageFromServer(property.FeaturedImage);
        //        }

        //        string featuredImagePath = await SaveImageAsync(model.ImageUpload, property.Id);
        //        property.FeaturedImage = featuredImagePath;
        //    }

        //    await _propertyRepository.UpdateAsync(property);

        //}

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

            // Delete featured image
            if (!string.IsNullOrEmpty(property.FeaturedImage))
            {
                DeleteImageFromServer(property.FeaturedImage);
            }

            // Delete additional images
            if (property.Images != null && property.Images.Count > 0)
            {
                foreach (var image in property.Images)
                {
                    DeleteImageFromServer(image.ImageUrl);
                }
                await _propertyImageRepository.DeletePropertyImagesAsync(id);
            }

            await _propertyRepository.DeleteAsync(property);
        }

        public async Task UpdatePropertyImagesAsync(PropertyImagesViewModel viewModel)
        {
            var property = await _propertyRepository.GetByIdAsync(viewModel.Id);
            if (property == null)
            {
                throw new KeyNotFoundException($"Property with ID {viewModel.Id} not found.");
            }

            // Update featured image if provided
            if (viewModel.FeaturedImage != null)
            {
                if (!string.IsNullOrEmpty(property.FeaturedImage))
                {
                    DeleteImageFromServer(property.FeaturedImage);
                }

                string featuredImagePath = await SaveImageAsync(viewModel.FeaturedImage, property.Id);
                property.FeaturedImage = featuredImagePath;
                await _propertyRepository.UpdateAsync(property);
            }

            // Save new additional images if provided
            if (viewModel.NewImages != null && viewModel.NewImages.Count > 0)
            {
                foreach (var image in viewModel.NewImages)
                {
                    string imagePath = await SaveImageAsync(image, property.Id);
                    await _propertyImageRepository.AddAsync(new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImageUrl = imagePath
                    });
                }
            }
        }

        public async Task DeletePropertyImageAsync(int propertyId, string imageUrl)
        {
            // Find the image in the database
            var image = await _propertyImageRepository.GetByUrlAsync(propertyId, imageUrl);
            if (image == null)
            {
                throw new KeyNotFoundException($"Image not found for property ID {propertyId}.");
            }

            // Delete from server
            DeleteImageFromServer(imageUrl);

            // Delete from database
            await _propertyImageRepository.DeleteAsync(image);
        }

        #region Private Helper Methods
        private PropertyViewModel MapPropertyToViewModel(Property property, bool isFavorite)
        {
            var imageUrls = property.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>();

            return new PropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                AgentId = property.AgentId,
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
                FeaturedImageUrl = property.FeaturedImage,
                ImageUrls = imageUrls
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

        private async Task<string> SaveImageAsync(IFormFile imageFile, int propertyId)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "properties", propertyId.ToString());

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique filename
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return the relative path for storage in the database
            return $"/images/properties/{propertyId}/{uniqueFileName}";
        }

        private void DeleteImageFromServer(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            try
            {
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception)
            {
                // Log the error but don't throw an exception
                // Implement proper logging here
            }
        }


        private async Task<string> MoveImageToCorrectFolder(string tempPath, int propertyId)
        {
            // Get the filename from the temp path
            string fileName = Path.GetFileName(tempPath);

            // Create the path with the correct property ID
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "properties", propertyId.ToString());

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Calculate new path
            string newPath = Path.Combine(uploadsFolder, fileName);

            // Get full path of temp file
            string fullTempPath = Path.Combine(_webHostEnvironment.WebRootPath, tempPath.TrimStart('/'));

            // Move the file
            if (File.Exists(fullTempPath))
            {
                // Create a copy in the new location
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    using (var sourceStream = new FileStream(fullTempPath, FileMode.Open))
                    {
                        await sourceStream.CopyToAsync(stream);
                    }
                }

                // Delete the original
                File.Delete(fullTempPath);
            }

            // Return the new relative path
            return $"/images/properties/{propertyId}/{fileName}";
        }


        public async Task<PropertyViewModel> GetPropertyViewModelByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
                return null;

            // Get all reviews for this property
            var reviews = await _reviewRepository.GetAllByPropertyIdAsync(id);
            var reviewViewModels = new List<ViewModels.Property.ReviewViewModel>();
            var ratingDistribution = new Dictionary<int, int>();

            // Initialize rating distribution
            for (int i = 1; i <= 5; i++)
            {
                ratingDistribution[i] = 0;
            }

            foreach (var review in reviews)
            {
                var user = await _userService.GetUserByIdAsync(review.UserId);

                reviewViewModels.Add(new ViewModels.Property.ReviewViewModel
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreatedAt = review.CreatedAt,
                    UserId = review.UserId,
                    UserName = user?.UserName ?? "Anonymous"
                });

                // Update rating distribution
                if (ratingDistribution.ContainsKey(review.Rating))
                {
                    ratingDistribution[review.Rating]++;
                }
            }

            // Get average rating
            var averageRating = await _reviewRepository.GetAverageRatingAsync(id);

            return new PropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                Address = property.Address,
                City = property.City,
                State = property.State,
                ZipCode = property.ZipCode,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                SquareFeet = property.Area,
                //YearBuilt = property.YearBuilt,
                Status = property.Status,
                Type = property.Type,
                AgentId = property.AgentId,
                AgentName = property.Agent?.FirstName,
                AgentEmail = property.Agent?.Email,
                AgentPhone = property.Agent?.PhoneNumber,
                Reviews = reviewViewModels,
                AverageRating = averageRating,
                ReviewCount = reviewViewModels.Count,
                RatingDistribution = ratingDistribution
            };
        }
        #endregion
    }
}