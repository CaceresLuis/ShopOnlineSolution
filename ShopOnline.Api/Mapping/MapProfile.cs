using AutoMapper;
using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
