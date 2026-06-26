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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Via>>> GetVias()
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
}