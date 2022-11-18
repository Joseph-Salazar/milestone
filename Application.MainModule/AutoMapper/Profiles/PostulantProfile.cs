using Application.Dto.Postulant;
using AutoMapper;
using Domain.MainModule.Entity;

namespace Application.MainModule.AutoMapper.Profiles;

public class PostulantProfile : Profile
{
    public PostulantProfile()
    {
        CreateMap<Postulant, PostulantDto>().ReverseMap();
        CreateMap<LoginPostulantDto, PostulantDto>().ReverseMap();
        CreateMap<UpdatePostulantDto, Postulant>().ReverseMap();
    }
}