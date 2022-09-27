using Azure.Storage.Blobs.Models;

namespace ProductsService.Model
{
    public interface IBolbService
    {

        Task<Stream> GetResourceByName(BolbModelRequest bolbModelRequest);
        Task<IEnumerable<string>> GetResourcesNameList(BolbModelRequest bolbModelRequest);
        Task UploadResource(BolbModelRequest bolbModelRequest);
        Task DeleteResource(BolbModelRequest bolbModelRequest);



    }
}