// AFTER
using LifestyleAPI.DTOs;
using LifestyleAPI.Models;

namespace LifestyleAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAsync(Order order, OrderStatus newStatus);
        Task<IEnumerable<NotificationDTO>> GetAllForCustomerAsync(int customerId);
    }
}