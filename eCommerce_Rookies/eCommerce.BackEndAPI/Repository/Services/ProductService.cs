using AutoMapper;
using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Models.Entities;
using eCommerce.BackEndAPI.Repository.IServices;
using eCommerce.SharedLibrary;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Repository.Services
{
    public class ProductService : IProductService
    {
        private readonly eCommerceDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ProductService(eCommerceDbContext db, IMapper mapper, IFileService fileService)
        {
            _db = db;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<ProductDetailsDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.Images = new List<ProductImages>();
            using (_db)
            {
                var categoryOfProduct = await _db.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);
                product.Category = categoryOfProduct;
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                if (productDto.Images != null)
                {
                    foreach (var imageP in productDto.Images)
                    {
                        var productImage = new ProductImages();
                        productImage.ProductId = product.Id;
                        productImage.Image = await _fileService.SaveFile(imageP, ImageConstants.PRODUCTS_PATH);
                        product.Images.Add(productImage);
                        await _db.ProductImages.AddAsync(productImage);
                    }
                    await _db.SaveChangesAsync();
                }

                var productDetailsDto = _mapper.Map<ProductDetailsDto>(product);
                return productDetailsDto;
            }
        }

        public async Task<ProductDetailsDto> DeleteProductAsync(int productId)
        {
            using (_db)
            {
                var product = await _db.Products.Include(p => p.Images).Include(c => c.Category).FirstOrDefaultAsync(x => x.Id == productId);
                if (product != null)
                {
                    if(product.Images != null)
                    {
                        foreach (var imageP in product.Images)
                        {
                            await _fileService.DeleteFile(imageP.Image, ImageConstants.PRODUCTS_PATH);
                        }
                    }
                    _db.Products.Remove(product);
                    await _db.SaveChangesAsync();
                    var productDetailsDto = _mapper.Map<ProductDetailsDto>(product);
                    return productDetailsDto;
                }
            }
            return null;
        }

        public async Task<ProductDetailsDto> GetDetailsProductAsync(int productId)
        {
            using (_db)
            {
                var product = await _db.Products.Include(p => p.Images)
                                                .Include(p => p.Category)
                                                .FirstOrDefaultAsync(p => p.Id == productId);
                if (product != null)
                {
                    var productDetailsDto = _mapper.Map<ProductDetailsDto>(product);
                    return productDetailsDto;
                }

            }
            return null;
        }

        public async Task<ProductsDto> ListProductsAsync(int? page, int? pageSize)
        {
            using (_db)
            {
                var products = await _db.Products.Include(c => c.Category)
                                                 .Include(s => s.Images)
                                                 .ToListAsync();
                if (products != null)
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
            }
            return null;
        }

        public async Task<ProductDetailsDto> UpdateProductAsync(int productId, UpdateProductDto productDto)
        {
            using (_db)
            {
                var product = await _db.Products.Include(c => c.Category)
                                                .FirstOrDefaultAsync(x => x.Id == productId);
                var productImages = await _db.ProductImages.Where(x => x.ProductId == productId)
                                                           .ToListAsync();
                if (product != null)
                {
                    if (productDto.Images != null)
                    {
                        if (product.Images != null)
                        {
                            foreach (var imageP in productImages)
                            {
                                await _fileService.DeleteFile(imageP.Image, ImageConstants.PRODUCTS_PATH);
                            }
                            _db.ProductImages.RemoveRange(productImages);
                            await _db.SaveChangesAsync();
                        }
                        product = _mapper.Map(productDto, product);
                        foreach(var image in productDto.Images)
                        {
                            var NewProductImages = new ProductImages()
                            {
                                ProductId = productId,
                                Image = await _fileService.SaveFile(image, ImageConstants.PRODUCTS_PATH)
                            };
                            await _db.ProductImages.AddAsync(NewProductImages);
                        }
                    }
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                    var productDetailsDto = _mapper.Map<ProductDetailsDto>(product);
                    return productDetailsDto;
                }
            }
            return null;
        }
    }
}
