namespace eCommerce.BackEndAPI.Models.DTOs.ProductService
{
    public class CreateProductRatingDto
    {
        public int ProductId { get; set; }
        public string AccountId { get; set; }
        public DateTime DateReview { get; set; }
        public double Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }
}
