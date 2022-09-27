using Azure.Storage.Blobs;
using ProductsService.Model;

namespace ProductsService.ServiceExtension
{
    public static class ConfigureProductsDI
    {
       

        public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration config)
        {
            ConfigureEnviroment(config);

            services.AddSingleton(s=> new CompanySettings() { 
                CompanyId= Guid.Parse(Environment.GetEnvironmentVariable("CompanyId", EnvironmentVariableTarget.User)),
                Container= Environment.GetEnvironmentVariable("bolbName", EnvironmentVariableTarget.User)
            });
            services.AddSingleton(s => new BlobServiceClient(Environment.GetEnvironmentVariable("AzureBlobStorageConnectionString", EnvironmentVariableTarget.User)));
            services.AddScoped<IBolbService, BolbService>();
            services.AddTransient<BolbModelRequest>();
            


            return services;
        }


        public static void ConfigureEnviroment(IConfiguration config)
        {
            EnviromentVariableCreateIfNotExists("AzureBlobStorageConnectionString", config["AzureBlobStorageConnectionString"], EnvironmentVariableTarget.User);
            EnviromentVariableCreateIfNotExists("CompanyId", config["AzureBlobStorageContainer:CompanyId"], EnvironmentVariableTarget.User);
            EnviromentVariableCreateIfNotExists("bolbName", config["AzureBlobStorageContainer:Name"], EnvironmentVariableTarget.User);

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
