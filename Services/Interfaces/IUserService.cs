using RealEstate.Models;
using RealEStateProject.ViewModels.Admin;
using RealEStateProject.ViewModels.Agent;
using RealEStateProject.ViewModels.User;

namespace RealEstate.Services
{
    public interface IUserService : IBaseService<ApplicationUser, UserProfileViewModel>
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task UpdateUserAsync(ApplicationUser user);
        Task AssignRoleAsync(string userId, string role);

    }
}