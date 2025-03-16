using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.ViewModels.Common
{
    public class HomeViewModel
    {
        public List<PropertyViewModel>? FeaturedProperties { get; set; }

        public List<PropertyViewModel>? RecentProperties { get; set; }

    }
}
