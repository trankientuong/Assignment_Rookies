namespace eCommerce.BackEndAPI.Models.DTOs.ProductService
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public List<ProductImagesDto> Images { get; set; }
        public string CategoryName { get; set; }
        public ProductDetailsDto()
        {
            this.Images = new List<ProductImagesDto>();
        }
    }
}
