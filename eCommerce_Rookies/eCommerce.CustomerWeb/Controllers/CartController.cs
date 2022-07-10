using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace eCommerce.CustomerWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        public CartController(IHttpContextAccessor httpContext, IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public IActionResult GetCartViewComponent()
        {
            return ViewComponent("Cart");//it will call cart.cs InvokeAsync.
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {            
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var accessToken = _httpContext.HttpContext.Session.GetString("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.CartService.GET_CART}/{accountId}");
            CartDto cartDto = new();
            if (response.IsSuccessStatusCode)
            {
                cartDto = await response.Content.ReadAs<CartDto>();
                if (cartDto.CartHeader != null)
                {
                    foreach (var details in cartDto.CartDetails)
                    {
                        cartDto.CartHeader.OrderTotal += (details.Product.Price * details.Count);
                    }
                }
                return View(cartDto);
            }

            return View(cartDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(ProductDetailsDto productDto)
        {
            var client = _httpClient.CreateClient();
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.Id
            };
            var response = await client.GetAsync($"{_configuration["Endpoints:BackEnd"]}/{EndpointConstants.CartService.PRODUCT_TO_CART}/{productDto.Id}");
            if (response != null && response.IsSuccessStatusCode)
            {
                cartDetails.Product = await response.Content.ReadAs<ProductInCartDto>();
            }
            List<CartDetailsDto> cartDetailsDtos = new List<CartDetailsDto>();
            cartDetailsDtos.Add(cartDetails);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = _httpContext.HttpContext.Session.GetString("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = JsonConvert.SerializeObject(cartDto);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var addToCartResponse = await client.PostAsync($"{_configuration["Endpoints:BackEnd"]}/{EndpointConstants.CartService.ADD_CART}", httpContent);
            if (addToCartResponse != null && addToCartResponse.IsSuccessStatusCode)
            {
                var cart = await addToCartResponse.Content.ReadAs<CartDto>();
                return Json(new { success = true, item = cart });
            }
            return Json(new { success = false });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateCart(ProductDetailsDto productDto)
        {
            var client = _httpClient.CreateClient();
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.Id
            };
            var response = await client.GetAsync($"{_configuration["Endpoints:BackEnd"]}/{EndpointConstants.CartService.PRODUCT_TO_CART}/{productDto.Id}");
            if (response != null && response.IsSuccessStatusCode)
            {
                cartDetails.Product = await response.Content.ReadAs<ProductInCartDto>();
            }
            List<CartDetailsDto> cartDetailsDtos = new List<CartDetailsDto>();
            cartDetailsDtos.Add(cartDetails);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = _httpContext.HttpContext.Session.GetString("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = JsonConvert.SerializeObject(cartDto);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var updateCartResponse = await client.PostAsync($"{_configuration["Endpoints:BackEnd"]}/{EndpointConstants.CartService.UPDATE_CART}", httpContent);
            if (updateCartResponse != null && updateCartResponse.IsSuccessStatusCode)
            {
                var cart = await updateCartResponse.Content.ReadAs<CartDto>();
                return Json(new { success = true, item = cart });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> RemoveCart(int cartDetailsId)
        {
            var client = _httpClient.CreateClient();
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var accessToken = _httpContext.HttpContext.Session.GetString("access_token");
            var json = JsonConvert.SerializeObject(cartDetailsId);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_configuration["Endpoints:BackEnd"]}/{EndpointConstants.CartService.REMOVE_CART}",httpContent);
            if(response != null && response.IsSuccessStatusCode)
            {
                return Json(new { success = true});
            }
            return Json(new { success = false });
        }

    }
}
