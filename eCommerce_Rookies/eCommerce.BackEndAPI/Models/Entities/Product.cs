namespace eCommerce.BackEndAPI.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Images { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
