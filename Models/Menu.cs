using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.Models
{
    public class Menu
    {
        public int Id {get; set;}

        [Required]
        public int CategoryId {get; set;}
        public Category Category {get; set;} = null!;

        [Required,StringLength(100)]
        public string Name {get; set;} = string.Empty;

        [Required,StringLength(250)]
        public string Description {get; set;} = string.Empty;

        [Required,Range(1, double.MaxValue)]
        public decimal Price {get; set;}

        [Required]
        public bool IsAvailable {get; set;}

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}