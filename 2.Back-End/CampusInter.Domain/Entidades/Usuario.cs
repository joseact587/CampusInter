using System.Net.Mail;
using CampusInter.Domain.Common.Auditing;
using CampusInter.Domain.Common.Auditing.Interfaces;
using CampusInter.Domain.Enums;

namespace CampusInter.Domain.Entidades;

public class Usuario : FullAuditedEntity, IHasConcurrencyToken
{
    // Atributos
    public int UsuarioId { get; private set; }
    public string Correo { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public RolUsuario Rol { get; private set; }
    public EstadoUsuario Estado { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    // Navegacion: perfil academico asociado al usuario
    public Estudiante? Estudiante { get; private set; }

    // Constructores
    private Usuario()
    {
    }

    public Usuario(
        string correo,
        string passwordHash,
        RolUsuario rol)
    {
        CambiarCorreo(correo);
        CambiarPasswordHash(passwordHash);
        AsignarRol(rol);
        Estado = EstadoUsuario.Activo;
    }

    // Metodos de dominio
    public void CambiarCorreo(string correo)
    {
        ValidarCorreo(correo);

        Correo = correo.Trim().ToLowerInvariant();
    }

    public void CambiarPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de la contrasena es obligatorio.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void AsignarRol(RolUsuario rol)
    {
        if (!Enum.IsDefined(typeof(RolUsuario), rol))
            throw new ArgumentException("El rol del usuario no es valido.", nameof(rol));

        Rol = rol;
    }

    public void Activar()
    {
        Estado = EstadoUsuario.Activo;
    }

    public void Desactivar()
    {
        Estado = EstadoUsuario.Inactivo;
    }

    public bool EstaActivo()
    {
        return Estado == EstadoUsuario.Activo;
    }

    private static void ValidarCorreo(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("El correo del usuario es obligatorio.", nameof(correo));

        var correoNormalizado = correo.Trim();

        try
        {
            var direccionCorreo = new MailAddress(correoNormalizado);

            if (direccionCorreo.Address != correoNormalizado)
                throw new ArgumentException("El correo del usuario no tiene un formato valido.", nameof(correo));
        }
        catch (FormatException)
        {
            throw new ArgumentException("El correo del usuario no tiene un formato valido.", nameof(correo));
        }
    }
}
