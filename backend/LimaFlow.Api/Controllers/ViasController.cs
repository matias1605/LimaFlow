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
        return await _context.Vias.ToListAsync();
    } // <--- Asegúrate de cerrar el método aquí

    [HttpGet("{id}")] // Agregué este para que el POST pueda hacer referencia al GetVia
    public async Task<ActionResult<Via>> GetVia(int id)
    {
        var via = await _context.Vias.FindAsync(id);
        if (via == null) return NotFound();
        return via;
    }

    [HttpPost]
    public async Task<ActionResult<Via>> CreateVia(Via via)
    {
        _context.Vias.Add(via);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetVia), new { id = via.Id }, via);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVia(int id)
    {
        var via = await _context.Vias.FindAsync(id);
        if (via == null)
        {
            return NotFound(new { mensaje = $"No se puede eliminar: La vía con ID {id} no existe." });
        }

        _context.Vias.Remove(via);
        await _context.SaveChangesAsync();

        return NoContent();
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