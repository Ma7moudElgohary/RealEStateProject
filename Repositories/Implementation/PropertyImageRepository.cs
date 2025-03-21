using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Models;

namespace RealEstate.Repositories.Implementation
{
    public class PropertyImageRepository : BaseRepository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PropertyImage> GetByUrlAsync(int propertyId, string imageUrl)
        {
            return await _context.PropertyImages
                .FirstOrDefaultAsync(pi => pi.PropertyId == propertyId && pi.ImageUrl == imageUrl);
        }

        public async Task DeletePropertyImagesAsync(int propertyId)
        {
            var images = await _context.PropertyImages
                .Where(pi => pi.PropertyId == propertyId)
                .ToListAsync();

            _context.PropertyImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddAsync(PropertyImage entity)
        {
            _context.PropertyImages.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}