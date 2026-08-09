using Microsoft.AspNetCore.Mvc;
using LimaFlow.Api.Models;
using LimaFlow.Api.Services;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonasController : ControllerBase
{
    private readonly IZonaService _service;

    public ZonasController(IZonaService service)
    {
        _service = service;
    }

    // GET: api/zonas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Zona>>> GetZonas()
    {
        var zonas = await _service.GetAllAsync();
        return Ok(zonas);
    }

    // POST: api/zonas
    [HttpPost]
    public async Task<ActionResult<Zona>> CreateZona(Zona zona)
    {
        var creada = await _service.CreateAsync(zona);
        return CreatedAtAction(nameof(GetZonas), new { id = creada.Id }, creada);
    }

    // DELETE: api/zonas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteZona(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result.IsNotFound)
        {
            return NotFound(new { mensaje = result.ErrorMessage });
        }
        return NoContent();
    }
}
