using AutoMapper;
using SadovodBack.Dtos;
using SadovodBack.Entities;

namespace SadovodBack.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}