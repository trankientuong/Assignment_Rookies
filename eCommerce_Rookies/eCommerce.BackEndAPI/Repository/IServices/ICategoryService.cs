using eCommerce.BackEndAPI.Models.DTOs.CategoryService;

namespace eCommerce.BackEndAPI.Repository.IServices
{
    public interface ICategoryService
    {
        Task<List<CategoriesDto>> GetCategoriesAsync();
        Task<CategoryDetailsDto> GetCategoryDetailsAsync(int id);
        Task<CategoryDetailsDto> CreateCategoryAsync(CategoryCreateDto createDto);
        Task<CategoryDetailsDto> UpdateCategoryAsync(int id,CategoryUpdateDto updateDto);
        Task<CategoryDetailsDto> DeleteCategoryAsync(int id);
    }
}
