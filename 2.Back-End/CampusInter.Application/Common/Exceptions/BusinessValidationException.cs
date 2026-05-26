namespace CampusInter.Application.Common.Exceptions;

/// <summary>
/// Representa errores de validacion del caso de uso o reglas de aplicacion.
/// </summary>
public sealed class BusinessValidationException : AppException
{
    public BusinessValidationException(string message, string code = "validation_error")
        : base(message, 400, code)
    {
    }
}
