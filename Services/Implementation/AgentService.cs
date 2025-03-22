using AutoMapper;
using RealEstate.Services;
using RealEStateProject.Models;
using RealEStateProject.Repositories.Interfaces;
using RealEStateProject.Services.Interfaces;
using RealEStateProject.ViewModels.Agent;

namespace RealEStateProject.Services
{
    public class AgentService : BaseService<Agent, AgentViewModel>, IAgentService
    {
        private readonly IAgentRepository _agentRepository;

        public AgentService(IAgentRepository agentRepository, IMapper mapper)
            : base(agentRepository, mapper)
        {
            _agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
        }


        public async Task<AgentViewModel> GetAgentByUserIdAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return _mapper.Map<AgentViewModel>(agent);
        }

        public async Task<bool> IsUserAgentAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return agent != null;
        }

        // get agent by property id
        public async Task<Agent> GetAgentByPropertyIdAsync(int propertyId)
        {
            var agent = await _agentRepository.GetByPropertyIdAsync(propertyId);
            return agent;
        }
    }
}