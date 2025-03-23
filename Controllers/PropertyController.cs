using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using RealEstate.Models;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;
using RealEStateProject.Services.Interfaces;


namespace RealEstate.Controllers
{
    //[Authorize]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IFavoriteService _favoriteService;
        private readonly IMessageService _messageService;
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(
            IPropertyService propertyService,
            IFavoriteService favoriteService,
            IMessageService messageService,
            IAgentService agentService,
            IMapper mapper,
            ILogger<PropertyController> logger)
        {
            _propertyService = propertyService;
            _favoriteService = favoriteService;
            _messageService = messageService;
            _agentService = agentService;
            _mapper = mapper;
            _logger = logger;
        }


        // GET: Properties
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var properties = await _propertyService.GetPropertiesByAgentIdAsync(userId, userId);
            return View(properties);
        }

        // GET: Search
        [AllowAnonymous]
        public async Task<IActionResult> Search(PropertySearchFilterViewModel filter, int page = 1, int pageSize = 10)
        {
            var userId = User.Identity.IsAuthenticated ? GetUserId() : null;
            var searchResults = await _propertyService.SearchPropertiesAsync(filter, userId, page, pageSize);

            if (userId != null)
            {
                var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
                ViewBag.FavoriteIds = favorites.Select(f => f.PropertyId).ToList();
            }
            else
            {
                ViewBag.FavoriteIds = new List<int>();
            }

            ViewBag.PropertyTypes = _propertyService.GetEnumSelectList<PropertyType>();

            return View(searchResults);
        }

        // GET: Properties/Details/id
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Identity.IsAuthenticated ? GetUserId() : null;
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);

            property.Agent = await _agentService.GetAgentByPropertyIdAsync(id);

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

        // GET: Properties/Create
        [Authorize(Roles = "Agent")]
        public IActionResult Create()
        {
            var viewModel = new PropertyViewModel
            {
                PropertyTypes = _propertyService.GetEnumSelectList<PropertyType>(),

                PropertyStatuses = _propertyService.GetEnumSelectList<PropertyStatus>()
            };
            return View(viewModel);
        }

        // POST: Properties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PropertyTypes = _propertyService.GetEnumSelectList<PropertyType>();
                viewModel.PropertyStatuses = _propertyService.GetEnumSelectList<PropertyStatus>();
                return View(viewModel);
            }
            // Get the current user ID
            var userId = GetUserId();

            await _propertyService.AddPropertyAsync(viewModel, userId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Properties/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);

            if (property == null)
            {
                return NotFound();
            }

            property.PropertyTypes = _propertyService.GetEnumSelectList<PropertyType>();
            property.PropertyStatuses = _propertyService.GetEnumSelectList<PropertyStatus>();
            return View(property);
        }

        // POST: Properties/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PropertyTypes = _propertyService.GetEnumSelectList<PropertyType>();
                viewModel.PropertyStatuses = _propertyService.GetEnumSelectList<PropertyStatus>();
                return View(viewModel);
            }

            var userId = GetUserId();
            await _propertyService.UpdatePropertyAsync(viewModel, userId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Properties/Delete/id
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

        // GET: Properties/Explore
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
            var searchResults = await _propertyService.SearchPropertiesAsync(filter, userId, 1, 20); _propertyService.GetEnumSelectList<PropertyType>();
            if (userId != null)
            {
                var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
                ViewBag.FavoriteIds = favorites.Select(f => f.PropertyId).ToList();
            }
            else
            {
                ViewBag.FavoriteIds = new List<int>();
            }

            ViewBag.PropertyTypes = _propertyService.GetEnumSelectList<PropertyType>();

            return View("Search", searchResults);
        }
        // GET: Properties/ToggleFavoriteAjax
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

        // remove fevorite
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFavorite(int propertyId)
        {
            var userId = GetUserId();
            await _favoriteService.RemoveFavoriteAsync(propertyId, userId);
            return RedirectToAction(nameof(Favorites));
        }
        // GET: Properties/Favorites
        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = GetUserId();
            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);

            return View(favorites);
        }

        // POST: Properties/ContactAgent
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

        // GET: Properties/Messages
        [Authorize]
        public async Task<IActionResult> Messages()
        {
            var userId = GetUserId();
            var messages = await _messageService.GetMessagesByAgentIdAsync(userId);
            return View(messages);
        }

        // POST: Properties/MarkMessageAsRead
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            await _messageService.MarkAsReadAsync(id);
            return RedirectToAction(nameof(Messages));
        }

        // POST: Properties/DeleteMessage
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _messageService.DeleteAsync(id);
            return RedirectToAction(nameof(Messages));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}