using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;
using LimaFlow.Api.DTOs;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
    {
        return await _context.Categorias
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre
            })
            .ToListAsync();
    }

    // POST: api/Categorias
    [HttpPost]
    public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategorias), new { id = categoria.Id }, categoria);
    }
}