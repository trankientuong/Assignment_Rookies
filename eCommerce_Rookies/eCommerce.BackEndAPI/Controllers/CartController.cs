using eCommerce.BackEndAPI.Models.DTOs.CartService;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetProductToCart(int productId)
        {
            var product = await _cartService.GetProductToCart(productId);
            return Ok(product);
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartService.GetCartByUserId(userId);
                return Ok(cartDto);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartService.CreateUpdateCart(cartDto);
                return Ok(cartDt);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartService.UpdateQuantityCart(cartDto);
                return Ok(cartDt);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveCart([FromBody] int cartId)
        {
            try
            {
                bool isSuccess = await _cartService.RemoveFromCart(cartId);
                return Ok(isSuccess);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
