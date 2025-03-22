using RealEStateProject.Models;
using RealEstate.Repositories;

namespace RealEStateProject.Repositories.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetByAgentIdAsync(string agentId);
    }
}
