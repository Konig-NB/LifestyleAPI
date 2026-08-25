using LifestyleAPI.Models;

namespace LifestyleAPI.DTOs
{
    public class NotificationDTO
    {
        public int Id {get; set;}
        public int OrderId { get;set;}
        public OrderStatus Status {get; set;}
        public string Message {get; set;} = string.Empty;
        public DateTime CreatedAt {get; set;}
        public bool IsRead {get; set;}
    }
}