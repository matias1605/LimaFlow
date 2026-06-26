using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ViasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/vias
   [HttpGet("{id}")]
public async Task<ActionResult<Via>> GetVias(int id)
{
    // Buscamos la vía por su ID en la base de datos
    var via = await _context.Vias.FindAsync(id);

    // Si no existe, devolvemos un 404 (Not Found)
    if (via == null)
    {
        return NotFound(new { mensaje = $"No se encontró la vía con ID {id}" });
    }

    return via;
}
    [HttpPost]
public async Task<ActionResult<Via>> CreateVia(Via via)
{
    // Agregamos la vía al DbSet
    _context.Vias.Add(via);
    
    // Guardamos los cambios en la base de datos (Postgres)
    await _context.SaveChangesAsync();

    // Retornamos el objeto creado con un código 201 (Created)
    return CreatedAtAction(nameof(GetVias), new { id = via.Id }, via);
}
}