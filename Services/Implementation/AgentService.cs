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


        public async Task<Agent> GetAgentByUserIdAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return agent;
        }

        public async Task<bool> IsUserAgentAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return agent != null;
        }

        // get agent by property id
        public async Task<AgentViewModel> GetAgentByPropertyIdAsync(int propertyId)
        {
            var agent = await _agentRepository.GetByPropertyIdAsync(propertyId);
            return agent;
        }

        // add agent
        public async Task<int> AddAgentAsync(Agent agent)
        {
            return await _agentRepository.AddAsync(agent);
        }

        // UPDATE AGENT
        public async Task UpdateAgentAsync(Agent agent)
        {

            await _agentRepository.UpdateAsync(agent);
        }

        // map agent to agent view model
        public AgentViewModel MapAgentToAgentViewModel(Agent agent)
        {
            return new AgentViewModel
            {
                FullName = agent.User.FirstName + " " + agent.User.LastName,
                Email = agent.User.Email,
                PhoneNumber = agent.User.PhoneNumber,
                PropertyCount = agent.Properties.Count,
                LicenseNumber = agent.LicenseNumber,
                Agency = agent.Agency,
                Biography = agent.Biography,
                YearsOfExperience = agent.YearsOfExperience,
                ProfileImageUrl = agent.User.UserImageURL
            };
        }

        // map agent view model to agent
        public Agent MapAgentViewModelToAgent(AgentViewModel agentViewModel)
        {
            return new Agent
            {
                LicenseNumber = agentViewModel.LicenseNumber,
                Agency = agentViewModel.Agency,
                Biography = agentViewModel.Biography,
                YearsOfExperience = agentViewModel.YearsOfExperience
            };
        }
    }
}