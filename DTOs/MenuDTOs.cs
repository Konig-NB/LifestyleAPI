using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.DTOs
{
    public class CreateMenuDTO
    {
        [Required]
        public int CategoryId {get; set;}

        [Required,StringLength(100)]
        public string Name {get; set;} = string.Empty;

        [Required,StringLength(250)]
        public string Description {get; set;} = string.Empty;

        [Required,Range(0.1, double.MaxValue)]
        public double Price {get; set;}

        [Required]
        public bool IsAvailable {get; set;}
    }

    public class UpdateMenuDTO
    {
        public int? CategoryId {get; set;}

        [StringLength(100)]
        public string? Name {get; set;}

        [StringLength(250)]
        public string? Description {get; set;}

        [Range(0.1, double.MaxValue)]
        public double? Price {get; set;}

        [Required]
        public bool? IsAvailable {get; set;}
    }

    public class MenuDTO
    {
        public int Id {get; set;}

        public int CategoryId {get; set;}
        public string CategoryName {get; set;} = string.Empty;

        public string Name {get; set;} = string.Empty;

        public string Description {get; set;} = string.Empty;

        public double Price {get; set;}

        public bool IsAvailable {get; set;}
    }
}