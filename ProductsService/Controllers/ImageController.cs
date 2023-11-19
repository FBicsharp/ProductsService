using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Data;
using ProductsService.Model;

namespace ProductsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesController : ControllerBase
    {
      

        private readonly ILogger<ResourcesController> _logger;
        private readonly CompanySettings _companySettings;
        private readonly IBolbService _blobService;
        private  IBolbModelRequest _request;
        private readonly IImagesRepo _imagesRepo;

        public ResourcesController(ILogger<ResourcesController> logger, CompanySettings companySettings, IBolbService blobService, IBolbModelRequest request,IImagesRepo imagesRepo)
        {
            _logger = logger;
            _companySettings = companySettings;
            _blobService = blobService;
            _request = request;
            _imagesRepo = imagesRepo;
        }

        [HttpGet(Name = nameof(GetAllResources))]
        public async Task<IActionResult> GetAllResources(CancellationToken cancellationToken)
        {   
            var images = await _imagesRepo.GetAllImagesAsync(_companySettings.CompanyId, cancellationToken);
            return Ok(images);
            //not returning only the resource name from blob  becouse the main reposotory is the image repo
//            return Ok(await _blobService.GetResourcesNameList(_request));            
        }



        [HttpGet("Stream/{id}/{ResourceName}", Name = nameof(GetResourceStreamByName))]
        public async Task<Stream> GetResourceStreamByName(Guid id, CancellationToken cancellationToken)
        {
            var image = await _imagesRepo.GetImageByIdAsync(_companySettings.CompanyId,id, cancellationToken);
            _request.ResourceName = image.external_storege_id;
            var strm = await _blobService.GetResourceByNameAsStream(_request);
            return strm;
           
        }

        [HttpGet("View/{id}/{ResourceName}", Name = nameof(GetResourceViewByName))]
        public async Task<IActionResult> GetResourceViewByName(Guid id, CancellationToken cancellationToken)
        {
            var image = await _imagesRepo.GetImageByIdAsync(_companySettings.CompanyId, id, cancellationToken);
            if (image is null || string.IsNullOrEmpty(image.external_storege_id) )
            {
                return NotFound();
            }
            _request.ResourceName = image.external_storege_id;
            var strm = await _blobService.GetResourceByNameAsStream(_request);
            return File(strm, $"\"image/{image.image_extension}\""); //"image/jpeg"

        }


        [HttpPost(Name = nameof(UploadResourceOnContainer))]
        public async Task<IActionResult> UploadResourceOnContainer(IFormFile resource,string? newname, CancellationToken cancellationToken)
        {
            
            if ( resource== null)
                return BadRequest();
            newname = string.IsNullOrEmpty(newname) ? resource.FileName : newname + Path.GetExtension(resource.FileName);

            #region Database


            var images = await _imagesRepo.GetAllImagesAsync(_companySettings.CompanyId, cancellationToken);
            var imegeExists =images.FirstOrDefault(x => x.name == newname);
            if (imegeExists is not null)
                return BadRequest("Image allready exists, change name ");
            var image = new Image();
            image.company = _companySettings.CompanyId;
            image.name = newname;
            image.description = "Uploaded from API";
            image.creation_date = DateTime.UtcNow;
            image.last_update = DateTime.UtcNow;
            image.external_storege_id = newname;
            image.image_extension = Path.GetExtension(resource.FileName).Replace(".", "");
            image.image_content = new byte[resource.OpenReadStream().Length];
            await resource.OpenReadStream().ReadAsync(image.image_content, cancellationToken);
            await _imagesRepo.CreateImageAsync(image, cancellationToken);
            var isSaved =await _imagesRepo.SaveChangesAsync(cancellationToken);
            #endregion
            if (!isSaved)
                return BadRequest("Error saving image");

            #region BlobStorage

            _request.ResourceName = image.external_storege_id;                        
            _request.ResourceStream = resource.OpenReadStream();

            var isUploadedOnBlobStorege =await _blobService.UploadResource(_request);
            #endregion
            if (!isUploadedOnBlobStorege)
            {
                await _imagesRepo.DeleteImageAsync(_companySettings.CompanyId, image.id, cancellationToken);
                return BadRequest("Error uploading image on blob storege");
            }

            return CreatedAtRoute(nameof(UploadResourceOnContainer), new { resourceName= _request.ResourceName });
        }

        [HttpDelete(Name = nameof(DeleteResourceByName))]
        public async Task<IActionResult> DeleteResourceByName(Guid id, CancellationToken cancellationToken)
        {
            var images = await _imagesRepo.GetImageByIdAsync(_companySettings.CompanyId,id, CancellationToken.None);
            if (images is null)
                return BadRequest();
            await _imagesRepo.DeleteImageAsync(_companySettings.CompanyId, id, cancellationToken);
            var isSaved = await _imagesRepo.SaveChangesAsync(cancellationToken);
            if (!isSaved)
                return BadRequest("Error deleting image");
            
            _request.ResourceName = images.external_storege_id;
            await _blobService.DeleteResource(_request);

            return NoContent();
        }
    }
}