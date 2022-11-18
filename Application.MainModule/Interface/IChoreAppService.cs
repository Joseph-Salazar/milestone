using Application.Dto.Chore;
using Application.Dto.Postulant;
using Domain.MainModule.Entity;

namespace Application.MainModule.Interface;

public interface IChoreAppService
{
    Task<ChoreDto> GetById(int choreId);
    Task<string> Add(ChoreDto choreDto);
    Task<string> Update(UpdateChoreDto updateChoreDto);
    Task<string> Delete(int choreId);
}