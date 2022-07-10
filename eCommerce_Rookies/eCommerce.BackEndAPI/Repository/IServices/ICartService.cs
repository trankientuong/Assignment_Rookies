using eCommerce.BackEndAPI.Models.DTOs.CartService;

namespace eCommerce.BackEndAPI.Repository.IServices
{
    public interface ICartService
    {
        Task<ProductInCartDto> GetProductToCart(int productId);
        Task<CartDto> GetCartByUserId(string userId);
        Task<CartDto> CreateUpdateCart(CartDto cartDto);
        Task<CartDto> UpdateQuantityCart(CartDto cartDto);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ClearCart(string userId);
    }
}
