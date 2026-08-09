namespace LimaFlow.Api.Models;

/// <summary>Nombres de rol utilizados por Identity y por los atributos [Authorize].</summary>
public static class Roles
{
    public const string Ciudadano = "Ciudadano";
    public const string Administrador = "Administrador";

    public static readonly string[] All = { Ciudadano, Administrador };
}
