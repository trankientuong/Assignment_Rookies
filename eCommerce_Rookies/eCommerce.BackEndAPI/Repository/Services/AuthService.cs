using AutoMapper;
using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.DTOs.AuthService;
using eCommerce.BackEndAPI.Models.Entities;
using eCommerce.BackEndAPI.Repository.IServices;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Repository.Services
{
    public class AuthService : IAuthService
    {
        private readonly eCommerceDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFileService _fileService;

        public AuthService(eCommerceDbContext db,IMapper mapper,UserManager<IdentityUser> userManager,IFileService fileService)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _fileService = fileService;
        }

        public async Task<UserDetailsDto> UpdateUserProfileAsync(UpdateProfileDto updateProfile)
        {            
            using (_db)
            {
                var userProfile = await _db.UserProfile.Include(u => u.User)
                                                       .Where(x => x.AccountId == updateProfile.AccountId)
                                                       .FirstOrDefaultAsync();
                if(updateProfile.Avatar != null)
                {
                    if(userProfile.Avatar != null)
                    {
                        await _fileService.DeleteFile(userProfile.Avatar, ImageConstants.AVATARS_PATH);
                    }
                    userProfile.Avatar = await _fileService.SaveFile(updateProfile.Avatar, ImageConstants.AVATARS_PATH);
                }
                userProfile = _mapper.Map(updateProfile, userProfile);
                _db.UserProfile.Update(userProfile);
                await _db.SaveChangesAsync();
                var userProfileDto = _mapper.Map<UserDetailsDto>(userProfile);
                return userProfileDto;
            }
        }        
    }
}
