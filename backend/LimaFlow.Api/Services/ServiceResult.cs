namespace LimaFlow.Api.Services;

/// <summary>
/// Resultado estandarizado que devuelven los servicios: éxito con datos, o
/// fallo con la lista de errores. Permite que los controladores mapeen
/// directamente a Ok / BadRequest / NotFound sin filtrar excepciones.
/// </summary>
public class ServiceResult<T>
{
    public bool Succeeded { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public IDictionary<string, string[]>? ValidationErrors { get; init; }
    public bool IsNotFound { get; init; }

    public static ServiceResult<T> Success(T data) =>
        new() { Succeeded = true, Data = data };

    public static ServiceResult<T> NotFound(string message) =>
        new() { Succeeded = false, IsNotFound = true, ErrorMessage = message };

    public static ServiceResult<T> Failure(string message) =>
        new() { Succeeded = false, ErrorMessage = message };

    public static ServiceResult<T> ValidationFailure(IDictionary<string, string[]> errors) =>
        new() { Succeeded = false, ValidationErrors = errors, ErrorMessage = "Error de validación en los datos enviados." };
}
