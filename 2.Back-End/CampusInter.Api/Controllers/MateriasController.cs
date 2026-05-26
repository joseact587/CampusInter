using CampusInter.Application.DTOs.Materias;
using CampusInter.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusInter.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/materias")]
public sealed class MateriasController : ControllerBase
{
    // Atributos
    private readonly IMateriaService _materiaService;

    // Constructores
    public MateriasController(IMateriaService materiaService)
    {
        _materiaService = materiaService;
    }

    // Endpoints
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MateriaResponse>>> GetMateriasDisponibles()
    {
        var materias = await _materiaService.GetMateriasDisponiblesAsync();

        return Ok(materias);
    }
}
