using RealEstate.Models;

namespace RealEStateProject.ViewModels.Property
{
    public class PropertySearchFilterViewModel
    {
        public PropertyType? Type { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string Location { get; set; }

        public int? MinBedrooms { get; set; }

        public int? MaxBedrooms { get; set; }

        public int? MinBathrooms { get; set; }
    }
}