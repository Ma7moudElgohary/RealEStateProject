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

        public async Task<AgentViewModel> GetAgentByIdAsync(int id)
        {
            var agent = await _agentRepository.GetByIdAsync(id);
            return _mapper.Map<AgentViewModel>(agent);
        }

        public async Task<AgentViewModel> GetAgentByUserIdAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return _mapper.Map<AgentViewModel>(agent);
        }

        public async Task<IEnumerable<AgentViewModel>> GetAllAgentsAsync()
        {
            var agents = await _agentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AgentViewModel>>(agents);
        }

        public async Task<int> CreateAgentAsync(AgentViewModel agentViewModel, string userId)
        {
            // Check if the user is already an agent
            var existingAgent = await _agentRepository.GetByUserIdAsync(userId);
            if (existingAgent != null)
            {
                throw new InvalidOperationException("User is already registered as an agent");
            }

            // Map view model to entity
            var agent = _mapper.Map<Agent>(agentViewModel);
            agent.UserId = userId;

            // Validate license number format if needed
            if (string.IsNullOrWhiteSpace(agent.LicenseNumber))
            {
                throw new ArgumentException("License number is required for agents");
            }

            // Save agent
            return await _agentRepository.AddAsync(agent);
        }

        public async Task UpdateAgentAsync(AgentViewModel agentViewModel)
        {
            var existingAgent = await _agentRepository.GetByIdAsync(agentViewModel.Id);
            if (existingAgent == null)
            {
                throw new KeyNotFoundException($"Agent with ID {agentViewModel.Id} not found");
            }

            // Update properties from view model while preserving relationships
            existingAgent.LicenseNumber = agentViewModel.LicenseNumber;
            existingAgent.Agency = agentViewModel.Agency;
            existingAgent.Biography = agentViewModel.Biography;
            existingAgent.YearsOfExperience = agentViewModel.YearsOfExperience;
            existingAgent.ProfileImageUrl = agentViewModel.ProfileImageUrl;

            // Save changes
            await _agentRepository.UpdateAsync(existingAgent);
        }

        public async Task DeleteAgentAsync(int id)
        {
            var agent = await _agentRepository.GetByIdAsync(id);
            if (agent == null)
            {
                throw new KeyNotFoundException($"Agent with ID {id} not found");
            }

            // Check if agent has properties
            if (agent.Properties != null && agent.Properties.Count > 0)
            {
                throw new InvalidOperationException("Cannot delete agent with associated properties");
            }

            await _agentRepository.DeleteAsync(id);
        }

        public async Task<bool> IsUserAgentAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return agent != null;
        }
    }
}