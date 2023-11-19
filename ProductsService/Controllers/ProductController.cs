using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Data;
using ProductsService.Dtos;
using ProductsService.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProductsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
      

        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepo _repository;
        public readonly IMapper _mapper;


        public ProductController(ILogger<ProductController> logger,
            IProductRepo productRepo,
            IMapper mapper)
        {
            _logger = logger;
            _repository = productRepo;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAllProducts))]
        public async  Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProducts(Guid company, CancellationToken cancellationToken)
        {
            var Products = await _repository.GetAllProductsAsync(company, cancellationToken);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(Products));
        }


        [HttpGet("{company}/{id}", Name = nameof(GetProductById))]
        public async Task<ActionResult<ProductReadDto>> GetProductById(Guid company,Guid id, CancellationToken cancellationToken)
        {
            var Product = await _repository.GetProductByIdAsync(company, id, cancellationToken);
            if (Product == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductReadDto>(Product));
        }

        [HttpPost("{newProduct}", Name = nameof(UpdateProduct))]
        public async Task<ActionResult<ProductReadDto>> UpdateProduct(ProductUpdateDto updatedProduct, CancellationToken cancellationToken)
        {
            var updatedProducttmp = _mapper.Map<Product>(updatedProduct);
            await _repository.UpdateProductAsync(updatedProducttmp, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            var Product = await _repository.GetProductByIdAsync(updatedProducttmp.company, updatedProducttmp.id, cancellationToken);
            if (Product == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductReadDto>(Product));
        }



        [HttpPost(Name = nameof(CreateProducts))]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> CreateProducts(ProductCreateDto newProduct,CancellationToken cancellationToken)
        {
            var Product = _mapper.Map<Product>(newProduct);

            await _repository.CreateProductAsync(Product, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            var ProductCreated = _mapper.Map<ProductReadDto>(Product);
                        
            try//asyncmethod
            {
                var ProductPublish = _mapper.Map<ProductNotifiyDto>(Product);
                ProductPublish.EventType = ProductEventType.Created_Event;                
                //_messageBusClient.PublishNewProduct(ProductPublish);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Could not send asynchronously" + ex.Message);
            }
            return CreatedAtRoute(nameof(CreateProducts), new { Id = ProductCreated.id }, ProductCreated);
        }



    }
}