using Application.Dto.Chore;
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
public class ChoreController
{
    private readonly IChoreAppService _postulantAppService;

    public ChoreController(IChoreAppService postulantAppService)
    {
        _postulantAppService = postulantAppService;
    }

    /// <summary>
    /// Permite obtener una tarea por id
    /// </summary>
    [AllowAnonymous]
    [HttpGet("Get")]
    [ProducesResponseType(typeof(JsonResult<ChoreDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int postulantId)
    {
        var result = await _postulantAppService.GetById(postulantId);
        return new OkObjectResult(new JsonResult<ChoreDto>(result));
    }

    /// <summary>
    /// Permite agregar una tarea
    /// </summary>
    [HttpPost(nameof(Add))]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] ChoreDto postulantDto)
    {
        var response = await _postulantAppService.Add(postulantDto);
        return new OkObjectResult(new JsonResult<string>(response));
    }

    /// <summary>
    /// Permite actualizar una tarea
    /// </summary>
    [HttpPut(nameof(Update))]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateChoreDto updatePostulantDto)
    {
        var response = await _postulantAppService.Update(updatePostulantDto);
        return new OkObjectResult(new JsonResult<string>(response));
    }

    /// <summary>
    /// Permite borrar una tarea
    /// </summary>
    [HttpDelete(nameof(Delete))]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JsonResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] int choreId)
    {
        var response = await _postulantAppService.Delete(choreId);
        return new OkObjectResult(new JsonResult<string>(response));
    }
}