using Application.Dto.Postulant;
using Domain.MainModule.Entity;

namespace Application.MainModule.Interface;

public interface IPostulantAppService
{
    Task<PostulantDto> GetById(int postulantId);
    Task<string> Update(UpdatePostulantDto updatePostulantDto);
    Task<PostulantDto> Register(RegisterPostulantDto register);
    Task<LoginPostulantResponseDto> Login(LoginPostulantDto login);
}