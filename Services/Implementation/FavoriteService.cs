using AutoMapper;
using RealEstate.Models;
using RealEstate.Repositories;
using RealEstate.Services;
using RealEStateProject.ViewModels.Agent;
using RealEStateProject.ViewModels.Property;
using RealEStateProject.Repositories.Implementation;
using RealEStateProject.Models;
using RealEStateProject.ViewModels.Agent;


namespace RealEStateProject.Services.Implementation
{

    public class FavoriteService : BaseService<Favorite, PropertyViewModel>, IFavoriteService
    {
       
        readonly IFavoriteRepository _repository;
        readonly IMapper _mapper;
        public FavoriteService(IBaseRepository<Favorite> repository, IMapper mapper) : base(repository, mapper)
        {

        }
        public Task AddFavoriteAsync(int propertyId, string userId)
        {
            _repository.AddAsync(new Favorite { PropertyId = propertyId, UserId = userId });
            return _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PropertyViewModel>> GetFavoritesByUserIdAsync(string userId)
        {
            var favorite = await _repository.GetByUserIdAsync(userId);
            return (IEnumerable<PropertyViewModel>)_mapper.Map<AgentViewModel>(favorite);
        }

        public async Task<bool> IsFavoriteAsync(int propertyId, string userId)
        {
            var favorite = await _repository.GetByUserIdAsync(userId);
            return favorite != null;

        }

        public async Task RemoveFavoriteAsync(int propertyId, string userId)
        {
            await _repository.DeleteAsync(propertyId, userId);
        }

    }
}
