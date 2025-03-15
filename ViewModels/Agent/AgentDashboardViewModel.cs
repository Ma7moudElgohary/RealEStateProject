using RealEstate.Models;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.ViewModels.Agent
{
    public class AgentDashboardViewModel
    {
        public List<PropertyViewModel>? Properties { get; set; }

        public List<PropertyRequestViewModel>? Requests { get; set; }

        public Dictionary<RequestStatus, int>? RequestStats { get; set; }
    }
}