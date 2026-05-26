using CampusInter.Domain.Common.Auditing;
using CampusInter.Domain.Common.Auditing.Interfaces;
using CampusInter.Domain.Common.Exceptions;
using CampusInter.Domain.Enums;

namespace CampusInter.Domain.Entidades;

public class Inscripcion : FullAuditedEntity, IHasConcurrencyToken
{
    // Atributos
    public const int CantidadMateriasPermitidas = 3;
    public const int TotalCreditosEsperados = 9;
    public int InscripcionId { get; private set; }
    public int EstudianteId { get; private set; }
    public Estudiante Estudiante { get; private set; } = null!;
    public DateTime FechaInscripcion { get; private set; }
    public int TotalCreditos { get; private set; }
    public EstadoInscripcion Estado { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    // Navegación: materias asociadas a la inscripción
    private readonly List<InscripcionMateria> _inscripcionesMateria = [];

    public IReadOnlyCollection<InscripcionMateria> InscripcionesMateria => _inscripcionesMateria.AsReadOnly();

    // Constructores
    private Inscripcion()
    {
    }

    public Inscripcion(int estudianteId, IReadOnlyCollection<Materia> materias)
    {
        if (estudianteId <= 0)
            throw new ArgumentException("El estudiante es obligatorio.", nameof(estudianteId));

        ValidarReglasDeInscripcion(materias);

        EstudianteId = estudianteId;
        FechaInscripcion = DateTime.UtcNow;
        Estado = EstadoInscripcion.Activa;
        TotalCreditos = materias.Sum(materia => materia.Creditos);

        foreach (var materia in materias)
        {
            _inscripcionesMateria.Add(new InscripcionMateria(this, materia));
        }
    }

    // Métodos de dominio
    public void Cancelar()
    {
        Estado = EstadoInscripcion.Cancelada;
    }

    public bool EstaActiva()
    {
        return Estado == EstadoInscripcion.Activa;
    }

    private static void ValidarReglasDeInscripcion(IReadOnlyCollection<Materia> materias)
    {
        if (materias is null)
            throw new ArgumentNullException(nameof(materias), "Las materias son obligatorias.");

        if (materias.Count != CantidadMateriasPermitidas)
            throw new DomainException("El estudiante debe seleccionar exactamente 3 materias.");

        if (materias.Any(materia => !materia.EstaActiva()))
            throw new DomainException("No se pueden seleccionar materias inactivas.");

        var idsMaterias = materias.Select(materia => materia.MateriaId).ToList();

        if (idsMaterias.Distinct().Count() != CantidadMateriasPermitidas)
            throw new DomainException("No se puede seleccionar la misma materia más de una vez.");

        var profesores = materias.Select(materia => materia.ProfesorId).ToList();

        if (profesores.Distinct().Count() != CantidadMateriasPermitidas)
            throw new DomainException("El estudiante no puede seleccionar materias dictadas por el mismo profesor.");

        var totalCreditos = materias.Sum(materia => materia.Creditos);

        if (totalCreditos != TotalCreditosEsperados)
            throw new DomainException("La inscripción debe tener exactamente 9 créditos.");
    }

}
