using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Models;
using RealEStateProject.ViewModels.Agent;

namespace RealEStateProject.ViewModels.Property
{
    public class PropertyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Square feet is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Area must be greater than 0")]
        public int SquareFeet { get; set; }

        [Required(ErrorMessage = "Number of bedrooms is required")]
        [Range(0, 100, ErrorMessage = "Bedrooms must be between 0 and 100")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "Number of bathrooms is required")]
        [Range(0, 100, ErrorMessage = "Bathrooms must be between 0 and 100")]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Property type is required")]
        public PropertyType Type { get; set; }

        [Required(ErrorMessage = "Property status is required")]
        public PropertyStatus Status { get; set; }

        public int? YearBuilt { get; set; }
        //public List<string>? Features { get; set; }

        // Agent information
        public string? AgentName { get; set; }
        public AgentViewModel? Agent { get; set; }
        public string? AgentId { get; set; }
        public string? AgentPhone { get; set; }
        public string? AgentEmail { get; set; }

        // Images
        public IFormFile? ImageUpload { get; set; }
        public IEnumerable<IFormFile>? AdditionalImages { get; set; }
        public string? FeaturedImageUrl { get; set; }
        public IEnumerable<string>? ImageUrls { get; set; }

        // Additional property information
        public DateTime? CreatedAt { get; set; }
        public bool? IsFavorite { get; set; }

        // For dropdowns in UI
        [IgnoreDataMember]
        public List<SelectListItem>? PropertyTypes { get; set; }

        [IgnoreDataMember]
        public List<SelectListItem>? PropertyStatuses { get; set; }



        public IEnumerable<ReviewViewModel>? Reviews { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public Dictionary<int, int>? RatingDistribution { get; set; }
    }

    public class ReviewViewModel
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }

}