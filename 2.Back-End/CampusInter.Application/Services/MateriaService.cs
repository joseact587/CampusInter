using CampusInter.Application.DTOs.Materias;
using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Application.Interfaces.Services;

namespace CampusInter.Application.Services;

public sealed class MateriaService : IMateriaService
{
    // Atributos
    private readonly IMateriaRepository _materiaRepository;

    // Constructores
    public MateriaService(IMateriaRepository materiaRepository)
    {
        _materiaRepository = materiaRepository;
    }

    // Consultas
    public async Task<IReadOnlyList<MateriaResponse>> GetMateriasDisponiblesAsync()
    {
        var materias = await _materiaRepository.GetActivasConProfesorAsync();

        return materias
            .Select(materia => new MateriaResponse
            {
                MateriaId = materia.MateriaId,
                Nombre = materia.Nombre,
                Creditos = materia.Creditos,
                ProfesorId = materia.ProfesorId,
                ProfesorNombre = ArmarNombreProfesor(
                    materia.Profesor.PrimerNombre,
                    materia.Profesor.SegundoNombre,
                    materia.Profesor.PrimerApellido,
                    materia.Profesor.SegundoApellido)
            })
            .ToList();
    }

    // Mapeo
    private static string ArmarNombreProfesor(
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido)
    {
        return string.Join(" ", new[]
        {
            primerNombre,
            segundoNombre,
            primerApellido,
            segundoApellido
        }.Where(valor => !string.IsNullOrWhiteSpace(valor)));
    }
}
