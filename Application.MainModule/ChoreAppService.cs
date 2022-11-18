using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Core;

using Application.Dto.Postulant;
using Application.MainModule.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.MainModule.Entity;
using Domain.MainModule.IRepository;
using Domain.MainModule.Validations.PostulantValidations;
using Infrastructure.CrossCutting.Constants;
using Infrastructure.CrossCutting.CustomExections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Application.Dto.Chore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.MainModule;

public class ChoreAppService : BaseAppService, IChoreAppService
{
    private readonly IChoreRepository _postulantRepository;
    private readonly IConfiguration _configuration;

    public ChoreAppService(
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _postulantRepository = serviceProvider.GetService<IChoreRepository>() ?? throw new InvalidOperationException();
        _configuration = serviceProvider.GetService<IConfiguration>() ?? throw new InvalidOperationException();
    }

    public async Task<ChoreDto> GetById(int postulantId)
    {
        if (postulantId == 0)
            throw new WarningException(MessageConst.InvalidSelection);

        var postulantDto = await _postulantRepository
            .Find(p => p.Id == postulantId)
            .ProjectTo<ChoreDto>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (postulantDto is null)
            throw new WarningException(MessageConst.InvalidSelection);

        return postulantDto;
    }

    public async Task<string> Add(ChoreDto postulantDto)
    {
        var newPostulant = Mapper.Map<Chore>(postulantDto);
        await _postulantRepository.AddAsync(newPostulant, new AddChoreValidator(_postulantRepository));
        await UnitOfWork.SaveChangesAsync();

        return MessageConst.ProcessSuccessfullyCompleted;
    }

    public async Task<string> Update(UpdateChoreDto postulantDto)
    {
        var postulantDomain = await _postulantRepository.GetAsync(postulantDto.Id);

        if (postulantDomain is null)
            throw new WarningException(MessageConst.InvalidSelection);

        Mapper.Map(postulantDto, postulantDomain);

        await _postulantRepository.UpdateAsync(postulantDomain, new AddChoreValidator(_postulantRepository));
        await UnitOfWork.SaveChangesAsync();

        return MessageConst.ProcessSuccessfullyCompleted;
    }

    public async Task<string> Delete(int choreId)
    {
        var choreDomain = await _postulantRepository.GetAsync(choreId);

        if (choreDomain is null) throw new WarningException(MessageConst.InvalidSelection);

        await _postulantRepository.DeleteAsync(choreDomain);
        await UnitOfWork.SaveChangesAsync();

        return MessageConst.ProcessSuccessfullyCompleted;
    }
}