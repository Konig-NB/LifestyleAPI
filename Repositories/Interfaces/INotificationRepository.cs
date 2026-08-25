using LifestyleAPI.Models;

namespace LifestyleAPI.Repositories.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetAllForCustomerAsync(int customerId);
    }
}