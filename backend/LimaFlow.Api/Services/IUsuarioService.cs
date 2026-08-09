using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<Usuario> CreateAsync(Usuario usuario);
}
