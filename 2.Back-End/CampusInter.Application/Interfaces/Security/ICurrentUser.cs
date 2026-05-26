namespace CampusInter.Application.Interfaces.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string UsuarioId { get; }
    string? Email { get; }
}
