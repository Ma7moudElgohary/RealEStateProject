using RealEstate.Models;

namespace RealEStateProject.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalAgents { get; set; }

        public int TotalProperties { get; set; }

        public int TotalRequests { get; set; }

        public Dictionary<PropertyType, int>? PropertyTypeStats { get; set; }

        public Dictionary<PropertyStatus, int>? PropertyStatusStats { get; set; }
    }
}