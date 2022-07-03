using System.ComponentModel.DataAnnotations;

namespace eCommerce.BackEndAPI.Models.Entities
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
    }
}
