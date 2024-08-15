using AutoMapper;
using UrlShortener.Models;
using UrlShortener.Models.Data;
using UrlShortener.Models.ResponseModels;

namespace UrlShortener.Mappers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserModel>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<RefreshToken, RefreshTokenModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<TokenResponse, TokenResponseModel>().ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));
        }
    }
}
