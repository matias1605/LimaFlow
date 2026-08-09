namespace LimaFlow.Api.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string Email { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
}
