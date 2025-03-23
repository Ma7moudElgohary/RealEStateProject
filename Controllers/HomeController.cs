using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Models;
using RealEstate.Services;
using RealEStateProject.Models;
using RealEStateProject.ViewModels.Common;
using RealEStateProject.ViewModels.Property;
using System.Diagnostics;

namespace RealEStateProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyService _propertyService;
        public HomeController(ILogger<HomeController> logger, IPropertyService propertyService)
        {
            _logger = logger;
            _propertyService = propertyService;
        }


        // Home page
        public async Task<IActionResult> Index()
        {
            ViewBag.PropertyTypes = _propertyService.GetEnumSelectList<PropertyType>();
            ViewBag.City = _propertyService.GetEnumSelectList<City>();

            // Get properties from the database
            var allProperties = await _propertyService.GetAllPropertiesAsync(null);
            var propertiesList = allProperties.ToList();

            var model = new HomeViewModel
            {
                // Take 3 featured properties
                FeaturedProperties = propertiesList.Take(3).ToList(),

                // Take most recent properties (sorted by creation date)
                RecentProperties = propertiesList.OrderByDescending(p => p.CreatedAt).Take(6).ToList()
            };

            return View("Index", model);
        }

        // Privacy page
        public IActionResult Privacy()
        {
            return View();
        }

        // About page
        public IActionResult About()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}