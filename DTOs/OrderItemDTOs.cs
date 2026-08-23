using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.DTOs
{
    public class CreateOrderItemDTO
    {
        [Required]
        public int OrderId {get; set;}

        [Required]
        public int MenuItemId {get; set;}

        [Required,Range(1,10,ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity {get; set;}
        
        [StringLength(250)]
        public string? SpecialInstructions {get; set;}
    }

    public class OrderItemDTO
    {
        public int Id {get; set;}
        public int OrderId {get; set;}
        public string CustomerName {get; set;} = string.Empty;
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public double MenuItemPrice { get; set; }
        public int Quantity {get; set;}
        public double TotalPrice {get; set;}
        public string? SpecialInstructions {get; set;}
    }
}