using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.Models
{
    public class Notification
    {
        public int Id {get; set;}

        [Required]
        public int OrderId {get; set;}
        public Order Order {get; set;} = null!;

        [Required]
        public int CustomerId {get; set;}
        public User Customer {get; set;} = null!;

        [Required]
        public OrderStatus Status {get; set;}

        [Required, StringLength(250)]
        public string Message {get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt {get; set;}

        public bool IsRead {get; set;} = false;
    }
}