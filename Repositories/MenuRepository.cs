using LifestyleAPI.Data;
using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LifestyleAPI.Repositories
{
    public class MenuRepository : Repository<Menu> ,IMenuRepository
    {
        public MenuRepository(AppDbContext db) : base(db) {}

        public async Task<IEnumerable<Menu>> GetAllMenusAsync(int page, int pagesize) =>
            await _db.Menus
                .Include(m => m.Category)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

        public async Task<int> GetTotalCountAsync() =>
            await _db.Menus.CountAsync();

        public async Task<Menu?> GetByIdMenuAsync(int id) =>
            await _db.Menus
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            
        public async Task<IEnumerable<Menu>> GetMenusByCategoryNameAsync(string categoryName) =>
            await _db.Menus
                .Include(m => m.Category)
                .Where(m => EF.Functions.ILike(m.Category.Name, categoryName))
                .ToListAsync();
    }
}