using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstate.Repositories;


namespace RealEstate.Services
{
    public abstract class BaseService<TEntity, TViewModel> : IBaseService<TEntity, TViewModel>
        where TEntity : class
        where TViewModel : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TViewModel> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<TViewModel>(entity);
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return MapToViewModels(entities);
        }

        public async Task<TViewModel> CreateAsync(TViewModel viewModel)
        {
            var entity = MapToEntity(viewModel);
            await _repository.AddAsync(entity);
            return _mapper.Map<TViewModel>(entity);
        }

        public async Task UpdateAsync(TViewModel viewModel)
        {
            var entity = MapToEntity(viewModel);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(object id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(object id)
        {
            return await _repository.ExistsAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        protected IEnumerable<TViewModel> MapToViewModels(IEnumerable<TEntity> entities)
        {
            return _mapper.Map<IEnumerable<TViewModel>>(entities);
        }

        protected TEntity MapToEntity(TViewModel viewModel)
        {
            return _mapper.Map<TEntity>(viewModel);
        }
    }
}