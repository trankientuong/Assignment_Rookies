using System.ComponentModel.DataAnnotations;

namespace eCommerce.CustomerWeb.Models
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
