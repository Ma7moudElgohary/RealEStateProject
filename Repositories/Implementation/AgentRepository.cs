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

        public async Task<Agent> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Properties)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Agent> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Properties)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<IEnumerable<Agent>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Properties)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Agent agent)
        {
            await _dbSet.AddAsync(agent);
            return await SaveChangesAsync();
        }

        public async Task UpdateAsync(Agent agent)
        {
            _dbSet.Attach(agent);
            _context.Entry(agent).State = EntityState.Modified;

            // Make sure navigation properties aren't modified directly
            _context.Entry(agent).Reference(a => a.User).IsModified = false;

            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var agent = await _dbSet.FindAsync(id);
            if (agent != null)
            {
                _dbSet.Remove(agent);
                await SaveChangesAsync();
            }
        }
    }
}