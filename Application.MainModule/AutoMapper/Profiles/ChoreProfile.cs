using Application.Dto.Chore;
using Application.Dto.Postulant;
using AutoMapper;
using Domain.MainModule.Entity;

namespace Application.MainModule.AutoMapper.Profiles;

public class ChoreProfile : Profile
{
    public ChoreProfile()
    {
        CreateMap<Chore, ChoreDto>().ReverseMap();
        CreateMap<Chore, UpdateChoreDto>().ReverseMap();
    }
}