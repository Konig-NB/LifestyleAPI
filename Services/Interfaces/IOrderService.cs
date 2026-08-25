using LifestyleAPI.Helpers;
using LifestyleAPI.DTOs;

namespace LifestyleAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResult<OrderDTO>> GetAllAsync(int page, int pageSize);
        Task<OrderDTO?> GetByIdAsync(int id);
        Task<OrderDTO> CreateAsync(int customerId, CreateOrderDTO dto);
        Task<OrderDTO?> UpdateAsync(int id, UpdateOrderDTO dto);
        Task<bool> ExistsAsync(int id);
        Task<PagedResult<OrderDTO>> GetAllForCustomerAsync(int customerId, int page, int pageSize);
    }
}