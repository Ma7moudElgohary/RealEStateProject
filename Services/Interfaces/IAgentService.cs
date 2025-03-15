using RealEstate.Services;
using RealEStateProject.Models;
using RealEStateProject.ViewModels.Agent;

namespace RealEStateProject.Services.Interfaces
{
    public interface IAgentService : IBaseService<Agent, AgentViewModel>
    {
        Task<AgentViewModel> GetAgentByIdAsync(int id);
        Task<AgentViewModel> GetAgentByUserIdAsync(string userId);
        Task<IEnumerable<AgentViewModel>> GetAllAgentsAsync();
        Task<int> CreateAgentAsync(AgentViewModel agentViewModel, string userId);
        Task UpdateAgentAsync(AgentViewModel agentViewModel);
        Task DeleteAgentAsync(int id);
        Task<bool> IsUserAgentAsync(string userId);
    }
}
