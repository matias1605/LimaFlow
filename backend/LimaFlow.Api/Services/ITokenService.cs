using LimaFlow.Api.Models;

namespace LimaFlow.Api.Services;

public interface ITokenService
{
    /// <summary>Genera un JWT firmado con los claims del usuario y sus roles.</summary>
    (string Token, DateTime Expiration) CreateToken(ApplicationUser user, IEnumerable<string> roles);
}
