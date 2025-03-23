using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using RealEstate.Models;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;
using Microsoft.EntityFrameworkCore;
using RealEStateProject.Models;
using RealEStateProject.Services.Interfaces;
using RealEStateProject.Repositories.Interfaces;
using RealEStateProject.Repositories;

namespace RealEstate.Controllers
{
    //[Authorize]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IFavoriteService _favoriteService;
        private readonly IMessageService _messageService;
        private readonly IAgentRepository _agentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(
            IPropertyService propertyService,
            IFavoriteService favoriteService,
            IMessageService messageService,
            IAgentRepository agentRepository,
            IMapper mapper,
            ILogger<PropertyController> logger)
        {
            _propertyService = propertyService;
            _favoriteService = favoriteService;
            _messageService = messageService;
            _agentRepository = agentRepository;
            _mapper = mapper;
            _logger = logger;
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

            // Add this if the user is authenticated
            if (userId != null)
            {
                var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
                ViewBag.FavoriteIds = favorites.Select(f => f.PropertyId).ToList();
            }
            else
            {
                ViewBag.FavoriteIds = new List<int>();
            }

            ViewBag.PropertyTypes = GetEnumSelectList<PropertyType>();

            return View(searchResults);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Identity.IsAuthenticated ? GetUserId() : null;
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);

            var propertyViewModel = await _propertyService.GetPropertyViewModelByIdAsync(id);

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
                Images = (IEnumerable<string>)property.ImageUrls
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
        [HttpPost]
        public async Task<IActionResult> ToggleFavoriteAjax(int propertyId)
        {
            var userId = GetUserId();
            var isFavorite = await _favoriteService.IsFavoriteAsync(propertyId, userId);

            if (isFavorite)
            {
                await _favoriteService.RemoveFavoriteAsync(propertyId, userId);
                isFavorite = false;
            }
            else
            {
                await _favoriteService.AddFavoriteAsync(propertyId, userId);
                isFavorite = true;
            }

            return Json(new { isFavorite });
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = GetUserId();
            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);

            return View(favorites);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactAgent(ContactAgentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please fill all required fields." });
            }

            try
            {
                // Make sure the AgentId is set if it's not coming from the form
                if (string.IsNullOrEmpty(model.AgentId))
                {
                    // Get the property to find its agent
                    var propertyViewModel = await _propertyService.GetPropertyByIdAsync(model.PropertyId, null);

                    if (propertyViewModel != null)
                    {
                        // Use the AgentId from the PropertyViewModel
                        model.AgentId = propertyViewModel.AgentId;
                    }
                    else
                    {
                        return Json(new { success = false, message = "Property not found." });
                    }
                }

                await _messageService.SendContactMessageAsync(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending contact message: " + ex.Message);
                return Json(new { success = false, message = "An error occurred while sending your message." });
            }
        }

        [Authorize]
        public async Task<IActionResult> Messages()
        {
            var userId = GetUserId();
            var messages = await _messageService.GetMessagesByAgentIdAsync(userId);
            return View(messages);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            await _messageService.MarkAsReadAsync(id);
            return RedirectToAction(nameof(Messages));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _messageService.DeleteAsync(id);
            return RedirectToAction(nameof(Messages));
        }

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> RemoveFavorite(int id)
        //{
        //    var userId = GetUserId();
        //    var favorite = await _favoriteService.GetFavoriteByIdAsync(id);

        //    // Check if the favorite exists and belongs to the current user
        //    if (favorite != null && favorite.UserId == userId)
        //    {
        //        await _favoriteService.RemoveFavoriteAsync(id);
        //    }

        //    return RedirectToAction("Favorites");
        //}
    }
}