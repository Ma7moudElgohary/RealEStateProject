using RealEStateProject.ViewModels.Common;

namespace RealEStateProject.ViewModels.Property
{
    public class PropertySearchViewModel
    {
        public List<PropertyViewModel>? Properties { get; set; }

        public PropertySearchFilterViewModel Filters { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}