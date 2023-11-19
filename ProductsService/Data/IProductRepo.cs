using ProductsService.Model;

namespace ProductsService.Data
{
    public interface IProductRepo
    {
        /// <summary>
        /// Retrieves all products in the system.
        /// </summary>
        /// <returns>An IEnumerable of Product objects representing all products.</returns>
        IEnumerable<Product> GetAllProducts(Guid company);

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>A Product object representing the product with the specified ID.</returns>
        Product GetProductById(Guid company, Guid id);

        /// <summary>
        /// Creates a new product in the system.
        /// </summary>
        /// <param name="product">The Product object to be created.</param>
        void CreateProduct(Product product);

        /// <summary>
        /// Deletes a product from the system based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to be deleted.</param>
        void DeleteProduct(Guid company, Guid id);

        void UpdateProduct(Product updateProduct);

        /// <summary>
        /// Saves changes made to the products in the system.
        /// </summary>
        /// <returns>True if changes were successfully saved; otherwise, false.</returns>
        bool SaveChanges();

        /// <summary>
        /// Asynchronously retrieves all products in the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is an IEnumerable of Product objects.</returns>
        Task<IEnumerable<Product>> GetAllProductsAsync(Guid company,CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is a Product object.</returns>
        Task<Product> GetProductByIdAsync(Guid company, Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously creates a new product in the system.
        /// </summary>
        /// <param name="product">The Product object to be created.</param>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        Task CreateProductAsync(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously deletes a product from the system based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to be deleted.</param>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        Task DeleteProductAsync(Guid company, Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously saves changes made to the products in the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is true if changes were successfully saved; otherwise, false.</returns>
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);


        Task UpdateProductAsync(Product updateProduct, CancellationToken cancellationToken);

    }
}
