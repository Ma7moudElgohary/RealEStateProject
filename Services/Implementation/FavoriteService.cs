using AutoMapper;
using RealEstate.Models;
using RealEstate.Repositories;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.Services.Implementation
{

    public class FavoriteService : BaseService<Favorite, PropertyViewModel>, IFavoriteService
    {
        /// <summary>
        /// /mapper
        /// </summary>
        readonly IFavoriteRepository _repository;
        readonly IMapper _mapper;
        public FavoriteService(IBaseRepository<Favorite> repository, IMapper mapper) : base(repository, mapper)
        {
            
        }
        public Task AddFavoriteAsync(int propertyId, string userId)
        {
            _repository.AddAsync(new Favorite { PropertyId = propertyId, UserId = userId });
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PropertyViewModel>> GetFavoritesByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFavoriteAsync(int propertyId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFavoriteAsync(int propertyId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
