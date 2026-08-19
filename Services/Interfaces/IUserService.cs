using LifestyleAPI.DTOs;
using LifestyleAPI.Helpers;

namespace LifestyleAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserDTO>> GetAllAsync(int page, int pageSize);
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO?> UpdateAsync(int id , UpdateUserDTO dto);
        Task<bool> ExistsAsync(int id);
    }
}