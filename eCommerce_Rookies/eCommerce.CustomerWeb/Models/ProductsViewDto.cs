namespace eCommerce.CustomerWeb.Models
{
    public class ProductsViewDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<ProductImagesDto> Images { get; set; }
        public int TotalReview { get; set; }
        public double TotalRating { get; set; }
        public string CategoryName { get; set; }

        public ProductsViewDto()
        {
            this.Images = new List<ProductImagesDto>();
        }
    }
}
