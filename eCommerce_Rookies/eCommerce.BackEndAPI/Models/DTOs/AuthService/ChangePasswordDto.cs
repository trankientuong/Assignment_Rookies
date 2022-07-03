using System.ComponentModel.DataAnnotations;

namespace eCommerce.BackEndAPI.Models.DTOs.AuthService
{
    public class ChangePasswordDto
    {
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
