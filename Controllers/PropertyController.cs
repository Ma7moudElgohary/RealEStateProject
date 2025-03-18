<<<<<<< HEAD
﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using RealEstate.Models;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;

namespace RealEstate.Controllers
{
    //[Authorize]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IFavoriteService _favoriteService;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyService propertyService, IFavoriteService favoriteService, IMapper mapper)
        {
            _propertyService = propertyService;
            _favoriteService = favoriteService;
            _mapper = mapper;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // This method is used to generate a list of SelectListItem objects from an enum type.
        // will add to interface repository and service
        public static List<SelectListItem> GetEnumSelectList<T>()
        {
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var selectList = enumValues.Select((e, i) => new SelectListItem
            {
                Value = i.ToString(),
                Text = e.ToString()
            }).ToList();

            return selectList;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var properties = await _propertyService.GetPropertiesByAgentIdAsync(userId, userId);
            return View(properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(PropertySearchFilterViewModel filter, int page = 1, int pageSize = 10)
        {
            var userId = User.Identity.IsAuthenticated ? GetUserId() : null;
            var searchResults = await _propertyService.SearchPropertiesAsync(filter, userId, page, pageSize);

            ViewBag.PropertyTypes = GetEnumSelectList<PropertyType>();

            return View(searchResults);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Identity.IsAuthenticated ? GetUserId() : null;
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);

            if (property == null)
            {
                return NotFound();
            }

            bool isFavorite = false;
            if (userId != null)
            {
                isFavorite = await _favoriteService.IsFavoriteAsync(id, userId);
            }

            ViewBag.IsFavorite = isFavorite;
            return View(property);
        }

        public IActionResult Create()
        {
            var viewModel = new PropertyViewModel
            {
                PropertyTypes = GetEnumSelectList<PropertyType>(),
                PropertyStatuses = GetEnumSelectList<PropertyStatus>()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PropertyTypes = GetEnumSelectList<PropertyType>();
                viewModel.PropertyStatuses = GetEnumSelectList<PropertyStatus>();
                return View(viewModel);
            }

            var userId = GetUserId();
            await _propertyService.AddPropertyAsync(viewModel, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);

            if (property == null)
            {
                return NotFound();
            }

            property.PropertyTypes = GetEnumSelectList<PropertyType>();
            property.PropertyStatuses = GetEnumSelectList<PropertyStatus>();
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PropertyTypes = GetEnumSelectList<PropertyType>();
                viewModel.PropertyStatuses = GetEnumSelectList<PropertyStatus>();
                return View(viewModel);
            }

            var userId = GetUserId();
            await _propertyService.UpdatePropertyAsync(viewModel, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateImages(int id)
        {
            var userId = GetUserId();
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);

            if (property == null)
            {
                return NotFound();
            }

            var viewModel = new PropertyImagesViewModel
            {
                Id = property.Id,
                Images = property.ImageUrls
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateImages(PropertyImagesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Call image update service
            await _propertyService.UpdatePropertyImagesAsync(viewModel);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int propertyId, string imageUrl)
        {
            await _propertyService.DeletePropertyImageAsync(propertyId, imageUrl);
            return RedirectToAction(nameof(UpdateImages), new { id = propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);

            if (property == null)
            {
                return NotFound();
            }

            await _propertyService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Explore(string query, int? minPrice, int? maxPrice,
                                               int? bedrooms, int? bathrooms, PropertyType? type)
        {
            var filter = new PropertySearchFilterViewModel
            {
                Location = query,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinBedrooms = bedrooms,
                MinBathrooms = bathrooms,
                Type = type
            };

            var userId = User.Identity.IsAuthenticated ? GetUserId() : null;
            var searchResults = await _propertyService.SearchPropertiesAsync(filter, userId, 1, 20);

            ViewBag.PropertyTypes = GetEnumSelectList<PropertyType>();

            return View("Search", searchResults);
        }

        [Authorize]
        public async Task<IActionResult> ToggleFavorite(int propertyId)
        {
            var userId = GetUserId();
            var isFavorite = await _favoriteService.IsFavoriteAsync(propertyId, userId);

            if (isFavorite)
            {
                await _favoriteService.RemoveFavoriteAsync(propertyId, userId);
            }
            else
            {
                await _favoriteService.AddFavoriteAsync(propertyId, userId);
            }

            return RedirectToAction(nameof(Details), new { id = propertyId });
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = GetUserId();
            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
            return View(favorites);
        }
    }
}
=======
﻿//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using RealEstate.Models;
//using RealEstate.Services;
//using RealEStateProject.ViewModels.Common;
//using RealEStateProject.ViewModels.Property;
//using System.Security.Claims;

//namespace RealEStateProject.Controllers
//{
//    public class PropertyController : Controller
//    {
//        private readonly IPropertyService _propertyService;
//        private readonly IFavoriteService _favoriteService;

//        public PropertyController(IPropertyService propertyService, IFavoriteService favoriteService)
//        {
//            _propertyService = propertyService;
//            _favoriteService = favoriteService;
//        }

//        // GET: Property
//        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
//        {
//            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

//            var properties = await _propertyService.GetAllAsync();

//            // Apply pagination manually since we're not using the search method
//            int totalItems = properties.Count();
//            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

//            var paginatedProperties = properties
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            var viewModel = new PropertySearchViewModel
//            {
//                Properties = paginatedProperties,
//                Pagination = new PaginationViewModel
//                {
//                    CurrentPage = page,
//                    TotalPages = totalPages,
//                    TotalItems = totalItems,
//                    PageSize = pageSize
//                },
//                Filters = new PropertySearchFilterViewModel()
//            };

//            return View(viewModel);
//        }

//        // GET: Property/Search
//        public async Task<IActionResult> Search(PropertySearchFilterViewModel filters, int page = 1, int pageSize = 9)
//        {
//            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

//            var result = await _propertyService.SearchPropertiesAsync(filters, userId, page, pageSize);

//            return View("Index", result);
//        }

//        // GET: Property/Details/5
//        public async Task<IActionResult> Details(int id)
//        {
//            if (id <= 0)
//            {
//                return BadRequest("Invalid property ID");
//            }

//            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

//            var property = await _propertyService.GetByIdAsync(id);

//            if (property == null)
//            {
//                return NotFound();
//            }

//            if (!string.IsNullOrEmpty(userId))
//            {
//                property.IsFavorite = await _favoriteService.IsFavoriteAsync(id, userId);
//            }

//            return View(property);
//        }

//        // GET: Property/Create
//        [Authorize(Roles = "Agent")]
//        public IActionResult Create()
//        {
//            return View(new PropertyViewModel());
//        }

//        // POST: Property/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Agent")]
//        public async Task<IActionResult> Create(PropertyViewModel model)
//        {
//            // Custom validation logic
//            ValidatePropertyModel(model);

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//                    if (string.IsNullOrEmpty(agentId))
//                    {
//                        ModelState.AddModelError("", "Agent ID could not be determined. Please log in again.");
//                        return View(model);
//                    }

//                    await _propertyService.CreateWithAgentAsync(model, agentId);
//                    TempData["SuccessMessage"] = "Property was successfully created.";
//                    return RedirectToAction(nameof(MyProperties));
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("", "Error creating property: " + ex.Message);
//                }
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

//        // GET: Property/Edit/5
//        [Authorize(Roles = "Agent")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            if (id <= 0)
//            {
//                return BadRequest("Invalid property ID");
//            }

//            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            if (string.IsNullOrEmpty(agentId))
//            {
//                return Unauthorized("Agent ID could not be determined");
//            }

//            var property = await _propertyService.GetByIdAsync(id);

//            if (property == null)
//            {
//                return NotFound();
//            }

//            // Check if the agent owns this property
//            try
//            {
//                // This is just to test authorization. It will throw an exception if not authorized.
//                await _propertyService.UpdateWithAgentValidationAsync(property, agentId);
//                return View(property);
//            }
//            catch (UnauthorizedAccessException)
//            {
//                return Forbid();
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//        }

//        // POST: Property/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Agent")]
//        public async Task<IActionResult> Edit(int id, PropertyViewModel model)
//        {
//            if (id != model.Id)
//            {
//                ModelState.AddModelError("", "ID mismatch error");
//                return BadRequest();
//            }

//            // Custom validation logic
//            ValidatePropertyModel(model);

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//                    if (string.IsNullOrEmpty(agentId))
//                    {
//                        ModelState.AddModelError("", "Agent ID could not be determined. Please log in again.");
//                        return View(model);
//                    }

//                    await _propertyService.UpdateWithAgentValidationAsync(model, agentId);
//                    TempData["SuccessMessage"] = "Property was successfully updated.";
//                    return RedirectToAction(nameof(MyProperties));
//                }
//                catch (UnauthorizedAccessException)
//                {
//                    ModelState.AddModelError("", "You are not authorized to edit this property.");
//                    return View(model);
//                }
//                catch (KeyNotFoundException)
//                {
//                    return NotFound();
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("", "Error updating property: " + ex.Message);
//                }
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

//        // GET: Property/Delete/5
//        [Authorize(Roles = "Agent")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (id <= 0)
//            {
//                return BadRequest("Invalid property ID");
//            }

//            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            if (string.IsNullOrEmpty(agentId))
//            {
//                return Unauthorized("Agent ID could not be determined");
//            }

//            var property = await _propertyService.GetByIdAsync(id);

//            if (property == null)
//            {
//                return NotFound();
//            }

//            // Check if the agent owns this property (just test authorization)
//            try
//            {
//                await _propertyService.DeleteWithAgentValidationAsync(id, agentId);
//                return View(property);
//            }
//            catch (UnauthorizedAccessException)
//            {
//                return Forbid();
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//        }

//        // POST: Property/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Agent")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (id <= 0)
//            {
//                ModelState.AddModelError("", "Invalid property ID");
//                return BadRequest("Invalid property ID");
//            }

//            try
//            {
//                var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//                if (string.IsNullOrEmpty(agentId))
//                {
//                    ModelState.AddModelError("", "Agent ID could not be determined");
//                    return Unauthorized("Agent ID could not be determined");
//                }

//                await _propertyService.DeleteWithAgentValidationAsync(id, agentId);
//                TempData["SuccessMessage"] = "Property was successfully deleted.";
//                return RedirectToAction(nameof(MyProperties));
//            }
//            catch (UnauthorizedAccessException)
//            {
//                ModelState.AddModelError("", "You are not authorized to delete this property");
//                return Forbid();
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError("", "Error deleting property: " + ex.Message);
//                // Return to delete view with the error
//                var property = await _propertyService.GetByIdAsync(id);
//                return View(property);
//            }
//        }

//        // GET: Property/MyProperties
//        [Authorize(Roles = "Agent")]
//        public async Task<IActionResult> MyProperties()
//        {
//            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            if (string.IsNullOrEmpty(agentId))
//            {
//                ModelState.AddModelError("", "Agent ID could not be determined");
//                return Unauthorized();
//            }

//            var properties = await _propertyService.GetPropertiesByAgentIdAsync(agentId, agentId);
//            return View(properties);
//        }

       
      

       
//        // Private helper methods

//        private void ValidatePropertyModel(PropertyViewModel model)
//        {
//            // Price validation
//            if (model.Price < 0)
//            {
//                ModelState.AddModelError(nameof(model.Price), "Price cannot be negative.");
//            }

//            // Square feet validation
//            if (model.SquareFeet <= 0)
//            {
//                ModelState.AddModelError(nameof(model.SquareFeet), "Square feet must be greater than zero.");
//            }

//            // Bedrooms validation
//            if (model.Bedrooms < 0)
//            {
//                ModelState.AddModelError(nameof(model.Bedrooms), "Bedrooms cannot be negative.");
//            }

//            // Bathrooms validation
//            if (model.Bathrooms < 0)
//            {
//                ModelState.AddModelError(nameof(model.Bathrooms), "Bathrooms cannot be negative.");
//            }

//            // Title validation
//            if (string.IsNullOrWhiteSpace(model.Title))
//            {
//                ModelState.AddModelError(nameof(model.Title), "Title is required.");
//            }
//            else if (model.Title.Length > 100)
//            {
//                ModelState.AddModelError(nameof(model.Title), "Title cannot exceed 100 characters.");
//            }

//            // Description validation
//            if (string.IsNullOrWhiteSpace(model.Description))
//            {
//                ModelState.AddModelError(nameof(model.Description), "Description is required.");
//            }

//            // Address validation
//            if (string.IsNullOrWhiteSpace(model.Address))
//            {
//                ModelState.AddModelError(nameof(model.Address), "Address is required.");
//            }

//            // City validation
//            if (string.IsNullOrWhiteSpace(model.City))
//            {
//                ModelState.AddModelError(nameof(model.City), "City is required.");
//            }

//            // State validation
//            if (string.IsNullOrWhiteSpace(model.State))
//            {
//                ModelState.AddModelError(nameof(model.State), "State is required.");
//            }

//            // Zip code validation
//            if (string.IsNullOrWhiteSpace(model.ZipCode))
//            {
//                ModelState.AddModelError(nameof(model.ZipCode), "Zip code is required.");
//            }
//            else if (!System.Text.RegularExpressions.Regex.IsMatch(model.ZipCode, @"^\d{5}(-\d{4})?$"))
//            {
//                ModelState.AddModelError(nameof(model.ZipCode), "Please enter a valid US zip code (e.g., 12345 or 12345-6789).");
//            }
//        }
//    }
//}
>>>>>>> 63037017522bc22c387cddd18a6a5aa07d66f4af
