using LifestyleAPI.Models;

namespace LifestyleAPI.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<OrderItem?> GetByIdOrderItemAsync(int id);
    }
}