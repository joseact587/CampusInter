using CampusInter.Application.DTOs.Auth;

namespace CampusInter.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> RegistrarAsync(RegisterRequest request);

    Task<AuthResponse> IniciarSesionAsync(LoginRequest request);
}
