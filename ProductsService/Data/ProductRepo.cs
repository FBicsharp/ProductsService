using Microsoft.EntityFrameworkCore;
using ProductsService.Model;
using System.Linq;

namespace ProductsService.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly ProductDbContext _context;

        public ProductRepo(ProductDbContext context)
        {
            _context = context;
        }

        public void CreateProduct(Product product) => _context.Products.Add(product);

        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p=> p.Id == id);
            if (product != null)
                _context.Products.Remove(product);
        }

        public IEnumerable<Product> GetAllProducts() =>_context.Products.ToList();

        public Product GetProductById(int id)=>_context.Products.FirstOrDefault(p => p.Id == id);

        public bool SaveChanges() => (_context.SaveChanges() > 0);
    }

}
