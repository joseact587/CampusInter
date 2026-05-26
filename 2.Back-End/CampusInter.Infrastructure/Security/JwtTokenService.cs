using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CampusInter.Application.Interfaces.Security;
using CampusInter.Domain.Entidades;
using CampusInter.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CampusInter.Infrastructure.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    // Atributos
    private readonly JwtOptions _jwtOptions;

    // Constructores
    public JwtTokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    // Token
    public string GenerateToken(Usuario usuario, Estudiante estudiante)
    {
        if (string.IsNullOrWhiteSpace(_jwtOptions.SecretKey))
            throw new InvalidOperationException("La clave JWT no esta configurada.");

        var expirationMinutes = _jwtOptions.ExpirationMinutes > 0
            ? _jwtOptions.ExpirationMinutes
            : 60;

        var issuer = string.IsNullOrWhiteSpace(_jwtOptions.Issuer)
            ? "CampusInter"
            : _jwtOptions.Issuer;

        var audience = string.IsNullOrWhiteSpace(_jwtOptions.Audience)
            ? "CampusInter.Client"
            : _jwtOptions.Audience;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
            new(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
            new("estudianteId", estudiante.EstudianteId.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Correo),
            new(ClaimTypes.Email, usuario.Correo),
            new(ClaimTypes.Name, $"{estudiante.PrimerNombre} {estudiante.PrimerApellido}".Trim()),
            new(ClaimTypes.Role, usuario.Rol.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
