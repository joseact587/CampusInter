using CampusInter.Application.Common.Exceptions;
using CampusInter.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Api.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    // Atributos
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    // Constructores
    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    // Manejo global
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Exception captured. Path={Path} TraceId={TraceId} Message={Message}",
            httpContext.Request.Path,
            traceId,
            exception.Message);

        var problem = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error interno del servidor",
            Detail = _environment.IsDevelopment()
                ? exception.Message
                : "Ocurrio un error interno en el servidor."
        };

        problem.Extensions["traceId"] = traceId;

        // Errores controlados de Application
        if (exception is AppException appException)
        {
            problem.Status = appException.StatusCode;
            problem.Title = "Error de aplicacion";
            problem.Detail = appException.Message;
            problem.Extensions["code"] = appException.Code;

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        // Errores controlados de Domain
        if (exception is DomainException domainException)
        {
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Title = "Error de dominio";
            problem.Detail = domainException.Message;
            problem.Extensions["code"] = "domain_error";

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        // Seguridad
        if (exception is UnauthorizedAccessException)
        {
            problem.Status = StatusCodes.Status401Unauthorized;
            problem.Title = "No autenticado";
            problem.Detail = "Debes autenticarte para acceder a este recurso.";
            problem.Extensions["code"] = "unauthorized";

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        // Solicitudes invalidas
        if (exception is ArgumentException or InvalidOperationException)
        {
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Title = "Solicitud incorrecta";
            problem.Detail = exception.Message;
            problem.Extensions["code"] = "bad_request";

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        // Concurrencia
        if (exception is DbUpdateConcurrencyException)
        {
            problem.Status = StatusCodes.Status409Conflict;
            problem.Title = "Conflicto de concurrencia";
            problem.Detail = "El registro fue modificado por otro proceso. Refresca los datos e intenta de nuevo.";
            problem.Extensions["code"] = "concurrency_conflict";
            problem.Extensions["hint"] = "Recarga la informacion y vuelve a intentarlo.";

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        // Base de datos
        if (exception is DbUpdateException dbUpdateException &&
            dbUpdateException.InnerException is SqlException sqlException)
        {
            MapSqlException(sqlException, problem, _environment);

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        // Servicios externos
        if (exception is HttpRequestException)
        {
            problem.Status = StatusCodes.Status502BadGateway;
            problem.Title = "Proveedor no disponible";
            problem.Detail = "No fue posible completar la operacion con un servicio externo.";
            problem.Extensions["code"] = "external_error";

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        // Tiempos de espera
        if (exception is TimeoutException or TaskCanceledException)
        {
            problem.Status = StatusCodes.Status504GatewayTimeout;
            problem.Title = "Tiempo de espera agotado";
            problem.Detail = "La operacion tardo demasiado en responder.";
            problem.Extensions["code"] = "timeout";

            await WriteProblemAsync(httpContext, problem, cancellationToken);
            return true;
        }

        problem.Extensions["code"] = "server_error";

        if (_environment.IsDevelopment())
        {
            problem.Extensions["exception"] = exception.GetType().FullName;
            problem.Extensions["stackTrace"] = exception.StackTrace;
        }

        await WriteProblemAsync(httpContext, problem, cancellationToken);
        return true;
    }

    // Errores SQL
    private static void MapSqlException(
        SqlException sqlException,
        ProblemDetails problem,
        IHostEnvironment environment)
    {
        switch (sqlException.Number)
        {
            case 2627:
            case 2601:
                problem.Status = StatusCodes.Status409Conflict;
                problem.Title = "Registro duplicado";
                problem.Detail = "Ya existe un registro con estos datos unicos.";
                problem.Extensions["code"] = "db_unique_violation";
                break;

            case 547:
                problem.Status = StatusCodes.Status409Conflict;
                problem.Title = "Conflicto de relacion";
                problem.Detail = "No se puede realizar la operacion porque existen dependencias relacionadas.";
                problem.Extensions["code"] = "db_fk_violation";
                break;

            case -2:
                problem.Status = StatusCodes.Status503ServiceUnavailable;
                problem.Title = "Base de datos ocupada";
                problem.Detail = "Tiempo de espera agotado al acceder a la base de datos.";
                problem.Extensions["code"] = "db_timeout";
                break;

            default:
                problem.Status = StatusCodes.Status503ServiceUnavailable;
                problem.Title = "Error de base de datos";
                problem.Detail = "Hubo un problema al procesar la solicitud en el servidor de datos.";
                problem.Extensions["code"] = "db_error";
                break;
        }

        if (environment.IsDevelopment())
        {
            problem.Extensions["sqlNumber"] = sqlException.Number;
            problem.Extensions["sqlMessage"] = sqlException.Message;
        }
    }

    // Respuesta
    private static async Task WriteProblemAsync(
        HttpContext context,
        ProblemDetails problem,
        CancellationToken cancellationToken)
    {
        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
    }
}
