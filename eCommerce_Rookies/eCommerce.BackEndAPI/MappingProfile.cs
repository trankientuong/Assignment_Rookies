using AutoMapper;
using eCommerce.BackEndAPI.Models.DTOs.AuthService;
using eCommerce.BackEndAPI.Models.DTOs.CartService;
using eCommerce.BackEndAPI.Models.DTOs.CategoryService;
using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Models.Entities;
using System.Linq;

namespace eCommerce.BackEndAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<List<ProductsViewDto>, ProductsDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Images, act => act.Ignore());
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Images, act => act.Ignore());
            CreateMap<Product, ProductsViewDto>()
                .ForMember(dest => dest.CategoryName, act => act.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TotalReview,act => act.MapFrom(src => src.ProductRatings.Count))
                .ForMember(dest => dest.TotalRating,act => act.MapFrom(src => src.ProductRatings.Select(c => c.Rating).Sum()))
                .ReverseMap();
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, act => act.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
            CreateMap<ProductImages, ProductImagesDto>()
                .ReverseMap();
            CreateMap<CreateProductRatingDto, ProductRating>();
            CreateMap<ProductRating, ProductRatingsDto>()
                .ForMember(dest => dest.UserName,act => act.MapFrom(src => src.User.UserName))
                .ReverseMap();



            CreateMap<Category, CategoriesDto>()
                .ReverseMap();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryDetailsDto, Category>()
                .ReverseMap();
            CreateMap<CategoryUpdateDto, Category>();


            CreateMap<UpdateProfileDto, UserProfile>()
                .ForMember(dest => dest.Avatar,act => act.Ignore());
            CreateMap<UserProfile, UserDetailsDto>()
                .ReverseMap();

            CreateMap<Product, ProductInCartDto>()
                .ForMember(dest => dest.Image,act => act.MapFrom(src => src.Images.FirstOrDefault().Image))
                .ForMember(dest => dest.CategoryName,act => act.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
            CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
            CreateMap<CartDetailsDto, CartDetails>()
                .ReverseMap();
            CreateMap<CartDto, Cart>()
                .ReverseMap();

        }
    }
}
