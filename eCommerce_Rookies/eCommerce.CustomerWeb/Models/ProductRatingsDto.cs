namespace eCommerce.CustomerWeb.Models
{
    public class ProductRatingsDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public DateTime DateReview { get; set; }
        public double Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public int Helpful { get; set; }
        public int Unhelpful { get; set; }
    }
}
