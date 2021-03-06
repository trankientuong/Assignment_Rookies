using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{page}/{pageSize}")]
        public async Task<IActionResult> GetProducts(int? page, int? pageSize)
        {
            var products = await _productService.ListProductsAsync(page, pageSize);
            if (products == null) return BadRequest();
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetProductDetails(int productId)
        {
            var product = await _productService.GetDetailsProductAsync(productId);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{categoryId}/{page}/{pageSize}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId,int? page,int? pageSize)
        {
            var products = await _productService.GetProductsByCategory(categoryId,page,pageSize);
            if (products == null) return NotFound();
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProduct)
        {
            var product = await _productService.CreateProductAsync(createProduct);
            if (product == null) return BadRequest();
            return Ok(product);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ReviewAndRatingProduct([FromBody] CreateProductRatingDto createProductRatingDto)
        {
            if (createProductRatingDto == null) return BadRequest();
            var productRatingDto = await _productService.WriteReviewAndRatingAsync(createProductRatingDto);
            return Ok(productRatingDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromForm] UpdateProductDto updateProduct)
        {
            var product = await _productService.UpdateProductAsync(productId, updateProduct);
            if (product == null) return BadRequest();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _productService.DeleteProductAsync(productId);
            if (product == null) return NotFound();
            return Ok(product);
        }
    }
}
