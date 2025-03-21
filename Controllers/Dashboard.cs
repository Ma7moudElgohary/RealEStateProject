using Microsoft.AspNetCore.Mvc;
using RealEStateProject.ViewModels.User;
using RealEStateProject.Services;
using System.Threading.Tasks;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFavoriteService _favoriteService;


        // Inject the service into the controller
        public DashboardController(IUserService userService, IFavoriteService favoriteService)
        {
            _userService = userService;
            _favoriteService = favoriteService;
        }

        // GET: /Dashboard/UserDashboard
        [HttpGet]
        public async Task<IActionResult> UserDashboard()
        {
            string currentUserId = "15cc7cac-a439-4088-bad8-07a2b91a94e0";
            // Validate userId
            if (string.IsNullOrEmpty(currentUserId))
            {
                return NotFound("User ID is required.");
            }

            // Fetch user details
            var user = await _userService.GetUserByIdAsync(currentUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Fetch favorites
            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(currentUserId);

            // Map the data to the ViewModel
            var model = new UserDashboardViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Favorites = (List<PropertyFavoriteViewModel>)favorites
            };

            // Pass the ViewModel to the view
            return View("UserDashboard", model);
        }
    }
}