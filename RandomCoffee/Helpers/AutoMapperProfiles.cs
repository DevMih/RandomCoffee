using AutoMapper;
using RandomCoffee.DTOs;
using RandomCoffee.Entities;

namespace RandomCoffee.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photo.Url));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<AppUser, UserDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
        }
    }
}
