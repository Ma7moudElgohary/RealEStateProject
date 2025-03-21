using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEStateProject.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Services;

namespace RealEStateProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IPropertyService _propertyService;
        private readonly IFavoriteService _favoriteService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public UserController(
            IAgentService agentService,
            IPropertyService propertyService,
            IFavoriteService favoriteService,
            IReviewService reviewService,
            IUserService userService)
        {
            _agentService = agentService;
            _propertyService = propertyService;
            _favoriteService = favoriteService;
            _reviewService = reviewService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview([Bind("Rating,Comment,PropertyId")] Review review)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            // Get the current user ID
            review.UserId = User.FindFirst("sub")?.Value ?? User.Identity.Name;

            // Create the review
            await _reviewService.CreateAsync(review);

            return RedirectToAction("Details", "Property", new { Id = review.PropertyId });
        }

        public async Task<IActionResult> RemoveReview(int reviewId)
        {
            var review = await _reviewService.GetByIdAsync(reviewId);

            if (review == null)
            {
                return NotFound();
            }

            // Store property ID before deletion
            var propertyId = review.PropertyId;

            // Ensure the review belongs to the current user
            string currentUserId = User.FindFirst("sub")?.Value ?? User.Identity.Name;
            if (review.UserId != currentUserId)
            {
                return Forbid();
            }

            int result = await _reviewService.DeleteAsync(reviewId);

            if (result > 0)
            {
                return RedirectToAction("Details", "Property", new { Id = propertyId });
            }

            return BadRequest();
        }
    }
}