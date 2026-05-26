using CampusInter.Application.DTOs.Inscripciones;
using CampusInter.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusInter.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/inscripciones")]
public sealed class InscripcionesController : ControllerBase
{
    // Atributos
    private readonly IInscripcionService _inscripcionService;

    // Constructores
    public InscripcionesController(IInscripcionService inscripcionService)
    {
        _inscripcionService = inscripcionService;
    }

    // Endpoints
    [HttpPost]
    public async Task<ActionResult<InscripcionResponse>> CrearInscripcion(
        [FromBody] CrearInscripcionRequest request)
    {
        var response = await _inscripcionService.CrearInscripcionAsync(request);

        return Ok(response);
    }

    [HttpGet("mi-inscripcion")]
    public async Task<ActionResult<InscripcionResponse>> ObtenerMiInscripcion()
    {
        var response = await _inscripcionService.ObtenerMiInscripcionAsync();

        return Ok(response);
    }
}
