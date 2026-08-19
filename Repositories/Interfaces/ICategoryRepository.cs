using LifestyleAPI.Models;

namespace LifestyleAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Category?> GetByIdCategoryAsync(int id);
    }
}