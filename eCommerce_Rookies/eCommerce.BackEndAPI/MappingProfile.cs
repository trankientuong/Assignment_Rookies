﻿using AutoMapper;
using eCommerce.BackEndAPI.Models.DTOs.AuthService;
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
            CreateMap<List<ProductDetailsDto>, ProductsDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Images, act => act.Ignore());
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Images, act => act.Ignore());
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, act => act.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
            CreateMap<ProductImages, ProductImagesDto>()
                .ReverseMap();
            CreateMap<CreateProductRatingDto, ProductRating>();
            CreateMap<ProductRating, ProductRatingsDto>()
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


        }
    }
}
