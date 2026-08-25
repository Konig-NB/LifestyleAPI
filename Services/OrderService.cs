using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using LifestyleAPI.DTOs;
using LifestyleAPI.Services.Interfaces;
using LifestyleAPI.Helpers;

namespace LifestyleAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IMenuRepository _menuRepo;
        private readonly INotificationService _notificationService;
        public OrderService(IOrderRepository repo, IMenuRepository menuRepo, INotificationService notificationService) 
        { 
            _repo = repo;
            _menuRepo = menuRepo;
            _notificationService = notificationService;
        }

        private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedTransitions = new()
        {
            [OrderStatus.Received]   = new[] { OrderStatus.InProgress, OrderStatus.Cancelled },
            [OrderStatus.InProgress] = new[] { OrderStatus.Completed, OrderStatus.Cancelled },
            [OrderStatus.Completed]  = Array.Empty<OrderStatus>(),
            [OrderStatus.Cancelled]  = Array.Empty<OrderStatus>()
        };

        public async Task<PagedResult<OrderDTO>> GetAllAsync(int page, int pageSize)
        {
            var orders = await _repo.GetAllOrdersAsync(page, pageSize);
            var totalCount = await _repo.GetTotalCountAsync();

            return new PagedResult<OrderDTO>
            {
                Data = orders.Select(ToDto),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            var order = await _repo.GetByIdOrderAsync(id);
            return order is null ? null : ToDto(order);
        }

        public async Task<OrderDTO> CreateAsync(int customerId, CreateOrderDTO dto)
        {
            if (dto.OrderItems is null || dto.OrderItems.Count == 0)
            throw new InvalidOperationException("An order must contain at least one item.");

            var orderItems = new List<OrderItem>();
            decimal totalPrice = 0;

            foreach (var itemDto in dto.OrderItems)
            {
                var menuItem = await _menuRepo.GetByIdMenuAsync(itemDto.MenuItemId);
                if (menuItem == null)
                    throw new KeyNotFoundException($"Menu item {itemDto.MenuItemId} not found");
                
                if (!menuItem.IsAvailable)
                    throw new InvalidOperationException($"Menu item {itemDto.MenuItemId} is not available");

                var orderItem = new OrderItem
                {
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity,
                    TotalPrice = menuItem.Price * itemDto.Quantity,
                    SpecialInstructions = itemDto.SpecialInstructions
                };

                orderItems.Add(orderItem);
                totalPrice += orderItem.TotalPrice;
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderItems = orderItems,
                Status = OrderStatus.Received,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.UtcNow,
                EstimatedCompletionTime = dto.EstimatedCompletionTime
            };

            var created = await _repo.CreateAsync(order);
            await _notificationService.NotifyAsync(created!, created!.Status);
            return ToDto(created!);
        }

        public async Task<OrderDTO?> UpdateAsync(int id, UpdateOrderDTO dto)
        {
            var order = await _repo.GetByIdOrderAsync(id);
            if (order is null) return null;

            bool statusChanged = false;
            OrderStatus? newStatusForNotification = null;

            if (dto.Status.HasValue && dto.Status.Value != order.Status)
            {
                var newStatus = dto.Status.Value;

                if (!AllowedTransitions[order.Status].Contains(newStatus))
                    throw new InvalidOperationException(
                        $"Cannot change order status from {order.Status} to {newStatus}.");

                order.Status = newStatus;
                statusChanged = true;
                newStatusForNotification = newStatus;
            }

            if (dto.EstimatedCompletionTime is not null) order.EstimatedCompletionTime = dto.EstimatedCompletionTime;
            order.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(order);

            if (statusChanged)
                await _notificationService.NotifyAsync(order, newStatusForNotification!.Value);

            var updated = await _repo.GetByIdOrderAsync(id);
            return ToDto(updated!);
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _repo.ExistsAsync(id);

        public async Task<PagedResult<OrderDTO>> GetAllForCustomerAsync(int customerId, int page, int pageSize)
        {
            var orders = await _repo.GetAllOrdersForCustomerAsync(customerId, page, pageSize);
            var totalCount = await _repo.GetTotalCountForCustomerAsync(customerId);

            return new PagedResult<OrderDTO>
            {
                Data = orders.Select(ToDto),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private static OrderDTO ToDto(Order o) => new OrderDTO
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            CustomerName = o.Customer?.Name ?? string.Empty,
            OrderItems = o.OrderItems
                .Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    MenuItemId = oi.MenuItemId,
                    MenuItemName = oi.MenuItem?.Name ?? string.Empty,
                    MenuItemPrice = oi.MenuItem?.Price ?? 0,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                    SpecialInstructions = oi.SpecialInstructions
                })
                .ToList(),
            Status = o.Status,
            TotalPrice = o.TotalPrice,
            EstimatedCompletionTime = o.EstimatedCompletionTime,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt
        };
    }
}