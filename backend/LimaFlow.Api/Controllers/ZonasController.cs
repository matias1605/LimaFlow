using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ZonasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/zonas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Zona>>> GetZonas()
    {
        return await _context.Zonas.ToListAsync();
    }

    // POST: api/zonas
    [HttpPost]
    public async Task<ActionResult<Zona>> CreateZona(Zona zona)
    {
        _context.Zonas.Add(zona);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetZonas), new { id = zona.Id }, zona);
    }

    // DELETE: api/zonas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteZona(int id)
    {
        var zona = await _context.Zonas.FindAsync(id);
        if (zona == null)
        {
            return NotFound(new { mensaje = $"No se puede eliminar: La zona con ID {id} no existe." });
        }

        _context.Zonas.Remove(zona);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}