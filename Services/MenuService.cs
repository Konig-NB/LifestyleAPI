using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using LifestyleAPI.DTOs;
using LifestyleAPI.Services.Interfaces;
using LifestyleAPI.Helpers;

namespace LifestyleAPI.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repo;
        private readonly ICategoryRepository _categoryRepo;
        public MenuService(IMenuRepository repo, ICategoryRepository categoryRepo)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
        }

        public async Task<PagedResult<MenuDTO>> GetAllAsync(int page, int pageSize, string? categoryName)
        {
            IEnumerable<Menu> menus;
            int totalCount;

            if (categoryName is not null)
            {
                menus = await _repo.GetMenusByCategoryNameAsync(categoryName);
                totalCount = menus.Count();
            }
            else
            {
                menus = await _repo.GetAllMenusAsync(page, pageSize);
                totalCount = await _repo.GetTotalCountAsync();
            }

            return new PagedResult<MenuDTO>
            {
                Data = menus.Select(ToDto),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<MenuDTO?> GetByIdAsync(int id)
        {
            var menu = await _repo.GetByIdMenuAsync(id);
            return menu is null ? null : ToDto(menu);
        }

        public async Task<MenuDTO> CreateAsync(CreateMenuDTO dto)
        {
            var menu = new Menu
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            await _repo.CreateAsync(menu);

            var created = await _repo.GetByIdMenuAsync(menu.Id);
            return ToDto(created!);
        }

        public async Task<MenuDTO?> UpdateAsync(int id, UpdateMenuDTO dto)
        {
            var menu = await _repo.GetByIdMenuAsync(id);
            if (menu is null) return null;

            if (dto.Name is not null) menu.Name = dto.Name;
            if (dto.Description is not null) menu.Description = dto.Description;
            if (dto.Price.HasValue) menu.Price = dto.Price.Value;
            if (dto.IsAvailable.HasValue) menu.IsAvailable = dto.IsAvailable.Value;

            await _repo.UpdateAsync(menu);
            var updated = await _repo.GetByIdMenuAsync(id);
            return ToDto(updated!);
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _repo.ExistsAsync(id);

        private static MenuDTO ToDto(Menu m) => new MenuDTO
        {
            Id = m.Id,
            CategoryId = m.CategoryId,
            CategoryName = m.Category?.Name ?? string.Empty,
            Name = m.Name,
            Description = m.Description,
            Price = m.Price,
            IsAvailable = m.IsAvailable
        };
    }
}