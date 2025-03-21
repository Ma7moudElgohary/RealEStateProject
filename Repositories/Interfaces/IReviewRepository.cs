using RealEstate.Models;
using RealEstate.Repositories;

namespace RealEStateProject.Repositories.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IEnumerable<Review>> GetAllByPropertyIdAsync(int propertyId);

        Task<double> GetAverageRatingAsync(int propertyId);

        Task<int> DeleteAsync(int id);

        Task<Review> GetByIdAsync(object id);
    }
}
