namespace eCommerce.BackEndAPI.Models.DTOs.AuthService
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
}
