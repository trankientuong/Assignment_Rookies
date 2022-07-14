using eCommerce.BackEndAPI.Controllers;
using eCommerce.BackEndAPI.Repository.IServices;
using eCommerce.UnitTest.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.UnitTest.Systems.Controllers
{
    public class TestProductController
    {

        [Fact]
        public async Task GetAllAsync_Return200Status()
        {
            /// Arrange
            var productService = new Mock<IProductService>();
            productService.Setup(_ => _.ListProductsAsync(1, 2)).ReturnsAsync(ProductMockData.getProducts());
            var sut = new ProductController(productService.Object);

            /// Act 
            var result = (OkObjectResult)await sut.GetProducts(1, 2);

            /// Assert
            result.StatusCode.Should().Be(200);
        }
    }
}
