using CampusInter.Domain.Common.Auditing;
using CampusInter.Domain.Common.Auditing.Interfaces;
using CampusInter.Domain.Enums;

namespace CampusInter.Domain.Entidades;

public class Estudiante : FullAuditedEntity, IHasConcurrencyToken
{
    // Atributos
    public int EstudianteId { get; private set; }
    public int UsuarioId { get; private set; }
    public string PrimerNombre { get; private set; } = string.Empty;
    public string? SegundoNombre { get; private set; }
    public string PrimerApellido { get; private set; } = string.Empty;
    public string? SegundoApellido { get; private set; }
    public string Documento { get; private set; } = string.Empty;
    public EstadoEstudiante Estado { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    // Navegacion: usuario de acceso asociado al estudiante
    public Usuario Usuario { get; private set; } = null!;

    // Navegacion: inscripciones realizadas por el estudiante
    private readonly List<Inscripcion> _inscripciones = [];

    public IReadOnlyCollection<Inscripcion> Inscripciones => _inscripciones.AsReadOnly();

    // Constructores
    private Estudiante()
    {
    }

    public Estudiante(
        int usuarioId,
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido,
        string documento)
    {
        if (usuarioId <= 0)
            throw new ArgumentException("El usuario asociado al estudiante es obligatorio.", nameof(usuarioId));

        UsuarioId = usuarioId;
        ActualizarDatosBasicos(primerNombre, segundoNombre, primerApellido, segundoApellido, documento);
        Estado = EstadoEstudiante.Activo;
    }

    // Metodos de dominio
    public void ActualizarDatosBasicos(
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido,
        string documento)
    {
        if (string.IsNullOrWhiteSpace(primerNombre))
            throw new ArgumentException("El primer nombre del estudiante es obligatorio.", nameof(primerNombre));

        if (string.IsNullOrWhiteSpace(primerApellido))
            throw new ArgumentException("El primer apellido del estudiante es obligatorio.", nameof(primerApellido));

        if (string.IsNullOrWhiteSpace(documento))
            throw new ArgumentException("El documento del estudiante es obligatorio.", nameof(documento));

        PrimerNombre = primerNombre.Trim();
        SegundoNombre = string.IsNullOrWhiteSpace(segundoNombre) ? null : segundoNombre.Trim();
        PrimerApellido = primerApellido.Trim();
        SegundoApellido = string.IsNullOrWhiteSpace(segundoApellido) ? null : segundoApellido.Trim();
        Documento = documento.Trim();
    }

    public void Activar()
    {
        Estado = EstadoEstudiante.Activo;
    }

    public void Desactivar()
    {
        Estado = EstadoEstudiante.Inactivo;
    }

    public bool EstaActivo()
    {
        return Estado == EstadoEstudiante.Activo;
    }
}
