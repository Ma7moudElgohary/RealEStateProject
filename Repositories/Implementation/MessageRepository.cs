using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEStateProject.Models;
using RealEStateProject.Repositories.Interfaces;

namespace RealEStateProject.Repositories.Implementation
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetByAgentIdAsync(string agentId)
        {
            return await _context.Messages
                .Where(m => m.AgentId == agentId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

    }
}