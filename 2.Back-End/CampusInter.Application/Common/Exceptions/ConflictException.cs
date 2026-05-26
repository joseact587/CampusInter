namespace CampusInter.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando hay conflicto con el estado actual del recurso.
/// </summary>
public sealed class ConflictException : AppException
{
    public ConflictException(string message, string code = "conflict")
        : base(message, 409, code)
    {
    }
}
