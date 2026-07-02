using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;
using LimaFlow.Api.DTOs;
using FluentValidation;

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

// GET: api/Incidencias?pageNumber=1&pageSize=10
[HttpGet]
public async Task<ActionResult<PagedResultDto<IncidenciaDto>>> GetIncidencias(
    [FromQuery] EstadoIncidencia? estado, 
    [FromQuery] int? viaId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
{
    // 1. Validaciones de seguridad para evitar números negativos o abusos de tamaño
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1 || pageSize > 50) pageSize = 10; // Límite máximo de 50 por página

    // 2. Preparamos la consulta base (sin ejecutarla aún)
    IQueryable<Incidencia> query = _context.Incidencias
        .Include(i => i.Via)
        .Include(i => i.Usuario);

    // 3. Aplicamos los filtros dinámicos que ya tenías
    if (estado.HasValue)
    {
        query = query.Where(i => i.Estado == estado.Value);
    }

    if (viaId.HasValue)
    {
        query = query.Where(i => i.ViaId == viaId.Value);
    }

    // 4. Contamos el total de registros que cumplen con los filtros (Antes de paginar)
    var totalRecords = await query.CountAsync();

    // 5. Aplicamos la fórmula matemática de paginación y ordenamos por las más recientes
    var items = await query
        .OrderByDescending(i => i.FechaRegistro) // Las más nuevas primero
        .Skip((pageNumber - 1) * pageSize)       // Se salta los registros de páginas anteriores
        .Take(pageSize)                          // Toma solo la cantidad solicitada
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

    // 6. Retornamos el DTO contenedor con toda la metadata
    var response = new PagedResultDto<IncidenciaDto>
    {
        Items = items,
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalRecords = totalRecords
    };

    return Ok(response);
}

 // POST: api/Incidencias
[HttpPost]
public async Task<ActionResult<Incidencia>> CreateIncidencia(
    Incidencia incidencia, 
    [FromServices] IValidator<Incidencia> validator)
{
    // 1. Ejecutar la validación de FluentValidation
    var validationResult = await validator.ValidateAsync(incidencia);

    // 2. Si hay errores, devolvemos un BadRequest detallado automáticamente
    if (!validationResult.IsValid)
    {
        return BadRequest(new
        {
            mensaje = "Error de validación en los datos enviados.",
            errores = validationResult.ToDictionary() // Convierte los errores a un formato JSON limpio
        });
    }

    // 3. Si todo está bien, guardamos la incidencia en la base de datos
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