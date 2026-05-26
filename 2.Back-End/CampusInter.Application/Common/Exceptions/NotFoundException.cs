namespace CampusInter.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando un recurso solicitado no existe.
/// </summary>
public sealed class NotFoundException : AppException
{
    public NotFoundException(string message, string code = "not_found")
        : base(message, 404, code)
    {
    }
}
