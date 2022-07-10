namespace eCommerce.CustomerWeb.Models
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
