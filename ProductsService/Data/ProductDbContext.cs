using Microsoft.EntityFrameworkCore;
using ProductsService.Model;

namespace ProductsService.Data
{
    public class ProductDbContext :DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> opt)
            : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(nameof(Product.Article), nameof(Product.Company));
                

            
        }

        public DbSet<Product> Products { get; set; }
    }
}
