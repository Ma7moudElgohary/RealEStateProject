using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Models;
using RealEstate.Repositories;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.Repositories.Implementation
{
    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> AddAsync(Property entity)
        {
            _context.Properties.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId)
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .Where(p => p.AgentId == agentId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> SearchAsync(PropertySearchFilterViewModel filter)
        {
            var query = _context.Properties
              .Include(p => p.Agent)
              .Include(p => p.Images)
              .AsQueryable();

            if (filter.Type.HasValue)
            {
                query = query.Where(p => p.Type == filter.Type.Value);
            }
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(filter.Location))
            {
                query = query.Where(p =>
                    p.City.Contains(filter.Location) ||
                    p.State.Contains(filter.Location) ||
                    p.ZipCode.Contains(filter.Location) ||
                    p.Address.Contains(filter.Location));
            }

            if (filter.MinBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms >= filter.MinBedrooms.Value);
            }
            if (filter.MaxBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms <= filter.MaxBedrooms.Value);
            }
            if (filter.MinBathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms >= filter.MinBathrooms.Value);
            }

            return await query
             .OrderByDescending(p => p.CreatedAt)
             .ToListAsync();
        }

        public new async Task<Property> GetByIdAsync(object id)
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == (int)id);
        }

        public new async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
