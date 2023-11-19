

namespace ProducctsService.Test
{
    public class ProductRepoTests
    {

        private IServiceProvider _serviceProvider;
        private List<Product> _productsMock;

        public ProductRepoTests()
        {

            //creo Iservice colelction
            var _services = new ServiceCollection();
            _services.AddDbContext<ProductDbContext>(opt => opt.UseInMemoryDatabase("InMemDb"));
            _services.AddSingleton(s => new CompanySettings()
            {
                CompanyId = Guid.NewGuid(),
                Container = ">MyBlob"
            });
            _services.AddScoped<IProductRepo, ProductRepo>();

            _serviceProvider = _services.BuildServiceProvider();
            var companySettings = _serviceProvider.GetRequiredService<CompanySettings>();
            Product product1 = new Product
            {
                company = companySettings.CompanyId,
                part_number = "ABC123",
                description = "Prodotto di esempio",
                unit_of_measure = "Piece",
                price = 19.99m,
                currency = "USD",
                creation_date = DateTime.Now,
                last_update = DateTime.Now,
                imageId = "image1.jpg"
            };

            // Inizializzazione del secondo oggetto
            Product product2 = new Product
            {
                company = companySettings.CompanyId,
                part_number = "XYZ789",
                description = "Altro prodotto di esempio",
                unit_of_measure = "Set",
                price = 49.99m,
                currency = "EUR",
                creation_date = DateTime.Now,
                last_update = DateTime.Now,
                imageId = "image2.jpg"
            };
            Product product3 = new Product
            {
                company = Guid.NewGuid(),
                part_number = "XYZ789",
                description = "Altro prodotto di esempio",
                unit_of_measure = "Set",
                price = 49.99m,
                currency = "EUR",
                creation_date = DateTime.Now,
                last_update = DateTime.Now,
                imageId = "image2.jpg"
            };
            _productsMock = new List<Product>();
            _productsMock.Add(product1);
            _productsMock.Add(product2);
            _productsMock.Add(product3);





        }
        [Fact]
        public void GetAllProducts_ShouldAddAndReturnAllProductsByCompany()
        {
            // Arrange
            var Repo = _serviceProvider.GetRequiredService<IProductRepo>();
            var company= _serviceProvider.GetRequiredService<CompanySettings>().CompanyId;
            foreach (var product in _productsMock)
            {
                Repo.CreateProduct(product);
            }
            Repo.SaveChanges();

            // Act
            var products = Repo.GetAllProducts(company).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(_productsMock.Where(c=>c.company==company).Count(), products.Count());
            Assert.All(_productsMock.Where(c => c.company == company), expectedProduct =>
                Assert.Contains(products, actualProduct => expectedProduct.id == actualProduct.id)
            );

        }

        [Fact]
        public void GetProductById_ShouldAddAndReturnCorrectProduct()
        {
            // Arrange
            var Repo = _serviceProvider.GetRequiredService<IProductRepo>();
            var company = _serviceProvider.GetRequiredService<CompanySettings>().CompanyId;
            foreach (var product in _productsMock)
            {
                Repo.CreateProduct(product);
            }
            Repo.SaveChanges();

            // Act
            var products = Repo.GetProductById(company, _productsMock[0].id);

            // Assert
            Assert.NotNull(products);
            Assert.Equal(products.id , _productsMock[0].id);
            
        }


        [Fact]
        public void DeleteProduct_ShouldAddAndDeleteCorrectProduct()
        {
            // Arrange
            var Repo = _serviceProvider.GetRequiredService<IProductRepo>();
            var company = _serviceProvider.GetRequiredService<CompanySettings>().CompanyId;
            foreach (var productitem in _productsMock)
            {
                Repo.CreateProduct(productitem);
            }
            Repo.SaveChanges();

            // Act
            Repo.DeleteProduct(company, _productsMock[0].id);
            Repo.SaveChanges();
            var product = Repo.GetProductById(company, _productsMock[0].id);            


            // Assert
            Assert.NotNull(product);
            Assert.Equal(product.part_number, string.Empty);
            Assert.Equal(product.company, Guid.Empty);
        }





    }

}