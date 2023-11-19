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
                .HasKey(nameof(Product.part_number), nameof(Product.company));
            modelBuilder.Entity<Image>()
                .HasKey(nameof(Image.name), nameof(Image.company));
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
