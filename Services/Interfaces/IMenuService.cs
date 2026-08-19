using LifestyleAPI.DTOs;
using LifestyleAPI.Helpers;

namespace LifestyleAPI.Services.Interfaces
{
    public interface IMenuService
    {
        Task<PagedResult<MenuDTO>> GetAllAsync(int page, int pageSize, string? categoryName);
        Task<MenuDTO?> GetByIdAsync(int id);
        Task<MenuDTO> CreateAsync(CreateMenuDTO dto);
        Task<MenuDTO?> UpdateAsync(int id, UpdateMenuDTO dto);
        Task<bool> ExistsAsync(int id);
    }
}