using CampusInter.Application.Interfaces.Security;
using Microsoft.AspNetCore.Identity;

namespace CampusInter.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    // Atributos
    private readonly PasswordHasher<object> _passwordHasher = new();

    // Seguridad
    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("La contrasena es obligatoria.", nameof(password));

        return _passwordHasher.HashPassword(new object(), password);
    }

    public bool Verify(string passwordHash, string password)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return false;

        if (string.IsNullOrWhiteSpace(password))
            return false;

        var result = _passwordHasher.VerifyHashedPassword(new object(), passwordHash, password);

        return result != PasswordVerificationResult.Failed;
    }
}
