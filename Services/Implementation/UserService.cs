using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RealEstate.Models;
using RealEstate.Repositories;
using RealEStateProject.ViewModels.Admin;
using RealEStateProject.ViewModels.Agent;
using RealEStateProject.ViewModels.Property;
using RealEStateProject.ViewModels.User;

namespace RealEstate.Services
{
    public class UserService : BaseService<ApplicationUser, UserProfileViewModel>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository,
                            IMapper mapper,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager) : base(userRepository, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<UserDashboardViewModel> GetUserDashboardAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var dashboard = new UserDashboardViewModel
            {
                Favorites = user.Favorites?.Select(f => new PropertyFavoriteViewModel
                {
                    Id = f.Property.Id,
                    Title = f.Property.Title,
                    Description = f.Property.Description,
                    Price = f.Property.Price,
                    Address = f.Property.Address,
                    City = f.Property.City,
                    State = f.Property.State,
                    ZipCode = f.Property.ZipCode,
                    Type = f.Property.Type,
                    Status = f.Property.Status,
                    //CreatedAt = f.Property.CreatedAt,
                    AgentName = $"{f.Property.Agent.FirstName} {f.Property.Agent.LastName}",
                    //IsFavorite = true,
                    //ImageUrls = f.Property.Images.Select(i => i.ImageUrl).ToList(),
                    //FeaturedImageUrl = f.Property.FeaturedImage
                }).ToList(),

                Requests = user.Requests?.Select(r => new PropertyRequestViewModel
                {
                    Id = r.Id,
                    Property = new PropertyViewModel
                    {
                        Id = r.Property.Id,
                        Title = r.Property.Title,
                        Description = r.Property.Description,
                        Price = r.Property.Price,
                        Address = r.Property.Address,
                        City = r.Property.City,
                        State = r.Property.State,
                        ZipCode = r.Property.ZipCode,
                        Type = r.Property.Type,
                        Status = r.Property.Status,
                        CreatedAt = r.Property.CreatedAt,
                        AgentName = $"{r.Property.Agent.FirstName} {r.Property.Agent.LastName}",
                        IsFavorite = false,
                        ImageUrls = r.Property.Images.Select(i => i.ImageUrl).ToList(),
                        FeaturedImageUrl = r.Property.FeaturedImage,
                    },
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserEmail = user.Email,
                    Message = r.Message,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                }).ToList() ?? new List<PropertyRequestViewModel>()
            };

            return dashboard;
        }

        public async Task<AgentDashboardViewModel> GetAgentDashboardAsync(string agentId)
        {
            var agent = await _userRepository.GetByIdAsync(agentId);

            if (agent == null)
            {
                throw new KeyNotFoundException("Agent not found.");
            }

            var propertyRequests = agent.Properties?
                .SelectMany(p => p.Requests)
                .ToList() ?? new List<PropertyRequest>();

            // Get all direct requests to the agent
            var agentRequests = agent.Properties?
                .SelectMany(p => p.Agent.Requests)
                .ToList() ?? new List<PropertyRequest>();

            // Combine both request collections for complete view
            var allRequests = propertyRequests.Union(agentRequests);

            var dashboard = new AgentDashboardViewModel
            {
                Properties = agent.Properties?.Select(p => new PropertyViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Address = p.Address,
                    City = p.City,
                    State = p.State,
                    ZipCode = p.ZipCode,
                    Type = p.Type,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    AgentName = $"{agent.FirstName} {agent.LastName}",
                    IsFavorite = false,
                    ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
                    FeaturedImageUrl = p.FeaturedImage,
                }).ToList() ?? new List<PropertyViewModel>(),

                Requests = allRequests.Select(r => new PropertyRequestViewModel
                {
                    Id = r.Id,
                    Property = new PropertyViewModel
                    {
                        Id = r.Property.Id,
                        Title = r.Property.Title,
                        Description = r.Property.Description,
                        Price = r.Property.Price,
                        Address = r.Property.Address,
                        City = r.Property.City,
                        State = r.Property.State,
                        ZipCode = r.Property.ZipCode,
                        Type = r.Property.Type,
                        Status = r.Property.Status,
                        CreatedAt = r.Property.CreatedAt,
                        AgentName = $"{agent.FirstName} {agent.LastName}",
                        IsFavorite = false,
                        ImageUrls = r.Property.Images.Select(i => i.ImageUrl).ToList(),
                        FeaturedImageUrl = r.Property.FeaturedImage
                    },
                    UserName = $"{r.User.FirstName} {r.User.LastName}",
                    UserEmail = r.User.Email,
                    Message = r.Message,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                }).ToList(),

                RequestStats = allRequests
                    .GroupBy(r => r.Status)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return dashboard;
        }

        public async Task<AdminDashboardViewModel> GetAdminDashboardAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var agentCount = 0;

            // Count users with the "Agent" role
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Agent"))
                {
                    agentCount++;
                }
            }


            var dashboard = new AdminDashboardViewModel
            {
                TotalUsers = users.Count(),
                TotalAgents = agentCount,
                TotalProperties = users.SelectMany(u => u.Properties).Count(),
                TotalRequests = users.SelectMany(u => u.Requests).Count(),
                PropertyTypeStats = users.SelectMany(u => u.Properties)
                        .GroupBy(p => p.Type)
                        .ToDictionary(g => g.Key, g => g.Count()),
                PropertyStatusStats = users.SelectMany(u => u.Properties)
                    .GroupBy(p => p.Status)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return dashboard;
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {

            await _userRepository.UpdateAsync(user);
        }

        public async Task AssignRoleAsync(string userId, string role)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Check if the role exists, create it if not
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            // Add the user to the role
            await _userManager.AddToRoleAsync(user, role);

            // If you still need to update the user in your repository
            await _userRepository.UpdateAsync(user);
        }
    }
}