using CampusInter.Application.DTOs.Auth;
using CampusInter.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampusInter.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AutenticacionController : ControllerBase
{
    // Atributos
    private readonly IAuthService _authService;

    // Constructores
    public AutenticacionController(IAuthService authService)
    {
        _authService = authService;
    }

    // Endpoints
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Registrar([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegistrarAsync(request);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> IniciarSesion([FromBody] LoginRequest request)
    {
        var response = await _authService.IniciarSesionAsync(request);

        return Ok(response);
    }
}
