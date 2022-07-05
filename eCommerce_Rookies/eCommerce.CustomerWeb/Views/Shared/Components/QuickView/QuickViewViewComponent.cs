using eCommerce.CustomerWeb.Extensions;
using eCommerce.CustomerWeb.Models;
using eCommerce.SharedLibrary;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.CustomerWeb.Views.Shared.Components.QuickView
{
    public class QuickViewViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public QuickViewViewComponent(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        public async Task<ProductDetailsDto> ProductDetails(int productId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["EndPoints:BackEnd"]}/{EndpointConstants.ProductService.DETAILS}/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAs<ProductDetailsDto>();
                return product;
            }
            return null;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var QuickView = await ProductDetails(productId);
            return View("QuickView", QuickView);
        }
    }
}
