using LifestyleAPI.DTOs;
using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using LifestyleAPI.Services.Interfaces;

namespace LifestyleAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository repo, ILogger<NotificationService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task NotifyAsync(Order order, OrderStatus newStatus)
        {
            var message = BuildMessage(newStatus);

            var notification = new Notification
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Status = newStatus,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(notification);

            _logger.LogInformation(
                "Notification created for Order {OrderId}, Customer {CustomerId}: {Message}",
                order.Id, order.CustomerId, message);
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllForCustomerAsync(int customerId)
        {
            var notifications = await _repo.GetAllForCustomerAsync(customerId);

            return notifications.Select(n => new NotificationDTO
            {
                Id = n.Id,
                OrderId = n.OrderId,
                Status = n.Status,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            });
        }

        private static string BuildMessage(OrderStatus status) => status switch
        {
            OrderStatus.Received   => "We've received your order and it's in the queue.",
            OrderStatus.InProgress => "Your order is being prepared.",
            OrderStatus.Completed  => "Your order is ready for collection.",
            OrderStatus.Cancelled  => "Your order has been cancelled.",
            _ => "Your order status has been updated."
        };
    }
}