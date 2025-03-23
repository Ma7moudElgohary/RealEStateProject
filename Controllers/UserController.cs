using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEStateProject.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Services;
using System.Security.Claims;
using RealEStateProject.ViewModels.User;

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

        // GET: User profile
        public IActionResult Index()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUserByIdAsync(Id).Result;

            return View(user);
        }

        // POST: Add Review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview([Bind("Rating,Comment,PropertyId")] Review review)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }
                return View(ModelState);
            }

            try
            {
                // Get the current user ID
                review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create the review
                await _reviewService.CreateAsync(review);

                // If AJAX request, return success status
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Details", "Property", new { Id = review.PropertyId });
            }
            catch (Exception ex)
            {
                // Log the exception (ideally to a file or logging service)
                Console.WriteLine(ex.ToString()); // For debugging

                // Get inner exception message if it exists
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = errorMessage });
                }
                ModelState.AddModelError("", "Error creating review: " + errorMessage);
                return View(ModelState);
            }
        }


        // POST: Remove review
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            // If AJAX request, return success status
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = result > 0 });
            }

            if (result > 0)
            {
                return RedirectToAction("Details", "Property", new { Id = propertyId });
            }

            return BadRequest();
        }

        // GET: Get reviews for a property
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyReviews(int propertyId, int page = 1, int pageSize = 5)
        {
            // Get paginated reviews for the property
            var reviews = await _reviewService.GetReviewsByPropertyIdAsync(propertyId, page, pageSize);

            // Get total review count for pagination
            var totalReviews = await _reviewService.GetReviewCountForPropertyAsync(propertyId);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalReviews / pageSize);

            // Create a view model with reviews and pagination info
            var model = new
            {
                Reviews = reviews,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalReviews = totalReviews
            };

            return PartialView("_ReviewsPartial", model);
        }

        // GET: Edit user profile
        [HttpGet]
        public IActionResult EditProfile()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUserByIdAsync(Id).Result;

            var viewModel = new UserProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.UserImageURL
            };

            return View(viewModel);
        }

        // Update user profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Get the current user ID
                model.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userService.GetUserByIdAsync(model.Id);

                // Handle profile image upload
                if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
                {
                    // Define allowed file extensions
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(model.ProfileImageFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ProfileImageFile", "Only .jpg, .jpeg and .png files are allowed");
                        return View(model);
                    }

                    // Generate a unique filename
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfileImageFile.CopyToAsync(fileStream);
                    }

                    // Update user with new image URL
                    user.UserImageURL = $"/uploads/profiles/{fileName}";
                }

                // Update user properties
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                // Update the user
                if (!string.IsNullOrEmpty(model.ProfilePicture) && model.ProfilePicture != user.UserImageURL)
                {
                    user.UserImageURL = model.ProfilePicture;
                }

                await _userService.UpdateUserAsync(user);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError("", "Error updating profile: " + ex.Message);
                return View(model);
            }
        }

    }
}
