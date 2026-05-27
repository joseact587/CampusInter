using CampusInter.Api.Authorization;
using CampusInter.Application.DTOs.Estudiantes;
using CampusInter.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusInter.Api.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicies.Estudiante)]
[Route("api/estudiantes")]
public sealed class EstudiantesController : ControllerBase
{
    // Atributos
    private readonly IEstudianteService _estudianteService;

    // Constructores
    public EstudiantesController(IEstudianteService estudianteService)
    {
        _estudianteService = estudianteService;
    }

    // Endpoints
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EstudianteResumenResponse>>> ObtenerEstudiantes()
    {
        var response = await _estudianteService.ObtenerEstudiantesAsync();

        return Ok(response);
    }

    [HttpGet("me")]
    public async Task<ActionResult<MiPerfilResponse>> ObtenerMiPerfil()
    {
        var response = await _estudianteService.ObtenerMiPerfilAsync();

        return Ok(response);
    }

    [HttpPut("me")]
    public async Task<ActionResult<MiPerfilResponse>> ActualizarMiPerfil(
        [FromBody] ActualizarMiPerfilRequest request)
    {
        var response = await _estudianteService.ActualizarMiPerfilAsync(request);

        return Ok(response);
    }

    [HttpPatch("me/inhabilitar")]
    public async Task<IActionResult> InhabilitarMiPerfil()
    {
        await _estudianteService.InhabilitarMiPerfilAsync();

        return NoContent();
    }
    [HttpPatch("me/habilitar")]
    public async Task<IActionResult> HabilitarMiPerfil()
    {
        await _estudianteService.HabilitarMiPerfilAsync();

        return NoContent();
    }

}
