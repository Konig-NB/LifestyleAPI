using System.ComponentModel.DataAnnotations;
using LifestyleAPI.Models;

namespace LifestyleAPI.DTOs
{
    public class UpdateUserDTO
    {
        [StringLength(100)]
        public string? Name {get; set;}
    }

    public class UserDTO
    {
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string PhoneNumber {get; set;} = string.Empty;
        public UserRole Role {get; set;} = UserRole.Customer;
        public DateTime CreatedAt {get; set;}
    }
}