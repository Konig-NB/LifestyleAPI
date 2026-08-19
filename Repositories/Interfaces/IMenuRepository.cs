using LifestyleAPI.Models;

namespace LifestyleAPI.Repositories.Interfaces
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetAllMenusAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Menu?> GetByIdMenuAsync(int id);
        Task<IEnumerable<Menu>> GetMenusByCategoryNameAsync(string categoryName);
    }
}