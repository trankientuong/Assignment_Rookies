namespace eCommerce.BackEndAPI.Models.DTOs.ProductService
{
    public class ProductsDto
    {
        public List<ProductsViewDto> Products { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public double NumberPage { get; set; }
        public int? PageSize { get; set; }
        public ProductsDto()
        {
            this.Products = new List<ProductsViewDto>();
        }
    }
}
