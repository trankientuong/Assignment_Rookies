using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace eCommerce.CustomerWeb.Views.Shared.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        public CartViewComponent(IHttpContextAccessor httpContext, IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<CartDto> GetCartOfUser()
        {
            var accountId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                return cartDto;
            }
            return cartDto;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await GetCartOfUser();
            return View("Cart", cart);
        }

    }
}
