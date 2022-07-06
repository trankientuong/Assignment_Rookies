using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.CustomerWeb.Services;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

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
            ViewBag.categoryId = categoryId;
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
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

        [HttpGet]
        public async Task<IActionResult> Details(int productId)
        {
            var client = _httpClient.CreateClient();
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.DETAILS}/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAs<ProductDetailsDto>();
                return View(product);
            }
            return null;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReviewAndRating(ReviewRatingVM reviewRating)
        {
            var accountId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            reviewRating.AccountId = accountId;
            reviewRating.DateReview = DateTime.Now;
            reviewRating.Title = SetTitleOfRating(reviewRating.Rating);
            var json = JsonConvert.SerializeObject(reviewRating);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClient.CreateClient();
            var accessToken = _httpContextAccessor.HttpContext.Session.GetString("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.PostAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.REVIEW_RATING}",httpContent);
            if (response.IsSuccessStatusCode)
            {
                var productRating = await response.Content.ReadAs<ProductRatingVM>();
                return Json(new { success = true, data = productRating });
            }
            return Json(new { success = false });
        }

        private string SetTitleOfRating(double rating)
        {
            string title = "";
            switch (rating)
            {
                case 1:
                    title = "Very Bad";
                    break;
                case 2:
                    title = "Poor";
                    break;
                case 3:
                    title = "OK";
                    break;
                case 4:
                    title = "Very Good";
                    break;
                case 5:
                    title = "Excellent";
                    break;
                default:                    
                    break;
            }
            return title;
        }
    }
}
