using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.CustomerWeb.Services;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eCommerce.CustomerWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClient;

        public AccountController(IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration, IHttpClientFactory httpClient)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnURL = null)
        {
            returnURL ??= Url.Content("~/");
            ViewData["returnUrl"] = returnURL;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterAndLoginVM request)
        {
            var client = _httpClient.CreateClient();
            ModelState.Remove("RegisterRequest");
            ViewBag.SelectedTab = "signin-2";
            if (!ModelState.IsValid)
                return View(request);
            var json = JsonConvert.SerializeObject(request.LoginRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.AccountService.LOGIN}", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<IDictionary<string, List<string>>>(jsonString);
                foreach (var message in messages)
                {
                    message.Value.ForEach(x =>
                    {
                        ModelState.AddModelError($"LoginRequest.{message.Key}", $"{x}");
                    });
                }
                return View();
            }
            var TokenResponse = await response.Content.ReadAs<TokenResponse>();
            var userPrincipal = this.ValidateToken(TokenResponse.AccessToken);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                IsPersistent = false
            };
            _httpContextAccessor.HttpContext.Session.SetString("access_token", TokenResponse.AccessToken);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return LocalRedirect(request.LoginRequest.ReturnURL);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnURL = null)
        {
            returnURL ??= Url.Content("~/");
            ViewData["returnUrl"] = returnURL;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAndLoginVM request)
        {
            var client = _httpClient.CreateClient();
            ModelState.Remove("LoginRequest");
            ViewBag.SelectedTab = "register-2";
            if (!ModelState.IsValid)
            {
                return View("Login", request);
            }

            var json = JsonConvert.SerializeObject(request.RegisterRequest);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.AccountService.REGISTER}", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<IDictionary<string, List<string>>>(jsonString);
                foreach (var message in messages)
                {
                    message.Value.ForEach(x =>
                    {
                        ModelState.AddModelError($"RegisterRequest.{message.Key}", $"{x}");
                    });
                }
                return View("Login");
            }
            var TokenResponse = await response.Content.ReadAs<TokenResponse>();
            var userPrincipal = this.ValidateToken(TokenResponse.AccessToken);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                IsPersistent = false
            };
            _httpContextAccessor.HttpContext.Session.SetString("access_token", TokenResponse.AccessToken);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            return LocalRedirect(request.RegisterRequest.ReturnURL);
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SymmetricKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            return principal;
        }
    }
}
