using LimaFlow.Api.Models;

namespace LimaFlow.Api.Services;

public interface IZonaService
{
    Task<IEnumerable<Zona>> GetAllAsync();
    Task<Zona> CreateAsync(Zona zona);
    Task<ServiceResult<Zona>> DeleteAsync(int id);
}
