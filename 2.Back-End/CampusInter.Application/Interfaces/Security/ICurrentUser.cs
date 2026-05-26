namespace CampusInter.Application.Interfaces.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string UsuarioId { get; }
    string? EstudianteId { get; }
    string? Email { get; }
}
