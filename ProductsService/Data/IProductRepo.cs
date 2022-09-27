using ProductsService.Model;

namespace ProductsService.Data
{
    public interface IProductRepo
    {

        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void CreateProduct(Product product);
        void DeleteProduct(int id);
        bool SaveChanges();


    }
}
