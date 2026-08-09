using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Services;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> GetAllAsync();
    Task<Categoria> CreateAsync(Categoria categoria);
}
