using LifestyleAPI.Data;
using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LifestyleAPI.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext db) : base(db) {}

        public async Task<IEnumerable<Notification>> GetAllForCustomerAsync(int customerId) =>
            await _db.Notifications
                .Where(n => n.CustomerId == customerId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
    }
}