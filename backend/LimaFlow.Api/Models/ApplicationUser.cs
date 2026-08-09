using Microsoft.AspNetCore.Identity;

namespace LimaFlow.Api.Models;

/// <summary>
/// Usuario de autenticación de ASP.NET Core Identity. Convive con la entidad
/// de dominio Usuario (que representa al autor de incidencias); se enlazan
/// por email.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? Nombre { get; set; }
}
