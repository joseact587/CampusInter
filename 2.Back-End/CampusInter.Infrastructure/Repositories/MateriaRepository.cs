using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Domain.Entidades;
using CampusInter.Domain.Enums;
using CampusInter.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Repositories;

public sealed class MateriaRepository : IMateriaRepository
{
    // Atributos
    private readonly ApplicationDbContext _context;

    // Constructores
    public MateriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Consultas
    public async Task<IReadOnlyList<Materia>> GetActivasConProfesorAsync()
    {
        return await _context.Materias
            .AsNoTracking()
            .Include(materia => materia.Profesor)
            .Where(materia => materia.Estado == EstadoRegistro.Activo)
            .OrderBy(materia => materia.MateriaId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Materia>> GetByIdsAsync(IReadOnlyCollection<int> materiasIds)
    {
        return await _context.Materias
            .Include(materia => materia.Profesor)
            .Where(materia => materiasIds.Contains(materia.MateriaId))
            .ToListAsync();
    }
}
