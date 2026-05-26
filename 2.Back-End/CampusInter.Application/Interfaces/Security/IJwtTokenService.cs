using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Interfaces.Security;

public interface IJwtTokenService
{
    string GenerateToken(Usuario usuario, Estudiante estudiante);
}
