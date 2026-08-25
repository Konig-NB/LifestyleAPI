using System.ComponentModel.DataAnnotations;
using LifestyleAPI.Models;

namespace LifestyleAPI.DTOs
{
    public class CreateOrderDTO
    {
        public ICollection<CreateOrderItemDTO> OrderItems { get; set; } = new List<CreateOrderItemDTO>();
        public TimeOnly? EstimatedCompletionTime { get; set; }
    }

    public class UpdateOrderDTO
    {
        public OrderStatus? Status {get; set;}
        public TimeOnly? EstimatedCompletionTime {get; set;}
    }

    public class OrderDTO
    {
        public int Id {get; set;}
        public int CustomerId {get; set;}
        public string CustomerName {get; set;} = string.Empty;
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public OrderStatus Status {get; set;} = OrderStatus.Received;
        public decimal TotalPrice {get; set;}
        public TimeOnly? EstimatedCompletionTime {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime? UpdatedAt {get; set;}
    }
}