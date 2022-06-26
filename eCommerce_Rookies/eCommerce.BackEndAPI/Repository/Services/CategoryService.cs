using AutoMapper;
using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.DTOs.CategoryService;
using eCommerce.BackEndAPI.Models.Entities;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Repository.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly eCommerceDbContext _db;
        private readonly IMapper _mapper;

        public CategoryService(eCommerceDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<CategoryDetailsDto> CreateCategoryAsync(CategoryCreateDto createDto)
        {
            var category = _mapper.Map<Category>(createDto);
            using (_db)
            {
                await _db.Categories.AddAsync(category);
                await _db.SaveChangesAsync();
                var categoryDetailsDto = _mapper.Map<CategoryDetailsDto>(category);
                return categoryDetailsDto;
            }
        }

        public async Task<CategoryDetailsDto> DeleteCategoryAsync(int id)
        {
            using (_db)
            {
                var category = await _db.Categories.SingleOrDefaultAsync(c => c.Id == id);
                if(category != null)
                {
                    _db.Categories.Remove(category);
                    await _db.SaveChangesAsync();
                    var categoryDetailsDto = _mapper.Map<CategoryDetailsDto>(category);
                    return categoryDetailsDto;
                }
            }
            return null;
        }

        public async Task<List<CategoriesDto>> GetCategoriesAsync()
        { 
            using (_db)
            {
                var categories = await _db.Categories.ToListAsync();
                var categoriesDto = _mapper.Map<List<CategoriesDto>>(categories);
                return categoriesDto;
            }
        }

        public async Task<CategoryDetailsDto> GetCategoryDetailsAsync(int id)
        {
            using (_db)
            {
                var category = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
                if(category != null)
                {
                    var categoryDetailsDto = _mapper.Map<CategoryDetailsDto>(category);
                    return categoryDetailsDto;
                }                
            }
            return null;
        }

        public async Task<CategoryDetailsDto> UpdateCategoryAsync(int id, CategoryUpdateDto updateDto)
        {
            using (_db)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
                category = _mapper.Map(updateDto, category);
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
                var categoryDetailsDto = _mapper.Map<CategoryDetailsDto>(category);
                return categoryDetailsDto;
            }
        }
    }
}
