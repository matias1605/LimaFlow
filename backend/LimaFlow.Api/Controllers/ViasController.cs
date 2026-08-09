using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Repositories;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ViasController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/vias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ViaDto>>> GetVias()
    {
        return await _unitOfWork.Vias.GetAll()
            .Include(v => v.Zona)
            .Include(v => v.Categoria)  
            .Select(v => new ViaDto
            {
                Id = v.Id,
                Nombre = v.Nombre,
                CategoriaId = v.CategoriaId,
                NombreCategoria = v.Categoria != null ? v.Categoria.Nombre : "Sin Categoría",
                NombreZona = v.Zona != null ? v.Zona.Nombre : "Sin Zona"
            })
            .ToListAsync();
    }

    // GET: api/vias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ViaDto>> GetVia(int id)
    {
        var via = await _unitOfWork.Vias.GetAll()
            .Include(v => v.Zona)
            .Include(v => v.Categoria)  // <--- Agrega la nueva relación con Categoría
            .Where(v => v.Id == id)
            .Select(v => new ViaDto
            {
                Id = v.Id,
                Nombre = v.Nombre,
                NombreZona = v.Zona != null ? v.Zona.Nombre : "Sin Zona",
                CategoriaId = v.CategoriaId,
                NombreCategoria = v.Categoria != null ? v.Categoria.Nombre : "Sin Categoría"
            })
            .FirstOrDefaultAsync();

        if (via == null) return NotFound();
        return via;
    }

    // POST: api/vias
    [HttpPost]
    public async Task<ActionResult<Via>> CreateVia(Via via)
    {
        // Validación de seguridad: si envían un categoriaId, verificamos que exista en Postgres
        if (via.CategoriaId.HasValue)
        {
            var categoriaExiste = await _unitOfWork.Categorias.GetAll()
                .AnyAsync(c => c.Id == via.CategoriaId.Value);
            if (!categoriaExiste)
            {
                return BadRequest(new { mensaje = $"La categoría con ID {via.CategoriaId} no existe." });
            }
        }
        await _unitOfWork.Vias.AddAsync(via);
        await _unitOfWork.SaveChangesAsync();
        
        // Retornamos el objeto creado
        return CreatedAtAction(nameof(GetVia), new { id = via.Id }, via);
    }

    // DELETE: api/vias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVia(int id)
    {
        var via = await _unitOfWork.Vias.GetByIdAsync(id);
        if (via == null) return NotFound(new { mensaje = "La vía no existe." });

        _unitOfWork.Vias.Remove(via);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}