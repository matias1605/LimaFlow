using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Services;

namespace LimaFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
    {
        var categorias = await _service.GetAllAsync();
        return Ok(categorias);
    }

    // POST: api/Categorias
    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
    {
        var creada = await _service.CreateAsync(categoria);
        return CreatedAtAction(nameof(GetCategorias), new { id = creada.Id }, creada);
    }
}
