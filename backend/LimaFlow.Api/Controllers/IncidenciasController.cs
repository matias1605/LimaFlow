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

    // GET: api/Incidencias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidenciaDto>>> GetIncidencias()
    {
        return await _context.Incidencias
            .Include(i => i.Via)
            .Include(i => i.Usuario)
            .Select(i => new IncidenciaDto
            {
                Id = i.Id,
                Descripcion = i.Descripcion,
                NombreVia = i.Via != null ? i.Via.Nombre : "Vía no especificada",
                NombreUsuario = i.Usuario != null ? i.Usuario.Nombre : "Anónimo",
                FechaRegistro = i.FechaRegistro
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
}