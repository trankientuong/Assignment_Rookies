using eCommerce.BackEndAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Models
{
    public class eCommerceDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public eCommerceDbContext(DbContextOptions<eCommerceDbContext> options) : base(options)
        {

        }
    }
}
