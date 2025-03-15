using RealEStateProject.ViewModels.Agent;

namespace RealEStateProject.ViewModels.Property
{
    public class PropertyDetailViewModel
    {
        public PropertyViewModel Property { get; set; }

        public AgentViewModel Agent { get; set; }

        public bool IsFavorite { get; set; }

        public List<PropertyViewModel>? SimilarProperties { get; set; }
    }
}