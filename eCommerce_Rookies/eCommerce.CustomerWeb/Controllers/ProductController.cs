using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.CustomerWeb.Services;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace eCommerce.CustomerWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClient;
        public ProductController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClient)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        private async Task<List<CategoryViewModel>> GetCategories()
        {
            var accessToken = _httpContextAccessor.HttpContext.Session.GetString("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.CategoryService.CATEGORIES}");
            if (!response.IsSuccessStatusCode) return null;
            var categories = await response.Content.ReadAs<List<CategoryViewModel>>();
            return categories;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadProducts(int? page, int? pageSize)
        {
            var accessToken =  _httpContextAccessor.HttpContext.Session.GetString("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.PRODUCTS}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var products = await response.Content.ReadAs<ProductsViewModel>();
            return Json(new { items = products });
        }

        public async Task<IActionResult> LoadProductsByCategory(int categoryId)
        {
            var accessToken = _httpContextAccessor.HttpContext.Session.GetString("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.PRODUCTS_BY_CATEGORY}/{categoryId}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var products = await response.Content.ReadAs<List<ProductDetailsDto>>();
            return Json(new { items = products });
        }
    }
}
