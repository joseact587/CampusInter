using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Domain.Entidades;
using CampusInter.Domain.Enums;
using CampusInter.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Repositories;

public sealed class EstudianteRepository : IEstudianteRepository
{
    // Atributos
    private readonly ApplicationDbContext _context;

    // Constructores
    public EstudianteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Consultas
    public async Task<IReadOnlyList<Estudiante>> ObtenerActivosAsync()
    {
        return await _context.Estudiantes
            .AsNoTracking()
            .Where(estudiante => estudiante.Estado == EstadoEstudiante.Activo)
            .OrderBy(estudiante => estudiante.PrimerApellido)
            .ThenBy(estudiante => estudiante.PrimerNombre)
            .ToListAsync();
    }

    public Task<Estudiante?> ObtenerPorIdAsync(int estudianteId)
    {
        return _context.Estudiantes
            .FirstOrDefaultAsync(estudiante => estudiante.EstudianteId == estudianteId);
    }

    public Task<Estudiante?> ObtenerPorIdSinSeguimientoAsync(int estudianteId)
    {
        return _context.Estudiantes
            .AsNoTracking()
            .FirstOrDefaultAsync(estudiante => estudiante.EstudianteId == estudianteId);
    }

    public Task<Estudiante?> ObtenerPorCorreoAsync(string correo)
    {
        var correoNormalizado = correo.Trim().ToLowerInvariant();

        return _context.Estudiantes
            .FirstOrDefaultAsync(estudiante => estudiante.Correo == correoNormalizado);
    }

    public Task<bool> ExistePorCorreoAsync(string correo)
    {
        var correoNormalizado = correo.Trim().ToLowerInvariant();

        return _context.Estudiantes
            .AnyAsync(estudiante => estudiante.Correo == correoNormalizado);
    }

    public Task<bool> ExistePorDocumentoAsync(string documento)
    {
        var documentoNormalizado = documento.Trim();

        return _context.Estudiantes
            .AnyAsync(estudiante => estudiante.Documento == documentoNormalizado);
    }

    // Comandos
    public async Task AgregarAsync(Estudiante estudiante)
    {
        await _context.Estudiantes.AddAsync(estudiante);
    }
}
