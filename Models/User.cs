using System.ComponentModel.DataAnnotations;

namespace LifestyleAPI.Models
{
    public enum UserRole
    {
        Admin,
        Owner,
        Customer
    }
    public class User
    {
        public int Id {get; set;}

        [Required,StringLength(100)]
        public string Name {get; set;} = string.Empty;

        [Required, StringLength(12), MinLength(12)]
        public string PhoneNumber {get; set;} = string.Empty;
        
        [Required]
        public string PasswordHash {get; set;} = string.Empty;

        [Required]
        public UserRole Role {get; set;} = UserRole.Customer;
        public DateTime CreatedAt {get; set;}
    }
}