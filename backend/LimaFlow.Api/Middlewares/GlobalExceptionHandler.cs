using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LimaFlow.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Registramos el error real en los logs de la consola para el desarrollador
        _logger.LogError(exception, "Ocurrió un error no controlado: {Message}", exception.Message);

        // 2. Preparamos una respuesta estandarizada basada en el formato RFC 7807 (ProblemDetails)
        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Error Interno del Servidor",
            Detail = "Ocurrió un problema inesperado en el servidor. Por favor, inténtalo más tarde.",
            Instance = httpContext.Request.Path
        };

        // 3. Configuramos la respuesta HTTP
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        // 4. Enviamos el JSON limpio al cliente/frontend
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Retornamos true para indicarle a .NET que ya manejamos el error con éxito
        return true;
    }
}