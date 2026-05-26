using System.Net.Mail;
using CampusInter.Domain.Common.Auditing;
using CampusInter.Domain.Common.Auditing.Interfaces;
using CampusInter.Domain.Enums;

namespace CampusInter.Domain.Entidades;

public class Estudiante : FullAuditedEntity, IHasConcurrencyToken
{
    // Atributos
    public int EstudianteId { get; private set; }
    public string PrimerNombre { get; private set; } = string.Empty;
    public string? SegundoNombre { get; private set; }
    public string PrimerApellido { get; private set; } = string.Empty;
    public string? SegundoApellido { get; private set; }
    public string Correo { get; private set; } = string.Empty;
    public string Documento { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public EstadoEstudiante Estado { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    // Navegación: inscripciones realizadas por el estudiante
    private readonly List<Inscripcion> _inscripciones = [];

    public IReadOnlyCollection<Inscripcion> Inscripciones => _inscripciones.AsReadOnly();

    // Constructores
    private Estudiante()
    {
    }

    public Estudiante(
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido,
        string correo,
        string documento,
        string passwordHash)
    {
        ActualizarDatosBasicos(primerNombre, segundoNombre, primerApellido, segundoApellido, correo, documento);
        CambiarPasswordHash(passwordHash);
        Estado = EstadoEstudiante.Activo;
    }

    // Métodos de dominio
    public void ActualizarDatosBasicos(
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido,
        string correo,
        string documento)
    {
        if (string.IsNullOrWhiteSpace(primerNombre))
            throw new ArgumentException("El primer nombre del estudiante es obligatorio.", nameof(primerNombre));

        if (string.IsNullOrWhiteSpace(primerApellido))
            throw new ArgumentException("El primer apellido del estudiante es obligatorio.", nameof(primerApellido));

        ValidarCorreo(correo);

        if (string.IsNullOrWhiteSpace(documento))
            throw new ArgumentException("El documento del estudiante es obligatorio.", nameof(documento));

        PrimerNombre = primerNombre.Trim();
        SegundoNombre = string.IsNullOrWhiteSpace(segundoNombre) ? null : segundoNombre.Trim();
        PrimerApellido = primerApellido.Trim();
        SegundoApellido = string.IsNullOrWhiteSpace(segundoApellido) ? null : segundoApellido.Trim();
        Correo = correo.Trim().ToLowerInvariant();
        Documento = documento.Trim();
    }

    public void CambiarPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de la contraseña es obligatorio.", nameof(passwordHash));

        PasswordHash = passwordHash;
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

    private static void ValidarCorreo(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("El correo del estudiante es obligatorio.", nameof(correo));

        var correoNormalizado = correo.Trim();

        try
        {
            var direccionCorreo = new MailAddress(correoNormalizado);

            if (direccionCorreo.Address != correoNormalizado)
                throw new ArgumentException("El correo del estudiante no tiene un formato válido.", nameof(correo));
        }
        catch (FormatException)
        {
            throw new ArgumentException("El correo del estudiante no tiene un formato válido.", nameof(correo));
        }
    }
}
