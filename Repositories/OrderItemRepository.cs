using LifestyleAPI.Data;
using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LifestyleAPI.Repositories
{
    public class OrderItemRepository : Repository<OrderItem> ,IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext db) : base(db) {}

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync(int page, int pagesize) =>
            await _db.OrderItems
                .Include(m => m.Order)
                    .ThenInclude(m => m.Customer)
                .Include(m => m.MenuItem)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

        public async Task<int> GetTotalCountAsync() =>
            await _db.OrderItems.CountAsync();

        public async Task<OrderItem?> GetByIdOrderItemAsync(int id) =>
            await _db.OrderItems
                .Include(m => m.Order)
                    .ThenInclude(m => m.Customer)
                .Include(m => m.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);
    }
}