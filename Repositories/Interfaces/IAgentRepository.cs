using RealEstate.Repositories;
using RealEStateProject.Models;

namespace RealEStateProject.Repositories.Interfaces
{
    public interface IAgentRepository : IBaseRepository<Agent>
    {
        //Task<Agent> GetByIdAsync(int id);
        Task<Agent> GetByUserIdAsync(string userId);
        //Task<IEnumerable<Agent>> GetAllAsync();
        //Task<int> AddAsync(Agent agent);
        //Task UpdateAsync(Agent agent);
        //Task DeleteAsync(int id);

        Task<Agent> GetByPropertyIdAsync(int propertyId);
    }
}
