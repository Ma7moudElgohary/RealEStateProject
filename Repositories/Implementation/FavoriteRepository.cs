using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Models;
using RealEstate.Repositories;
using System.Linq.Expressions;

namespace RealEStateProject.Repositories.Implementation
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        readonly ApplicationDbContext _context;
        public FavoriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task DeleteAsync(int propertyId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Favorite> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFavoriteAsync(int propertyId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
