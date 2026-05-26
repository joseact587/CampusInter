using CampusInter.Domain.Common.Auditing;
using CampusInter.Domain.Common.Exceptions;
using CampusInter.Domain.Enums;

namespace CampusInter.Domain.Entidades;

public class Materia : FullAuditedEntity
{
    // Atributos
    public const int CreditosPorDefecto = 3;
    public int MateriaId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public int Creditos { get; private set; }
    public int ProfesorId { get; private set; }
    public Profesor Profesor { get; private set; } = null!;
    public EstadoRegistro Estado { get; private set; }

    // Navegación: inscripciones asociadas a la materia
    private readonly List<InscripcionMateria> _inscripcionesMateria = [];

    public IReadOnlyCollection<InscripcionMateria> InscripcionesMateria => _inscripcionesMateria.AsReadOnly();

    // Constructores
    private Materia()
    {
    }

    public Materia(
        string nombre,
        int profesorId,
        int creditos = CreditosPorDefecto)
    {
        ActualizarDatos(nombre, profesorId, creditos);
        Estado = EstadoRegistro.Activo;
    }

    // Métodos de dominio
    public void ActualizarDatos(
        string nombre,
        int profesorId,
        int creditos = CreditosPorDefecto)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la materia es obligatorio.", nameof(nombre));

        if (profesorId <= 0)
            throw new ArgumentException("El profesor de la materia es obligatorio.", nameof(profesorId));

        if (creditos != CreditosPorDefecto)
            throw new DomainException("Cada materia debe tener exactamente 3 créditos.");

        Nombre = nombre.Trim();
        ProfesorId = profesorId;
        Creditos = creditos;
    }

    public void Activar()
    {
        Estado = EstadoRegistro.Activo;
    }

    public void Desactivar()
    {
        Estado = EstadoRegistro.Inactivo;
    }

    public bool EstaActiva()
    {
        return Estado == EstadoRegistro.Activo;
    }
}
