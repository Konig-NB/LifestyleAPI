using LifestyleAPI.Models;

namespace LifestyleAPI.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Order?> GetByIdOrderAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersForCustomerAsync(int customerId, int page, int pageSize);
        Task<int> GetTotalCountForCustomerAsync(int customerId);
    }
}