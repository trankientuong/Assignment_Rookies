using System.ComponentModel.DataAnnotations;

namespace eCommerce.CustomerWeb.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnURL { get; set; }

    }
}
