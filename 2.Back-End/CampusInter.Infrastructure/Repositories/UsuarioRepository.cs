using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Domain.Entidades;
using CampusInter.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    // Atributos
    private readonly ApplicationDbContext _context;

    // Constructores
    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Consultas
    public Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        var correoNormalizado = correo.Trim().ToLowerInvariant();

        return _context.Usuarios
            .Include(usuario => usuario.Estudiante)
            .FirstOrDefaultAsync(usuario => usuario.Correo == correoNormalizado);
    }

    public Task<Usuario?> ObtenerPorIdAsync(int usuarioId)
    {
        return _context.Usuarios
            .Include(usuario => usuario.Estudiante)
            .FirstOrDefaultAsync(usuario => usuario.UsuarioId == usuarioId);
    }

    public Task<bool> ExistePorCorreoAsync(string correo)
    {
        var correoNormalizado = correo.Trim().ToLowerInvariant();

        return _context.Usuarios
            .AnyAsync(usuario => usuario.Correo == correoNormalizado);
    }

    public Task<bool> ExistePorCorreoEnOtroUsuarioAsync(string correo, int usuarioId)
    {
        var correoNormalizado = correo.Trim().ToLowerInvariant();

        return _context.Usuarios
            .AnyAsync(usuario =>
                usuario.Correo == correoNormalizado &&
                usuario.UsuarioId != usuarioId);
    }

    // Comandos
    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }
}
