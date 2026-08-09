using Microsoft.AspNetCore.Mvc;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Services;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase
{
    private readonly IIncidenciaService _service;

    public IncidenciasController(IIncidenciaService service)
    {
        _service = service;
    }

    // GET: api/Incidencias?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<IncidenciaDto>>> GetIncidencias(
        [FromQuery] EstadoIncidencia? estado,
        [FromQuery] int? viaId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetPagedAsync(estado, viaId, pageNumber, pageSize);
        return Ok(result);
    }

    // POST: api/Incidencias
    [HttpPost]
    public async Task<ActionResult<Incidencia>> CreateIncidencia(Incidencia incidencia)
    {
        var result = await _service.CreateAsync(incidencia);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                mensaje = result.ErrorMessage,
                errores = result.ValidationErrors
            });
        }

        return CreatedAtAction(nameof(GetIncidencias), new { id = result.Data!.Id }, result.Data);
    }

    // PUT: api/Incidencias/1/estado
    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDto dto)
    {
        var result = await _service.ChangeEstadoAsync(id, dto.NuevoEstado);

        if (result.IsNotFound)
        {
            return NotFound(new { mensaje = result.ErrorMessage });
        }

        return Ok(new
        {
            mensaje = "Estado actualizado con éxito",
            incidenciaId = id,
            nuevoEstado = result.Data!.Estado.ToString()
        });
    }
}
