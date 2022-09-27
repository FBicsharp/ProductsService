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
        private  BolbModelRequest _request;



        public ResourcesController(ILogger<ResourcesController> logger, IBolbService blobService, BolbModelRequest request)
        {
            _logger = logger;
            _blobService = blobService;
            _request = request;
        }

        [HttpGet(Name = nameof(GetAllResources))]
        public async Task<IActionResult> GetAllResources()
        {   
            return Ok(await _blobService.GetResourcesNameList(_request));            
        }



        [HttpGet("Stream/{ResourceName}", Name = nameof(GetResourceStreamByName))]
        public async Task<Stream> GetResourceStreamByName(string ResourceName)
        {
            _request.ResourceName = ResourceName;
            var strm = await _blobService.GetResourceByName(_request);
            return strm;
           
        }

        [HttpGet("View/{ResourceName}", Name = nameof(GetResourceViewByName))]
        public async Task<IActionResult> GetResourceViewByName(string ResourceName)
        {
            _request.ResourceName = ResourceName;
            var strm = await _blobService.GetResourceByName(_request);
            return File(strm, "image/jpeg");

        }


        [HttpPost(Name = nameof(UploadResourceOnContainer))]
        public async Task<IActionResult> UploadResourceOnContainer(IFormFile resource,string? newname)
        {
            
            if ( resource== null)
                return BadRequest();
            
            if (!string.IsNullOrEmpty(newname))
                _request.ResourceName =  newname+ Path.GetExtension(resource.FileName);            
            
            _request.ResourceStream = resource.OpenReadStream();
            await _blobService.UploadResource(_request);            
            return CreatedAtRoute(nameof(UploadResourceOnContainer), new { resourceName= _request.ResourceName });
        }

        [HttpDelete(Name = nameof(DeleteResourceByName))]
        public async Task<IActionResult> DeleteResourceByName(string newname)
        {
            if (string.IsNullOrEmpty(newname))
            {
                return BadRequest();
            }
            _request.ResourceName = newname;
            await _blobService.DeleteResource(_request);
            return NoContent();
        }
    }
}