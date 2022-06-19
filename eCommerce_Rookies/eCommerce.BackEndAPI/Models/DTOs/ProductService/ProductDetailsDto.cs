namespace eCommerce.BackEndAPI.Models.DTOs.ProductService
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Images { get; set; }
        public string CategoryName { get; set; }
    }
}
