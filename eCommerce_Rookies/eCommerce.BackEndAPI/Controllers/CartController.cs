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

        [HttpGet("GetCart/{userId}")]
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

        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartService.CreateUpdateCart(cartDto);
                return Ok(cartDt);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("UpdateCart")]
        public async Task<IActionResult> UpdateCart(CartDto cartDto)
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

        [HttpPost("RemoveCart")]
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
