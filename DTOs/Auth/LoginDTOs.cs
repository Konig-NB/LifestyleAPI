using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.DTOs.Auth
{
public class LoginDto
    {
        [Required,StringLength(12)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}