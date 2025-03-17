using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate.Data;
using RealEstate.Models;
using RealEstate.Repositories;

namespace RealEStateProject.Repositories.Implementation
{
    public class PropertyRequestRepository : IPropertyRequestRepository
    {
        #region Inject


        private ApplicationDbContext _context;

        public PropertyRequestRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        #endregion

        public async Task<int> AddAsync(PropertyRequest request)
        {
            _context.PropertyRequests.AddAsync(request);

            return await _context.SaveChangesAsync();

        }

        public async Task<int> AddRangeAsync(IEnumerable<PropertyRequest> entities)
        {
            _context.PropertyRequests.AddRangeAsync(entities);

            int affectedrow = await _context.SaveChangesAsync();

            return affectedrow;
        }

        public async Task<int> CountAsync(Expression<Func<PropertyRequest, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.PropertyRequests.CountAsync();

            }
            return await _context.PropertyRequests.CountAsync(predicate);
        }

        public async Task DeleteAsync(object id)
        {
            var property = await _context.PropertyRequests.FindAsync(id);

            if (property != null)
            {
                _context.PropertyRequests.Remove(property);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(PropertyRequest entity)
        {
            var property = await _context.PropertyRequests
                 .FirstOrDefaultAsync(p => p.Id == entity.Id);

            if (property != null)
            {
                _context.PropertyRequests.Remove(property);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<PropertyRequest, bool>> predicate)
        {

            return await _context.PropertyRequests.AnyAsync(predicate);
        }

        public async Task<IEnumerable<PropertyRequest>> FindAsync(Expression<Func<PropertyRequest, bool>> predicate)
        {
            var properties = await _context.PropertyRequests.Where(predicate).ToListAsync();

            return properties;
        }

        public async Task<IEnumerable<PropertyRequest>> GetAllAsync()
        {
            var PropertyRequests = await _context.PropertyRequests.ToListAsync();

            return PropertyRequests;

        }

        public async Task<IEnumerable<PropertyRequest>> GetByAgentIdAsync(string agentId)
        {
            var PropertyRequests = await _context.PropertyRequests
                     .Where(p => p.UserId == agentId)
                        .ToListAsync();

            return PropertyRequests;

        }

        public async Task<PropertyRequest> GetByIdAsync(int id)
        {
            var propertyrequest = await _context.PropertyRequests.FindAsync(id);

            return propertyrequest;
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

        public async Task<int> SaveChangesAsync()
        {
            int roweffect = await _context.SaveChangesAsync();

            return roweffect;
        }

        public async Task UpdateAsync(PropertyRequest request)
        {  
            var existingProperty = await _context.PropertyRequests.FindAsync(request.Id);

            if (existingProperty != null)
            {
                _context.Entry(existingProperty).CurrentValues.SetValues(request);

                await _context.SaveChangesAsync();
            }
        }
    }
}
