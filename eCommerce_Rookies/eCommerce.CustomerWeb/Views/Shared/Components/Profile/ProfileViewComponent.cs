using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommerce.CustomerWeb.Views.Shared.Components.Profile
{
    public class ProfileViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;    
        public ProfileViewComponent(IHttpClientFactory clientFactory, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;

            _httpContext = httpContext;
        }

        public async Task<UserDetailsVM> GetUserProfile()
        {
            var accountId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.AccountService.PROFILE}/{accountId}");
            if (response.IsSuccessStatusCode)
            {
                var userProfile = await response.Content.ReadAs<UserDetailsVM>();
                return userProfile;
            }
            return null;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userProfile = await GetUserProfile();
            return View("Profile", userProfile);
        }
    }
}
