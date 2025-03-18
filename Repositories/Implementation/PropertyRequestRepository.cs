using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Models;
using RealEstate.Repositories;

namespace RealEStateProject.Repositories.Implementation
{
    public class PropertyRequestRepository : BaseRepository<PropertyRequest>,IPropertyRequestRepository
    {
        #region Inject


        private ApplicationDbContext _context;

        public PropertyRequestRepository(ApplicationDbContext _context):base(_context)
        {
            
        }
        #endregion

        
        public async Task<IEnumerable<PropertyRequest>> GetByAgentIdAsync(string agentId)
        {
            var PropertyRequests = await _context.PropertyRequests
                     .Where(p => p.UserId == agentId)
                        .ToListAsync();

            return PropertyRequests;

        }


        public async Task<PropertyRequest> GetByIdAsync(object id)
        {
            var propertyrequest = await _context.PropertyRequests.FindAsync(id);

            return propertyrequest;
        }

        public async Task<IEnumerable<PropertyRequest>> GetByPropertyIdAsync(int propertyId)
        {
            var properties = await _context.PropertyRequests.Where(s=>s.PropertyId == propertyId)
                                .ToListAsync();

            return properties;
        }

        public async Task<IEnumerable<PropertyRequest>> GetByUserIdAsync(string userId)
        {
            var properties = await _context.PropertyRequests.Where(s => s.UserId == userId).
                        ToListAsync();

            return properties;
        }

       
    }
}
