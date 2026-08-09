using Microsoft.AspNetCore.Mvc;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Services;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
    {
        var usuarios = await _service.GetAllAsync();
        return Ok(usuarios);
    }

    // POST: api/Usuarios
    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
    {
        var creado = await _service.CreateAsync(usuario);
        return CreatedAtAction(nameof(GetUsuarios), new { id = creado.Id }, creado);
    }
}
