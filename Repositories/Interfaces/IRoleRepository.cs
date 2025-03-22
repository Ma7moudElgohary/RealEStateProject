using Microsoft.AspNetCore.Identity;
using RealEstate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IdentityRole> GetRoleByNameAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(IdentityRole role);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole role);

        // User-Role operations
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> AddUserToRolesAsync(ApplicationUser user, IEnumerable<string> roleNames);
        Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> RemoveUserFromRolesAsync(ApplicationUser user, IEnumerable<string> roleNames);

        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);


        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
    }
}