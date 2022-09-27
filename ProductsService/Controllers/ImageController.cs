using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Model;

namespace ProductsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesController : ControllerBase
    {
      

        private readonly ILogger<ResourcesController> _logger;
        private readonly IBolbService _blobService;


        public ResourcesController(ILogger<ResourcesController> logger, IBolbService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        [HttpGet(Name = nameof(GetAllResources))]
        public async Task<IActionResult> GetAllResources()
        {
            var request = new BolbModelRequest() { ContainerName = "test", ResourceName = "" };
            return Ok(await _blobService.GetResourcesNameList(request));            
        }



        [HttpGet("Stream/{ResourceName}", Name = nameof(GetResourceStreamByName))]
        public async Task<Stream> GetResourceStreamByName(string ResourceName)
        {
            var request = new BolbModelRequest() { ContainerName = "test", ResourceName = ResourceName };
            var strm = await _blobService.GetResourceByName(request);
            return strm;
           
        }

        [HttpGet("View/{ResourceName}", Name = nameof(GetResourceViewByName))]
        public async Task<IActionResult> GetResourceViewByName(string ResourceName)
        {
            var request = new BolbModelRequest() { ContainerName = "test", ResourceName = ResourceName };
            var strm = await _blobService.GetResourceByName(request);
            return File(strm, "image/jpeg");

        }


        [HttpPost(Name = nameof(UploadResourceOnContainer))]
        public async Task<IActionResult> UploadResourceOnContainer(IFormFile resource,string? newname)
        {
            
            if ( resource== null)
            {
                return BadRequest();
            }
            var bolbModelRequest = new BolbModelRequest()
            {
                ContainerName = "test",
                ResourceName = string.IsNullOrEmpty(newname) ? resource.FileName : newname,
            };
            bolbModelRequest.ResourceStream = resource.OpenReadStream();
            await _blobService.UploadResource(bolbModelRequest);            
            return Ok();
        }

        [HttpDelete(Name = nameof(DeleteResourceByName))]
        public async Task<IActionResult> DeleteResourceByName(string newname)
        {
            var bolbModelRequest = new BolbModelRequest()
            {
                ContainerName = "test",
                ResourceName = newname
            };
            await _blobService.DeleteResource(bolbModelRequest);
            return NoContent();
        }
    }
}