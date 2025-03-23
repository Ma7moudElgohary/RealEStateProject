using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealEstate.Services
{
    public interface IBaseService<TEntity, TViewModel> where TEntity : class where TViewModel : class
    {
        Task<TViewModel> GetByIdAsync(object id);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task<TViewModel> CreateAsync(TViewModel viewModel);
        Task UpdateAsync(TViewModel viewModel);
        Task DeleteAsync(object id);
        Task<bool> ExistsAsync(object id);
        Task<int> CountAsync();
        List<SelectListItem> GetEnumSelectList<T>();
    }
}