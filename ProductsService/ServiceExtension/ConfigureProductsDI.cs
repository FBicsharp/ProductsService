using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Model;

namespace ProductsService.ServiceExtension
{
    public static class ConfigureProductsDI
    {
       

        public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration config)
        {
            ConfigureAndValidateEnviromentFroimConfiguration(config);

            services.AddSingleton(s=> new CompanySettings() { 
                CompanyId= Guid.Parse(Environment.GetEnvironmentVariable("CompanyId", EnvironmentVariableTarget.User)),
                Container= Environment.GetEnvironmentVariable("bolbName", EnvironmentVariableTarget.User)
            });
            services.AddSingleton(s => new BlobServiceClient(Environment.GetEnvironmentVariable("AzureBlobStorageConnectionString", EnvironmentVariableTarget.User)));
            services.AddScoped<IBolbService, BolbService>();
            services.AddTransient<IBolbModelRequest,BolbModelRequest>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#if DEBUG
            services.AddDbContext<ProductDbContext>(opt => opt.UseInMemoryDatabase("InMemDb"));
                Console.WriteLine("--> Using InMemoryDb");

#else
                services.AddDbContext<ProductDbContext>(opt =>
                    opt.UseSqlServer(Environment.GetEnvironmentVariable("ProductDbConnection", EnvironmentVariableTarget.User)));
                Console.WriteLine("--> Using UseSqlServerDb");
#endif

    
            return services;
        }


        public static void ConfigureAndValidateEnviromentFroimConfiguration(IConfiguration config)
        {
            //validate configuration
            var dbConnection = config.GetConnectionString("ProductConnection");
            if (string.IsNullOrEmpty(dbConnection))
                throw new Exception("ProductDbConnection is not set");

            var azureBlobStorageConnectionString = config["AzureBlobStorageConnectionString"];
            if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                throw new Exception("AzureBlobStorageConnectionString is not set");

            var companyId = config["AzureBlobStorageContainer:CompanyId"];
            if (string.IsNullOrEmpty(companyId))
                throw new Exception("AzureBlobStorageContainer:CompanyId is not set");            

            var bolbName = config["AzureBlobStorageContainer:Name"];
            if (string.IsNullOrEmpty(bolbName))
                throw new Exception("AzureBlobStorageContainer:Name is not set");


            EnviromentVariableCreateIfNotExists("ProductDbConnection", dbConnection, EnvironmentVariableTarget.User);
            EnviromentVariableCreateIfNotExists("AzureBlobStorageConnectionString", azureBlobStorageConnectionString, EnvironmentVariableTarget.User);
            EnviromentVariableCreateIfNotExists("CompanyId", companyId, EnvironmentVariableTarget.User);
            EnviromentVariableCreateIfNotExists("bolbName", bolbName, EnvironmentVariableTarget.User);
            
        }

        
        public static void EnviromentVariableCreateIfNotExists(string key,string value, EnvironmentVariableTarget target)
        {

            var variablevalue = Environment.GetEnvironmentVariable(key, target);
            if (string.IsNullOrEmpty(variablevalue) && !string.IsNullOrEmpty(value))
            {                
                Environment.SetEnvironmentVariable(key, value, target);
            }

        }
    }
}
