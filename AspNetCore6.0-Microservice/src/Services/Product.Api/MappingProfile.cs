using AutoMapper;
using Infrastructure.Mappings;
using Product.Api.Entities;
using Shared.DTOs.Product;

namespace Product.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<CreateProductDto, CatalogProduct>().ReverseMap();
            CreateMap<UpdateProductDto, CatalogProduct>().ReverseMap()
                .IgnoreAllNonExisting();
        }
    }
}
