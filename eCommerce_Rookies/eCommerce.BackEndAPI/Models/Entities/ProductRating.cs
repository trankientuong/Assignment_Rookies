using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.BackEndAPI.Models.Entities
{
    public class ProductRating
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public string AccountId { get; set; }
        [ForeignKey("AccountId")]
        public IdentityUser User { get; set; }
        public DateTime DateReview { get; set; }
        public double Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public int? Helpful { get; set; }
        public int? Unhelpful { get; set; }
    }
}
