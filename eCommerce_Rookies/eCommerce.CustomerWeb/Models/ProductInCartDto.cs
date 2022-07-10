namespace eCommerce.CustomerWeb.Models
{
    public class ProductInCartDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
    }
}
