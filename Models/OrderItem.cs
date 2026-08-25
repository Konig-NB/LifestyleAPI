using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.Models
{
    public class OrderItem
    {
        public int Id {get; set;}

        [Required]
        public int OrderId {get; set;}
        public Order Order {get; set;} = null!;

        [Required]
        public int MenuItemId {get; set;}
        public Menu MenuItem {get; set;} = null!;

        [Required,Range(1,10,ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity {get; set;}

        [Required]
        public decimal TotalPrice {get; set;}
        
        [StringLength(250)]
        public string? SpecialInstructions {get; set;}
    }
}