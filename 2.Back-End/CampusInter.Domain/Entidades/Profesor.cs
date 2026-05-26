using CampusInter.Domain.Common.Auditing;
using CampusInter.Domain.Enums;

namespace CampusInter.Domain.Entidades;

public class Profesor : FullAuditedEntity
{
    // Atributos
    public int ProfesorId { get; private set; }
    public string PrimerNombre { get; private set; } = string.Empty;
    public string? SegundoNombre { get; private set; }
    public string PrimerApellido { get; private set; } = string.Empty;
    public string? SegundoApellido { get; private set; }
    public EstadoRegistro Estado { get; private set; }

    // Navegación: materias dictadas por el profesor
    private readonly List<Materia> _materias = [];

    public IReadOnlyCollection<Materia> Materias => _materias.AsReadOnly();

    // Constructores
    private Profesor()
    {
    }

    public Profesor(
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido)
    {
        ActualizarNombre(primerNombre, segundoNombre, primerApellido, segundoApellido);
        Estado = EstadoRegistro.Activo;
    }

    // Métodos de dominio
    public void ActualizarNombre(
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido)
    {
        if (string.IsNullOrWhiteSpace(primerNombre))
            throw new ArgumentException("El primer nombre del profesor es obligatorio.", nameof(primerNombre));

        if (string.IsNullOrWhiteSpace(primerApellido))
            throw new ArgumentException("El primer apellido del profesor es obligatorio.", nameof(primerApellido));

        PrimerNombre = primerNombre.Trim();
        SegundoNombre = string.IsNullOrWhiteSpace(segundoNombre) ? null : segundoNombre.Trim();
        PrimerApellido = primerApellido.Trim();
        SegundoApellido = string.IsNullOrWhiteSpace(segundoApellido) ? null : segundoApellido.Trim();
    }

    public void Activar()
    {
        Estado = EstadoRegistro.Activo;
    }

    public void Desactivar()
    {
        Estado = EstadoRegistro.Inactivo;
    }

    public bool EstaActivo()
    {
        return Estado == EstadoRegistro.Activo;
    }
}
