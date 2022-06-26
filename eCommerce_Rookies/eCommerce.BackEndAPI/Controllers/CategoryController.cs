using eCommerce.BackEndAPI.Models.DTOs.CategoryService;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategoryDetails(int id)
        {
            var categories = await _categoryService.GetCategoryDetailsAsync(id);
            if(categories == null) return NotFound();
            return Ok(categories);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto createDto)
        {
            var category = await _categoryService.CreateCategoryAsync(createDto);
            return Ok(category);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto updateDto)
        {
            var category = await _categoryService.UpdateCategoryAsync(id, updateDto);
            if(category == null) return NotFound();
            return Ok(category);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.DeleteCategoryAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
    }
}
