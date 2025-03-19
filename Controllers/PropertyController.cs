using AutoMapper;
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
