using eCommerce.BackEndAPI.Models.DTOs.ProductService;

namespace eCommerce.BackEndAPI.Models.DTOs.CartService
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        //public virtual CartHeaderDto CartHeader { get; set; }
        public int ProductId { get; set; }
        public virtual ProductInCartDto Product { get; set; }
        public int Count { get; set; }
    }
}
