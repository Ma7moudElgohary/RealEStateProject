using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.ViewModels.User
{
    public class UserDashboardViewModel
    {
        public List<PropertyViewModel>? Favorites { get; set; }

        public List<PropertyRequestViewModel>? Requests { get; set; }
    }
}