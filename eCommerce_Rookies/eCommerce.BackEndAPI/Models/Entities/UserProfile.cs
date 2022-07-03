using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.BackEndAPI.Models.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }       
        public string AccountId { get; set; }
        [ForeignKey("AccountId")]
        public IdentityUser User { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
    }
}
