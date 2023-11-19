using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net.Mime;
using System.Text;

namespace ProductsService.Model
{
    public class BolbService : IBolbService
    {
        private readonly ILogger<BolbService> _logger;
        private readonly BlobServiceClient _bolbServiceClient;

        public BolbService(ILogger<BolbService> logger, BlobServiceClient bolbServiceClient)
        {
            _logger = logger;
            _bolbServiceClient = bolbServiceClient;
        }

        private async Task<BinaryData> GetResourceByNameAsBinaryData(IBolbModelRequest bolbModelRequest)
        {

            var client = InitializeOrRetriveContainerClient(bolbModelRequest);
            var blobDownload = await client.DownloadContentAsync();
            return blobDownload.Value.Content;
        }

        public async Task<Stream> GetResourceByNameAsStream(IBolbModelRequest bolbModelRequest)
            => (await GetResourceByNameAsBinaryData(bolbModelRequest)).ToStream();

        public async Task<byte[]> GetResourceByNameAsByteArray(IBolbModelRequest bolbModelRequest) 
            => (await GetResourceByNameAsBinaryData(bolbModelRequest)).ToArray();






        public async Task<IEnumerable<string>> GetResourcesNameList(IBolbModelRequest bolbModelRequest)
        {
            var containerClient = _bolbServiceClient.GetBlobContainerClient(bolbModelRequest.ContainerName);
            var items = new List<string>();
            try
            {
                await foreach (var blobitem in containerClient.GetBlobsAsync())
                {
                    items.Add(blobitem.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("--> Could not read the resources", ex.Message, ex.StackTrace);
            }
            return items;
        }

        public async Task<bool> UploadResource(IBolbModelRequest bolbModelRequest)
        {
            var client = InitializeOrRetriveContainerClient(bolbModelRequest);
            var blobhttp = new BlobHttpHeaders() { ContentType = "application/octet-stream" };
            try
            {
                await client.UploadAsync(bolbModelRequest.ResourceStream, blobhttp);
            }
            catch (Exception ex)
            {
                _logger.LogError("--> Could not upload the resources", ex.Message, ex.StackTrace);
                return false;
            }
            _logger.LogInformation($"--> Resource {bolbModelRequest.ResourceName} uploaded on container {bolbModelRequest.ContainerName}");
            return true;
        }

        public async Task<bool> DeleteResource(IBolbModelRequest bolbModelRequest)
        {
            var client = InitializeOrRetriveContainerClient(bolbModelRequest);
            var isDelated= await client.DeleteIfExistsAsync();
            if (!isDelated)
            {
                _logger.LogError($"--> Resource {bolbModelRequest.ResourceName} not found on container {bolbModelRequest.ContainerName}");
                return false;
            }
            _logger.LogInformation($"--> Resource {bolbModelRequest.ResourceName} deleted from container {bolbModelRequest.ContainerName}");
            return true;
        }

               

      
        private BlobClient InitializeOrRetriveContainerClient(IBolbModelRequest bolbModelRequest)
        {
            var containerClient = _bolbServiceClient.GetBlobContainerClient(bolbModelRequest.ContainerName);
            var createedresponse =containerClient.CreateIfNotExists();
            if (createedresponse!=null)
            {
                _logger.LogInformation($"--> Created new container with {bolbModelRequest.ContainerName}");
            }
            return containerClient.GetBlobClient(bolbModelRequest.ResourceName);
        }



    }
}