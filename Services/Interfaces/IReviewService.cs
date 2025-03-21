using RealEstate.Models;
using RealEstate.Services;
using RealEStateProject.ViewModels.Common;

namespace RealEStateProject.Services.Interfaces
{
    public interface IReviewService : IBaseService<Review, Review>
    {
        Task<Review> CreateAsync(Review review);

        //Task<IEnumerable<Review>> GetAllAsync();

        Task<int> DeleteAsync(int id);

        Task<IEnumerable<Review>> GetByPropertyIdAsync(int propertyId);

        Task<double> GetAverageRatingAsync(int propertyId);
    }
}
