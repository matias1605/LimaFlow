using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Repositories;
using LimaFlow.Api.Services;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existente = await _userManager.FindByEmailAsync(dto.Email);
        if (existente != null)
        {
            return BadRequest(new { mensaje = "Ya existe un usuario con ese email." });
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            Nombre = dto.Nombre
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                mensaje = "No se pudo crear el usuario.",
                errores = result.Errors.Select(e => e.Description)
            });
        }

        // Todo nuevo registro entra como Ciudadano por defecto.
        await _userManager.AddToRoleAsync(user, Roles.Ciudadano);

        // Espejo en la entidad de dominio Usuario para poder autor incidencias con FK int.
        await _unitOfWork.Usuarios.AddAsync(new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email
        });
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { mensaje = "Usuario registrado con éxito.", email = user.Email });
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return Unauthorized(new { mensaje = "Email o contraseña incorrectos." });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiration) = _tokenService.CreateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Expiration = expiration,
            Email = user.Email ?? string.Empty,
            Roles = roles
        });
    }
}
