using AutoMapper;
using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Models.Entities;
using System.Linq;

namespace eCommerce.BackEndAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<List<ProductDetailsDto>, ProductsDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
            CreateMap<Product, CreateProductDto>()
                .ReverseMap();
            CreateMap<Product, ProductDetailsDto>()
                .ReverseMap();
        }
    }
}
