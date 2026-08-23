using LifestyleAPI.DTOs;
using LifestyleAPI.Helpers;

namespace LifestyleAPI.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<PagedResult<OrderItemDTO>> GetAllAsync(int page, int pageSize);
        Task<OrderItemDTO?> GetByIdAsync(int id);
        Task<OrderItemDTO> CreateAsync(CreateOrderItemDTO dto);
        Task<bool> ExistsAsync(int id);
    }
}