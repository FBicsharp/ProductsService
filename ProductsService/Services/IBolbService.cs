using Azure.Storage.Blobs.Models;

namespace ProductsService.Model
{
    public interface IBolbService
    {

        Task<Stream> GetResourceByNameAsStream(IBolbModelRequest bolbModelRequest);
        Task<byte[]> GetResourceByNameAsByteArray(IBolbModelRequest bolbModelRequest);
        Task<IEnumerable<string>> GetResourcesNameList(IBolbModelRequest bolbModelRequest);
        Task<bool> UploadResource(IBolbModelRequest bolbModelRequest);
        Task<bool> DeleteResource(IBolbModelRequest bolbModelRequest);


    }
}