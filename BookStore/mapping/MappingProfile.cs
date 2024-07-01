using AutoMapper;
using BookStore.Models;
using BookStore.ViewModel;

namespace BookStore.mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryVM>().ReverseMap();
        }
    }
}
