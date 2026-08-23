using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.Models
{
    public enum OrderStatus
    {
        Recieved,
        InProgress,
        Completed,
        Cancelled
    }
    public class Order
    {
        public int Id {get; set;}

        [Required]
        public int CustomerId {get; set;}
        public User Customer {get; set;} = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [Required]
        public OrderStatus Status {get; set;} = OrderStatus.Recieved;

        [Required]
        public double TotalPrice {get; set;} //Collection of OrderItems TotalPrice that from all the order items the customer ordered

        [Required]
        public DateTime CreatedAt {get; set;}
        public DateTime? UpdatedAt {get; set;}
        public TimeOnly? EstimatedCompletionTime {get; set;}
    }
}