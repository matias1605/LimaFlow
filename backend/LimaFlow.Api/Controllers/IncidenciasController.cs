using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;
using LimaFlow.Api.DTOs;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase
{
    private readonly AppDbContext _context;

    public IncidenciasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Incidencias?estado=2&viaId=8
[HttpGet]
public async Task<ActionResult<IEnumerable<IncidenciaDto>>> GetIncidencias(
    
    [FromQuery] EstadoIncidencia? estado, 
    [FromQuery] int? viaId)
{
    // 1. Preparamos la consulta sin ejecutarla aún (IQueryable)
    IQueryable<Incidencia> query = _context.Incidencias
        .Include(i => i.Via)
        .Include(i => i.Usuario);

    // 2. Si el usuario envió un estado, filtramos por estado
    if (estado.HasValue)
    {
        query = query.Where(i => i.Estado == estado.Value);
    }

    // 3. Si el usuario envió un viaId, filtramos por esa vía específica
    if (viaId.HasValue)
    {
        query = query.Where(i => i.ViaId == viaId.Value);
    }

    // 4. Proyectamos al DTO y ejecutamos la consulta en la base de datos
    return await query
        .Select(i => new IncidenciaDto
        {
            Id = i.Id,
            Descripcion = i.Descripcion,
            NombreVia = i.Via != null ? i.Via.Nombre : "Vía no especificada",
            NombreUsuario = i.Usuario != null ? i.Usuario.Nombre : "Anónimo",
            FechaRegistro = i.FechaRegistro,
            Estado = i.Estado.ToString()
        })
        .ToListAsync();
}

    // POST: api/Incidencias
    [HttpPost]
    public async Task<ActionResult<Incidencia>> CreateIncidencia(Incidencia incidencia)
    {
        // Forzamos la fecha a UTC para evitar problemas de zona horaria con Postgres
        incidencia.FechaRegistro = DateTime.UtcNow;

        _context.Incidencias.Add(incidencia);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetIncidencias), new { id = incidencia.Id }, incidencia);
    }

    // PUT: api/Incidencias/1/estado
    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDto dto)
    {
    // 1. Buscar la incidencia en la base de datos
    var incidencia = await _context.Incidencias.FindAsync(id);

    // 2. Si no existe, retornar un error 404
    if (incidencia == null)
    {
        return NotFound(new { mensaje = $"No se encontró la incidencia con ID {id}" });
    }

    // 3. Actualizar solo el campo del estado
    incidencia.Estado = dto.NuevoEstado;

    // 4. Guardar los cambios en PostgreSQL
    await _context.SaveChangesAsync();

    // 5. Responder con un mensaje amigable y el estado en texto
    return Ok(new { 
        mensaje = "Estado actualizado con éxito", 
        incidenciaId = id, 
        nuevoEstado = incidencia.Estado.ToString() 
        });
    }
}