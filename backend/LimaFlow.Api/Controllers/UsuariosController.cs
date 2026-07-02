using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;
using LimaFlow.Api.DTOs;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
    {
        return await _context.Usuarios
            .Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre
            })
            .ToListAsync();
    }

    // POST: api/Usuarios
    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuario);
    }
}