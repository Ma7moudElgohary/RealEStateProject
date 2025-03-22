using System;
using System.Collections.Generic;
using RealEstate.Models;

namespace RealEStateProject.ViewModels.Property
{
    public class PropertyFavoriteViewModel
    {
        // Primary identifier
        public int Id { get; set; }

        // Favorite-specific properties
        public int FavoriteId { get; set; }
        public DateTime DateSaved { get; set; }
        public string UserId { get; set; }

        // Core property information
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int SquareFeet { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

        // Location information
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }



        // Property status and type information
        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; }

        // Image data
        public string Cover { get; set; }
        public List<string> AdditionalImageUrls { get; set; }

        // Agent information
        public string AgentName { get; set; }
        public string AgentId { get; set; }
        public string AgentPhone { get; set; }
        public string AgentEmail { get; set; }

    }
}