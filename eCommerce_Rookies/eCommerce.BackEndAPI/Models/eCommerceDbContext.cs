using eCommerce.BackEndAPI.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Models
{
    public class eCommerceDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductRating> ProductRatings { get; set; }
        public eCommerceDbContext(DbContextOptions<eCommerceDbContext> options) : base(options)
        {

        }
    }
}
