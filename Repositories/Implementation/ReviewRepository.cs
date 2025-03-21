using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Models;
using RealEstate.Services;
using RealEStateProject.Repositories.Interfaces;

namespace RealEStateProject.Repositories.Implementation
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllByPropertyIdAsync(int propertyId)
        {
            return await _context.Reviews
                .Where(r => r.PropertyId == propertyId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int propertyId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.PropertyId == propertyId)
                .ToListAsync();

            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return 0;
            }

            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync();
        }
    }
}
