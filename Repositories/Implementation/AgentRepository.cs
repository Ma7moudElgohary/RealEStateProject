using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Repositories;
using RealEStateProject.Models;
using RealEStateProject.Repositories.Interfaces;

namespace RealEStateProject.Repositories
{
    public class AgentRepository : BaseRepository<Agent>, IAgentRepository
    {
        public AgentRepository(ApplicationDbContext context) : base(context)
        {
        }

       

        public async Task<Agent> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Properties)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

    
    }
}