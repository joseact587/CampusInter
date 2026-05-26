using System.Security.Claims;
using CampusInter.Application.Interfaces.Security;
using Microsoft.AspNetCore.Http;

namespace CampusInter.Infrastructure.Security;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http)
    {
        _http = http;
    }

    public bool IsAuthenticated =>
        _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public string UsuarioId
    {
        get
        {
            var httpContext = _http.HttpContext;

            if (httpContext is null)
                return "system";

            if (httpContext.User?.Identity?.IsAuthenticated != true)
                return "auto-registro";

            return httpContext.User.FindFirst("sub")?.Value
                ?? httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? "system";
        }
    }

    public string? Email =>
        _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
        ?? _http.HttpContext?.User?.FindFirst("email")?.Value;
}
