using AutoMapper;
using ProductService.Core.Domain;
using ProductService.Core.Application.Products.Dtos;

namespace ProductService.Core.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}