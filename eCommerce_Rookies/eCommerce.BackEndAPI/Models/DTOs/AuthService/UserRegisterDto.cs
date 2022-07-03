﻿using System.ComponentModel.DataAnnotations;

namespace eCommerce.BackEndAPI.Models.DTOs.AuthService
{
    public class UserRegisterDto
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
