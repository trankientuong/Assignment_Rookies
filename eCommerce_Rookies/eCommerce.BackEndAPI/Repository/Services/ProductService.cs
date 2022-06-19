using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Repository.Services
{
    public class ProductService : IProductService
    {
        private readonly eCommerceDbContext _db;

        public ProductService(eCommerceDbContext db)
        {
            _db = db;

        }
        public async Task<ProductDetailsDto> CreateProductAsync(CreateProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDetailsDto> DeleteProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDetailsDto> GetDetailsProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductsDto>> ListProductsAsync()
        {
            using (_db)
            {
                var products = await _db.Products.Include(c => c.Category).ToListAsync();
                if(products != null)
                {
                    var productsDto = new List<ProductsDto>();
                    foreach(var product in products)
                    {
                        foreach(var productDto in productsDto)
                        {
                            productDto.Id = product.Id;
                            productDto.ProductName = product.ProductName;
                            productDto.Description = product.Description;
                            productDto.CategoryName = product.Category.CategoryName;
                            productDto.Price = product.Price;
                            productDto.Images = product.Images;
                        }
                    }
                    return productsDto;
                }
            };
            return null;
        }

        public async Task<ProductDetailsDto> UpdateProductAsync(int productId, UpdateProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
