using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using LifestyleAPI.DTOs;
using LifestyleAPI.Services.Interfaces;
using LifestyleAPI.Helpers;

namespace LifestyleAPI.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repo;
        private readonly IMenuRepository _menuRepo;
        private readonly IOrderRepository _orderRepo;
        public OrderItemService(IOrderItemRepository repo, IMenuRepository menuRepo, IOrderRepository orderRepo) 
        { 
            _repo = repo;
            _menuRepo = menuRepo;
            _orderRepo = orderRepo;
        }

        public async Task<PagedResult<OrderItemDTO>> GetAllAsync(int page, int pageSize)
        {
            var orderItems = await _repo.GetAllOrderItemsAsync(page, pageSize);
            var totalCount = await _repo.GetTotalCountAsync();

            return new PagedResult<OrderItemDTO>
            {
                Data = orderItems.Select(ToDto),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<OrderItemDTO?> GetByIdAsync(int id)
        {
            var orderItem = await _repo.GetByIdOrderItemAsync(id);
            return orderItem is null ? null : ToDto(orderItem);
        }

        public async Task<OrderItemDTO> CreateAsync(CreateOrderItemDTO dto)
        {
            var order = await _orderRepo.GetByIdOrderAsync(dto.OrderId);
            if (order is null)
                throw new KeyNotFoundException($"Order {dto.OrderId} not found.");

            if (order.Status is OrderStatus.Completed or OrderStatus.Cancelled)
                throw new InvalidOperationException(
                    $"Cannot add items to an order that is already {order.Status}.");

            var menuItem = await _menuRepo.GetByIdMenuAsync(dto.MenuItemId);
            if (menuItem == null)
                throw new KeyNotFoundException($"Menu item {dto.MenuItemId} not found.");

            if (!menuItem.IsAvailable)
                throw new InvalidOperationException($"Menu item {menuItem.Name} is not available.");

            var orderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                MenuItemId = dto.MenuItemId,
                Quantity = dto.Quantity,
                TotalPrice = menuItem.Price * dto.Quantity,
                SpecialInstructions = dto.SpecialInstructions
            };

            var created = await _repo.CreateAsync(orderItem);
            order.OrderItems.Add(created);
            order.TotalPrice = order.OrderItems.Sum(oi => oi.TotalPrice);
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepo.UpdateAsync(order);

            var withIncludes = await _repo.GetByIdOrderItemAsync(created.Id);
            return ToDto(withIncludes!);
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _repo.ExistsAsync(id);

        private static OrderItemDTO ToDto(OrderItem oi) => new OrderItemDTO
        {
            Id = oi.Id,
            OrderId = oi.OrderId,
            CustomerName = oi.Order?.Customer?.Name ?? string.Empty,
            MenuItemId = oi.MenuItemId,
            MenuItemName = oi.MenuItem?.Name ?? string.Empty,
            MenuItemPrice = oi.MenuItem?.Price ?? 0,
            Quantity = oi.Quantity,
            TotalPrice = oi.TotalPrice,
            SpecialInstructions = oi.SpecialInstructions
        };
    }
}