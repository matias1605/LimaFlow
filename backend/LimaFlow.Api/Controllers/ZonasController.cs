using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;
using LimaFlow.Api.Repositories;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ZonasController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/zonas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Zona>>> GetZonas()
    {
        return await _unitOfWork.Zonas.GetAll().ToListAsync();
    }

    // POST: api/zonas
    [HttpPost]
    public async Task<ActionResult<Zona>> CreateZona(Zona zona)
    {
        await _unitOfWork.Zonas.AddAsync(zona);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetZonas), new { id = zona.Id }, zona);
    }

    // DELETE: api/zonas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteZona(int id)
    {
        var zona = await _unitOfWork.Zonas.GetByIdAsync(id);
        if (zona == null)
        {
            return NotFound(new { mensaje = $"No se puede eliminar: La zona con ID {id} no existe." });
        }

        _unitOfWork.Zonas.Remove(zona);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}