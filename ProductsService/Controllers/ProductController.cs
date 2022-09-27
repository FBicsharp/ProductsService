using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Data;
using ProductsService.Dtos;
using ProductsService.Model;

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
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var Products = _repository.GetAllProducts();
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(Products));
        }


        [HttpGet("{id}", Name = nameof(GetProductById))]
        public ActionResult<ProductReadDto> GetProductById(int id)
        {
            var Product = _repository.GetProductById(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductReadDto>(Product));
        }


        [HttpPost(Name = nameof(CreateProducts))]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> CreateProducts(ProductCreateDto newProduct)
        {
            var Product = _mapper.Map<Product>(newProduct);

            _repository.CreateProduct(Product);
            _repository.SaveChanges();
            var ProductCreated = _mapper.Map<ProductReadDto>(Product);
                        
            try//asyncmethod
            {
                var ProductPublish = _mapper.Map<ProductPublishDto>(Product);
                //ProductPublish.Event = "ProductPublished";
                //_messageBusClient.PublishNewProduct(ProductPublish);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Could not send asynchronously" + ex.Message);
            }


            return CreatedAtRoute(nameof(CreateProducts), new { Id = ProductCreated.Id }, ProductCreated);
        }



    }
}