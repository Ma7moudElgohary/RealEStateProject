using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEStateProject.Models;
using RealEStateProject.ViewModels.Common;
using RealEStateProject.ViewModels.Property;
using System.Diagnostics;

namespace RealEStateProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                FeaturedProperties = new List<PropertyViewModel>
                {
                    new PropertyViewModel
                    {
                        Id = 1,
                        Title = "Luxury Villa",
                        Price = 1000000,
                        Bedrooms = 5,
                        Bathrooms = 3,
                        SquareFeet = 500,
                        Type = PropertyType.SALE,
                        Address = "123 Luxury Lane",
                        City = "Beverly Hills",
                        State = "CA",
                        FeaturedImageUrl = "/images/img_1.jpg"
                    },
                    new PropertyViewModel
                    {
                        Id = 2,
                        Title = "Modern Apartment",
                        Price = 750000,
                        Bedrooms = 3,
                        Bathrooms = 2,
                        SquareFeet = 350,
                        Type = PropertyType.SALE,
                        Address = "456 Urban Ave",
                        City = "Manhattan",
                        State = "NY",
                        FeaturedImageUrl = "/images/img_2.jpg"
                    },
                    new PropertyViewModel
                    {
                        Id = 3,
                        Title = "Beachfront Property",
                        Price = 1200000,
                        Bedrooms = 4,
                        Bathrooms = 3,
                        SquareFeet = 450,
                        Type = PropertyType.SALE,
                        Address = "789 Ocean Drive",
                        City = "Miami",
                        State = "FL",
                        FeaturedImageUrl = "/images/img_3.jpg"
                    }
                },
                RecentProperties = new List<PropertyViewModel>
                {
                    new PropertyViewModel
                    {
                        Id = 4,
                        Title = "Cozy Cottage",
                        Price = 450000,
                        Bedrooms = 2,
                        Bathrooms = 1,
                        SquareFeet = 200,
                        Type = PropertyType.SALE,
                        Address = "101 Country Road",
                        City = "Portland",
                        State = "OR",
                        FeaturedImageUrl = "/images/img_4.jpg"
                    },
                }
            };

            return View("Index", model);
        }

        public IActionResult Privacy()
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