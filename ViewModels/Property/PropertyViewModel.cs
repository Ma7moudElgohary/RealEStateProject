using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RealEstate.Models;

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
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Area must be a positive value")]
        public int SquareFeet { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Bedrooms must be a positive value")]
        public int Bedrooms { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Bathrooms must be a positive value")]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        public string ZipCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required(ErrorMessage = "Property type is required")]
        public PropertyType Type { get; set; }

        [Required(ErrorMessage = "Property status is required")]
        public PropertyStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AgentName { get; set; }

        public bool IsFavorite { get; set; }

        public List<string>? ImageUrls { get; set; }

        public string FeaturedImageUrl { get; set; }
    }
}