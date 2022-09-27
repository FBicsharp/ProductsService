using AutoMapper;
using ProductsService.Dtos;
using ProductsService.Model;

namespace ProductsService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductPublishDto>();
            
        }
    }
}
