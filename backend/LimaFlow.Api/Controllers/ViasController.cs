using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Services;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViasController : ControllerBase
{
    private readonly IViaService _service;

    public ViasController(IViaService service)
    {
        _service = service;
    }

    // GET: api/vias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ViaDto>>> GetVias()
    {
        var vias = await _service.GetAllAsync();
        return Ok(vias);
    }

    // GET: api/vias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ViaDto>> GetVia(int id)
    {
        var via = await _service.GetByIdAsync(id);
        if (via == null) return NotFound();
        return via;
    }

    // POST: api/vias
    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<Via>> CreateVia(Via via)
    {
        var result = await _service.CreateAsync(via);
        if (!result.Succeeded)
        {
            return BadRequest(new { mensaje = result.ErrorMessage });
        }
        return CreatedAtAction(nameof(GetVia), new { id = result.Data!.Id }, result.Data);
    }

    // DELETE: api/vias/5
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> DeleteVia(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result.IsNotFound)
        {
            return NotFound(new { mensaje = result.ErrorMessage });
        }
        return NoContent();
    }
}
