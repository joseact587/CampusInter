using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Interfaces.Persistence;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);

    Task<Usuario?> ObtenerPorIdAsync(int usuarioId);

    Task<bool> ExistePorCorreoAsync(string correo);

    Task<bool> ExistePorCorreoEnOtroUsuarioAsync(string correo, int usuarioId);

    Task AgregarAsync(Usuario usuario);
}
