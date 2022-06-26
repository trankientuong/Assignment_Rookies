using System.ComponentModel.DataAnnotations;

namespace eCommerce.BackEndAPI.Models.DTOs.AuthService
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
