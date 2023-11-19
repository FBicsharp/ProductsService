using Microsoft.EntityFrameworkCore;
using ProductsService.Model;

namespace ProductsService.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly ProductDbContext _context;

        public ProductRepo(ProductDbContext context)
        {
            _context = context;
        }
        #region ASYNC


        public async Task CreateProductAsync(Product product, CancellationToken cancellationToken)
            => await _context.Products.AddAsync(product, cancellationToken);


        public async Task DeleteProductAsync(Guid company, Guid id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.id == id && p.company == company);
            if (product != null)
                _context.Products.Remove(product);            
                //product.state = ProductState.Inactive;
        }

        public async Task UpdateProductAsync(Product updateProduct, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.id == updateProduct.id && p.company == updateProduct.company);
            if (product == null)
                return;
            product.description = updateProduct.description;
            product.unit_of_measure = updateProduct.unit_of_measure;
            product.price = updateProduct.price;
            product.currency = updateProduct.currency;
            product.last_update = DateTime.UtcNow;
            product.imageId = updateProduct.imageId;
            product.state= updateProduct.state;
            _context.Products.Update(product);            
        }


        public async Task<IEnumerable<Product>> GetAllProductsAsync(Guid company,CancellationToken cancellationToken)
            => await _context.Products.Where(c=>c.company==company).ToListAsync();


        public async Task<Product> GetProductByIdAsync(Guid company,Guid id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.id == id && p.company==company, cancellationToken);
            if (product != null)
                return product;

            return new Product();
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken) > 0;
        #endregion

        #region SYNCRONOUS


        public void CreateProduct(Product product)
            => CreateProductAsync(product, CancellationToken.None).GetAwaiter().GetResult();

        public void DeleteProduct(Guid company, Guid id)
            => DeleteProductAsync(company,id, CancellationToken.None).GetAwaiter().GetResult();

        public IEnumerable<Product> GetAllProducts(Guid company)
            => GetAllProductsAsync(company,CancellationToken.None).GetAwaiter().GetResult();


        public Product GetProductById(Guid company, Guid id)
            => GetProductByIdAsync(company,id, CancellationToken.None).GetAwaiter().GetResult();

        public bool SaveChanges()
            => SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
        public void UpdateProduct(Product updateProduct)
            => UpdateProductAsync(updateProduct, CancellationToken.None).GetAwaiter().GetResult();

        #endregion


    }

}
