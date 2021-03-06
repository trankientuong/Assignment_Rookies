using AutoMapper;
using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.DTOs.CartService;
using eCommerce.BackEndAPI.Models.Entities;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Repository.Services
{
    public class CartService : ICartService
    {
        private readonly eCommerceDbContext _db;
        private readonly IMapper _mapper;

        public CartService(eCommerceDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeaderFromDb != null)
            {
                _db.CartDetails.RemoveRange(_db.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _db.CartHeaders.Remove(cartHeaderFromDb);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);

            //check if header is null
            var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                // Create header and details
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _db.SaveChangesAsync();
            }
            else
            {
                // if header is not null
                // check if details has same product
                var cartDetailsFromDb = await _db.CartDetails.AsNoTracking()
                                                             .FirstOrDefaultAsync(u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId
                                                                                 && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                if (cartDetailsFromDb == null)
                {
                    // create details
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // Update the count / cart details
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    _db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
            }
            var productInCart = cartDto.CartDetails.FirstOrDefault().Product;
            var cartDtoRes = _mapper.Map<CartDto>(cart);
            cartDtoRes.CartDetails.FirstOrDefault().Product = productInCart;
            return cartDtoRes;
        }

        public async Task<CartDto> UpdateQuantityCart(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);

            var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
            var cartDetailsFromDb = await _db.CartDetails.AsNoTracking()
                                                         .FirstOrDefaultAsync(u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId
                                                                             && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

            // Update the count / cart details
            cart.CartDetails.FirstOrDefault().Product = null;
            cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
            cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
            _db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
            await _db.SaveChangesAsync();
            var productInCart = cartDto.CartDetails.FirstOrDefault().Product;
            var cartDtoRes = _mapper.Map<CartDto>(cart);
            cartDtoRes.CartDetails.FirstOrDefault().Product = productInCart;
            return cartDtoRes;
        }


        public async Task<CartDto> GetCartByUserId(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId)
            };


            cart.CartDetails = _db.CartDetails.Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId).Include(x => x.Product).Include(x => x.Product.Images);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<ProductInCartDto> GetProductToCart(int productId)
        {
            using (_db)
            {
                var product = await _db.Products.Include(x => x.Images)
                                                .Include(x => x.Category)
                                                .FirstOrDefaultAsync(x => x.Id == productId);
                var productDto = _mapper.Map<ProductInCartDto>(product);
                return productDto;
            }
            return null;
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await _db.CartDetails.FirstOrDefaultAsync(x => x.CartDetailsId == cartDetailsId);
                int totalCountOfItems = _db.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (totalCountOfItems == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
