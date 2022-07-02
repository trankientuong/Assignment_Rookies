using eCommerce.BackEndAPI.Models.DTOs.AuthService;

namespace eCommerce.BackEndAPI.Repository.IServices
{
    public interface IAuthService
    {
        Task<UserDetailsDto> UpdateUserProfileAsync(UpdateProfileDto updateProfile);
    }
}
