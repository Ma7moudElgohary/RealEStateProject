using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEStateProject.Models;
//using RealEStateProject.Data;
using RealEstate.Repositories;
using RealEstate.Data;
using RealEstate.Models;

namespace RealEStateProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public  Task<ApplicationUser> GetByIdAsync(string id)
        {
            return  _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Favorites)
                .Include(u => u.Requests)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Favorites)
                .Include(u => u.Requests)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role)
        {
            return await _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Favorites)
                .Include(u => u.Requests)
                .Where(u => u.Roles.Any(r => r.Name == role))
                .ToListAsync();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}