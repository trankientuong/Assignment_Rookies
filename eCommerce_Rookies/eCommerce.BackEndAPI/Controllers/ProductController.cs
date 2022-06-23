using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProducts(int? page,int? pageSize)
        {
            var products = await _productService.ListProductsAsync(page, pageSize);
            if (products == null) return BadRequest();
            return Ok(products);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProduct)
        {
            var product = await _productService.CreateProductAsync(createProduct);
            if (product == null) return BadRequest("Failed");
            return Ok(product);
        }    
          
    }
}
