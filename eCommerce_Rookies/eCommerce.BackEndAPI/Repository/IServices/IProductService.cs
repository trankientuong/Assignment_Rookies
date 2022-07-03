using eCommerce.BackEndAPI.Models.DTOs.ProductService;

namespace eCommerce.BackEndAPI.Repository.IServices
{
    public interface IProductService
    {
        Task<ProductsDto> ListProductsAsync(int? page, int? pageSize);
        Task<ProductDetailsDto> GetDetailsProductAsync(int productId);
        Task<ProductDetailsDto> CreateProductAsync(CreateProductDto productDto);
        Task<ProductDetailsDto> UpdateProductAsync(int productId,UpdateProductDto productDto);
        Task<ProductDetailsDto> DeleteProductAsync(int productId);
        Task<ProductRatingsDto> WriteReviewAndRatingAsync(CreateProductRatingDto productRatingsDto);        
    }
}
