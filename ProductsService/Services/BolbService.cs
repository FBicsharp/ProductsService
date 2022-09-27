using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net.Mime;
using System.Text;

namespace ProductsService.Model
{
    public class BolbService : IBolbService
    {
        private readonly BlobServiceClient _bolbServiceClient;

        public BolbService(BlobServiceClient bolbServiceClient)
        {
            this._bolbServiceClient = bolbServiceClient;
        }



        public async Task<Stream> GetResourceByName(BolbModelRequest bolbModelRequest)
        {

            var client = InitializeContainerClient(bolbModelRequest);
            var blobDownload = await client.DownloadContentAsync();
            return blobDownload.Value.Content.ToStream();
        }

        public async Task<IEnumerable<string>> GetResourcesNameList(BolbModelRequest bolbModelRequest)
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
                Console.WriteLine("--> Could not read the resources", ex.Message, ex.StackTrace);
            }
            return items;
        }

        public async Task UploadResource(BolbModelRequest bolbModelRequest)
        {
            var client = InitializeContainerClient(bolbModelRequest);
            var blobhttp = new BlobHttpHeaders() { ContentType = "application/octet-stream" };
            try
            {
                await client.UploadAsync(bolbModelRequest.ResourceStream, blobhttp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("--> Could not upload the resources", ex.Message, ex.StackTrace);
            }
        }

        public async Task DeleteResource(BolbModelRequest bolbModelRequest)
        {
            var client = InitializeContainerClient(bolbModelRequest);
            var blobDownloadInfo = await client.DeleteIfExistsAsync();
        }
        

        private BlobClient InitializeContainerClient(BolbModelRequest bolbModelRequest)
        {
            var containerClient = _bolbServiceClient.GetBlobContainerClient(bolbModelRequest.ContainerName);
            return containerClient.GetBlobClient(bolbModelRequest.ResourceName);
        }



    }
}