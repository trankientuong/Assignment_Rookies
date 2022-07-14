using eCommerce.BackEndAPI.Models.DTOs.ProductService;
using eCommerce.BackEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.UnitTest.MockData
{
    public class ProductMockData
    {
        public static ProductsDto getProducts()
        {
            return new ProductsDto()
            {
                CurrentPage = 1,
                NumberPage = 1,
                PageSize = 2,
                TotalItem = 2,
                Products = new List<ProductsViewDto>()
                {
                    new ProductsViewDto
                    {
                        Id = 1,
                    ProductName = "Watch 1",
                    CategoryName = "Rolex",
                    TotalRating = 5,
                    TotalReview = 1,
                    Price = 25000,
                    Description = "Description test",
                    Images = new List<ProductImagesDto>()
                    {
                        new ProductImagesDto
                        {
                            Id = 1,
                            ProductId = 1,
                            Image = "1.jpg"
                        }
                    }
                    },
                    new ProductsViewDto
                    {
                        Id = 2,
                    ProductName = "Watch 2",
                    CategoryName = "Rolex",
                    TotalRating = 10,
                    TotalReview = 2,
                    Price = 25000,
                    Description = "Description test2",
                    Images = new List<ProductImagesDto>()
                    {
                        new ProductImagesDto
                        {
                            Id = 1,
                            ProductId = 2,
                            Image = "1.jpg"
                        }
                    }
                    }
                }



            };
        }
    }
}
