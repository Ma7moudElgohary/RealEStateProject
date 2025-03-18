using RealEstate.Services;
using RealEStateProject.Models;
using RealEStateProject.ViewModels.Agent;

namespace RealEStateProject.Services.Interfaces
{
    public interface IAgentService : IBaseService<Agent, AgentViewModel>
    {
        Task<AgentViewModel> GetAgentByUserIdAsync(string userId);
        
        Task<bool> IsUserAgentAsync(string userId);
    }
}
