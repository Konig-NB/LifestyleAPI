using LifestyleAPI.Data;
using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LifestyleAPI.Repositories
{
    public class OrderRepository : Repository<Order> ,IOrderRepository
    {
        public OrderRepository(AppDbContext db) : base(db) {}

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(int page, int pagesize) =>
            await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

        public async Task<int> GetTotalCountAsync() =>
            await _db.Orders.CountAsync();

        public async Task<Order?> GetByIdOrderAsync(int id) =>
            await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<Order>> GetAllOrdersForCustomerAsync(int customerId, int page, int pageSize) =>
        await _db.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Customer)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task<int> GetTotalCountForCustomerAsync(int customerId) =>
            await _db.Orders.CountAsync(o => o.CustomerId == customerId);
    }
}