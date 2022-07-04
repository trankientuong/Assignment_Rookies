namespace eCommerce.CustomerWeb.Models
{
    public class ProductsViewModel
    {
        public List<ProductDetailsDto> Products { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public double NumberPage { get; set; }
        public int? PageSize { get; set; }
        public ProductsViewModel()
        {
            this.Products = new List<ProductDetailsDto>();
        }
    }
}
