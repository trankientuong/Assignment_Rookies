using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace eCommerce.CustomerWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClient;
        public CategoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClient)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = _httpContextAccessor.HttpContext.Session.GetString("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.CategoryService.CATEGORIES}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var categories = await response.Content.ReadAs<List<CategoryViewModel>>();
            return Json(new { items = categories });
        }
    }
}
