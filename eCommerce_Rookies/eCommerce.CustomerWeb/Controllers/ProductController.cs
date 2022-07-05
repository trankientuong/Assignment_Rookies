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

        public async Task<IActionResult> Index(int? categoryId,int? page,int? pageSize)
        {
            var client = _httpClient.CreateClient();
            pageSize = pageSize ?? 12; // Số bản ghi trên trang
            page = page ?? 1; // Số trang hiện tại nếu null thì bằng 1
            if (categoryId == 0 || categoryId == null)
            {                
                var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.PRODUCTS}/{page}/{pageSize}");
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadAs<ProductsViewModel>();
                    return View(products);
                }                
            }
            else
            {
                var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.PRODUCTS_BY_CATEGORY}/{categoryId}/{page}/{pageSize}");
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadAs<ProductsViewModel>();
                    return View(products);
                }                
            }
            return NotFound();
        }

        public async Task<IActionResult> LoadProducts(int? page, int? pageSize)
        {
            var client = _httpClient.CreateClient();
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.PRODUCTS}/{page}/{pageSize}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var products = await response.Content.ReadAs<ProductsViewModel>();
            return Json(new { items = products });
        }

        [HttpGet]
        public async Task<IActionResult> LoadProductsByCategory(int categoryId,int? page,int? pageSize)
        {
            var client = _httpClient.CreateClient();
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.PRODUCTS_BY_CATEGORY}/{categoryId}/{page}/{pageSize}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var products = await response.Content.ReadAs<ProductsViewModel>();
            return Json(new { items = products });
        }
    }
}
