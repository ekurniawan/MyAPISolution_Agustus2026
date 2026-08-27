using AutoMapper;
using MyAPISolution.SampleAPI.DTO;
using MyAPISolution.SampleAPI.Models;

namespace MyAPISolution.SampleAPI.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryInsertDTO, Category>();
            CreateMap<CategoryEditDTO, Category>();
            CreateMap<Product, ProductDTO>();
        }
    }
}
