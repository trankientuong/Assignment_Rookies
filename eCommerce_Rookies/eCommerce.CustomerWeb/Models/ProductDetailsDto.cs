using System.ComponentModel.DataAnnotations;

namespace eCommerce.CustomerWeb.Models
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<ProductImagesDto> Images { get; set; }
        public List<ProductRatingsDto> ProductRatings { get; set; }
        public string CategoryName { get; set; }
        [Range(1,100)]
        public int Count { get; set; }
        public ProductDetailsDto()
        {
            Count = 1;
            this.Images = new List<ProductImagesDto>();
            this.ProductRatings = new List<ProductRatingsDto>();
        }
    }
}
