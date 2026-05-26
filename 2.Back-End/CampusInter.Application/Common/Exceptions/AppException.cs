namespace CampusInter.Application.Common.Exceptions;

/// <summary>
/// Excepcion base de la capa Application.
/// Permite centralizar codigo HTTP y codigo interno de error.
/// </summary>
public abstract class AppException : Exception
{
    public int StatusCode { get; }
    public string Code { get; }

    protected AppException(string message, int statusCode, string code)
        : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }
}
