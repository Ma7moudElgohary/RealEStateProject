using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Models;
using RealEstate.Repositories;

namespace RealEStateProject.Repositories.Implementation
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task DeleteAsync(int propertyId, string userId)
        {
            var favorite = await _context.Favorites
           .Include(f => f.Property)
           .Include(f => f.User)
           .FirstOrDefaultAsync(f => f.PropertyId == propertyId && f.UserId == userId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId)
        {
            return (IEnumerable<Favorite>)await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId);
        }

        public async Task<bool> IsFavoriteAsync(int propertyId, string userId)
        {
            //return await _context.Favorites
            //.AnyAsync(f => f.PropertyId == propertyId && f.UserId == userId);

            var favorite = await _context.Favorites
           .Include(f => f.Property)
           .Include(f => f.User)
           .FirstOrDefaultAsync(f => f.PropertyId == propertyId && f.UserId == userId);

            if (favorite != null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

    }
}