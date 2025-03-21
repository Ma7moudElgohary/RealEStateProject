using RealEStateProject.Models;
using RealEStateProject.ViewModels.Property;
using RealEStateProject.ViewModels.User;
using RealEStateProject.ViewModels;
using RealEStateProject.Models;
using RealEStateProject.ViewModels.User;

namespace RealEStateProject.ViewModels.User
{

    public class UserDashboardViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<PropertyFavoriteViewModel> Favorites { get; set; } = new List<PropertyFavoriteViewModel>();
        public List<PropertyRequestViewModel> Requests { get; set; } = new List<PropertyRequestViewModel>();
    }

    public class UserDetailsViewModel
    {
        public RealEstate.Models.ApplicationUser User { get; set; }
        public List<PropertyViewModel> Favorites { get; set; } = new List<PropertyViewModel>();
    }
}
