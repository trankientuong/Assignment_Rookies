using AutoMapper;
using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Models.Entities;
using eCommerce.BackEndAPI.Repository.IServices;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Repository.Services
{
    public class ProductService : IProductService
    {
        private readonly eCommerceDbContext _db;
        private readonly IMapper _mapper;

        public ProductService(eCommerceDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ProductDetailsDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            using (_db)
            {
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                var productDetailsDto = _mapper.Map<ProductDetailsDto>(product);
                return productDetailsDto;
            }
            return null;
        }

        public async Task<ProductDetailsDto> DeleteProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDetailsDto> GetDetailsProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductsDto> ListProductsAsync(int? page,int? pageSize)
        {
            using (_db)
            {
                var products = await _db.Products.Include(c => c.Category).ToListAsync();
                if(products != null)
                {
                    var _pageSize = pageSize ?? 8; // Số bản ghi trên trang
                    var pageIndex = page ?? 1; // Số trang hiện tại nếu null thì bằng 1
                    var totalPage = products.Count;
                    var numberPage = Math.Ceiling((float)totalPage / _pageSize);
                    var startPage = (pageIndex - 1) * _pageSize;
                    products = products.Skip(startPage).Take(_pageSize).ToList();
                    var ListProductDetailsDto = _mapper.Map<List<ProductDetailsDto>>(products);
                    var productsDto = _mapper.Map<ProductsDto>(ListProductDetailsDto);
                    productsDto.TotalItem = totalPage;
                    productsDto.CurrentPage = pageIndex;
                    productsDto.NumberPage = numberPage;
                    productsDto.PageSize = _pageSize;
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
