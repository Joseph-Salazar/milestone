using Application.Dto.Postulant;
using Application.MainModule;
using Application.MainModule.Interface;
using Domain.MainModule.Entity;
using Hangfire.MemoryStorage.Dto;
using Infrastructure.CrossCutting.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Distributed.Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostulantController
{
    private readonly IPostulantAppService _postulantAppService;

    public PostulantController(IPostulantAppService postulantAppService)
    {
        _postulantAppService = postulantAppService;
    }

    /// <summary>
    /// Permite registrar un usuario
    /// </summary>
    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(typeof(JsonResult<PostulantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser(RegisterPostulantDto register)
    {
        var result = await _postulantAppService.Register(register);
        return new OkObjectResult(new JsonResult<PostulantDto>(result));
    }

    /// <summary>
    /// Permite a un usuario ingresar al sistema
    /// </summary>
    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(typeof(JsonResult<LoginPostulantResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUser(LoginPostulantDto loginCompany)
    {
        var result = await _postulantAppService.Login(loginCompany);
        return new OkObjectResult(new JsonResult<LoginPostulantResponseDto>(result));
    }

    /// <summary>
    /// Permite obtener un usuario por id
    /// </summary>
    [AllowAnonymous]
    [HttpGet("Get")]
    [ProducesResponseType(typeof(JsonResult<PostulantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int postulantId)
    {
        var result = await _postulantAppService.GetById(postulantId);
        return new OkObjectResult(new JsonResult<PostulantDto>(result));
    }

    /// <summary>
    /// Permite actualizar un usuario
    /// </summary>
    [HttpPut(nameof(Update))]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdatePostulantDto updatePostulantDto)
    {
        var response = await _postulantAppService.Update(updatePostulantDto);
        return new OkObjectResult(new JsonResult<string>(response));
    }
}