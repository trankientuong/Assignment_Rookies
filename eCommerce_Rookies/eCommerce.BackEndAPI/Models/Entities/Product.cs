namespace eCommerce.BackEndAPI.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public virtual List<ProductImages> Images { get; set; }
        public virtual List<ProductRating> ProductRatings { get; set; }
    }
}
