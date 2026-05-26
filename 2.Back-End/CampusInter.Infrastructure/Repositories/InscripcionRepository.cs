using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Domain.Entidades;
using CampusInter.Domain.Enums;
using CampusInter.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Repositories;

public sealed class InscripcionRepository : IInscripcionRepository
{
    // Atributos
    private readonly ApplicationDbContext _context;

    // Constructores
    public InscripcionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Comandos
    public async Task AgregarAsync(Inscripcion inscripcion)
    {
        await _context.Inscripciones.AddAsync(inscripcion);
    }

    // Consultas
    public Task<bool> TieneInscripcionActivaAsync(int estudianteId)
    {
        return _context.Inscripciones
            .AnyAsync(inscripcion =>
                inscripcion.EstudianteId == estudianteId &&
                inscripcion.Estado == EstadoInscripcion.Activa);
    }

    public Task<Inscripcion?> ObtenerActivaPorEstudianteIdAsync(int estudianteId)
    {
        return _context.Inscripciones
            .Include(inscripcion => inscripcion.InscripcionesMateria)
                .ThenInclude(detalle => detalle.Materia)
                    .ThenInclude(materia => materia.Profesor)
            .FirstOrDefaultAsync(inscripcion =>
                inscripcion.EstudianteId == estudianteId &&
                inscripcion.Estado == EstadoInscripcion.Activa);
    }
}
