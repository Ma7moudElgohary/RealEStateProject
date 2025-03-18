using Microsoft.EntityFrameworkCore;
using RealEstate.Repositories;
using RealEstate.Data;
using RealEstate.Models;
using RealEstate.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace RealEStateProject.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<ApplicationUser> GetByIdAsync(string id)
        {
            return _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Favorites)
                .Include(u => u.Requests)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

       

        public async Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role)
        {
            // Get the role
            var identityRole = await _roleManager.FindByNameAsync(role);
            if (identityRole == null)
                return new List<ApplicationUser>();

            // Get the users in this role through the UserRoles table
            var usersInRole = await _context.UserRoles
                .Where(ur => ur.RoleId == identityRole.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            // Now get the full user objects
            return await _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Favorites)
                .Include(u => u.Requests)
                .Where(u => usersInRole.Contains(u.Id))
                .ToListAsync();
        }

       
    }
}