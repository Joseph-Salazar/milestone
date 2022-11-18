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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.MainModule;

public class PostulantAppService : BaseAppService, IPostulantAppService
{
    private readonly IPostulantRepository _postulantRepository;
    private readonly IConfiguration _configuration;

    public PostulantAppService(
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _postulantRepository = serviceProvider.GetService<IPostulantRepository>() ?? throw new InvalidOperationException();
        _configuration = serviceProvider.GetService<IConfiguration>() ?? throw new InvalidOperationException();
    }

    public async Task<PostulantDto> GetById(int postulantId)
    {
        if (postulantId == 0)
            throw new WarningException(MessageConst.InvalidSelection);

        var postulantDto = await _postulantRepository
            .Find(p => p.Id == postulantId)
            .ProjectTo<PostulantDto>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (postulantDto is null)
            throw new WarningException(MessageConst.InvalidSelection);

        return postulantDto;
    }

    public async Task<string> Update(UpdatePostulantDto postulantDto)
    {
        var postulantDomain = await _postulantRepository.GetAsync(postulantDto.Id);

        if (postulantDomain is null)
            throw new WarningException(MessageConst.InvalidSelection);

        Mapper.Map(postulantDto, postulantDomain);

        await _postulantRepository.UpdateAsync(postulantDomain, new AddPostulantValidator(_postulantRepository));
        await UnitOfWork.SaveChangesAsync();

        return MessageConst.ProcessSuccessfullyCompleted;
    }

    public async Task<PostulantDto> Register(RegisterPostulantDto registerPostulantDto)
    {
        Postulant postulantToDomain = new Postulant();

        if (_postulantRepository.GetAll().Any(x => x.Email == registerPostulantDto.Email))
        {
            throw new WarningException(MessageConst.EmailAlreadyExists);
        }

        CreatePasswordHash(registerPostulantDto.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

        var postulantRegister = new PostulantDto
        {
            Email = registerPostulantDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };


        postulantToDomain = Mapper.Map<PostulantDto, Postulant>(postulantRegister);

        postulantToDomain.FirstName = "";
        postulantToDomain.LastName = "";

        await _postulantRepository.AddAsync(postulantToDomain);
        await UnitOfWork.SaveChangesAsync();

        return postulantRegister;
    }

    public async Task<LoginPostulantResponseDto> Login(LoginPostulantDto registerCompanyDto)
    {
        var loginCompany =
            await _postulantRepository.GetAll().FirstOrDefaultAsync(u => u.Email == registerCompanyDto.Email);

        if (loginCompany is null)
        {
            throw new WarningException(MessageConst.InvalidLogin);
        }

        if (!VerifyPasswordHash(registerCompanyDto.Password, loginCompany.PasswordHash, loginCompany.PasswordSalt))
        {
            throw new WarningException(MessageConst.InvalidLogin);
        }

        LoginPostulantResponseDto response = new LoginPostulantResponseDto();

        string token = CreateToken(loginCompany);

        response.Postulant = Mapper.Map<PostulantDto>(loginCompany);
        response.Token = token;

        return response;
    }

    private string CreateToken(Postulant postulant)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, postulant.Email)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
            );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash
                (System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash
                (System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}