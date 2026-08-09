using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Services;

public interface IViaService
{
    Task<IEnumerable<ViaDto>> GetAllAsync();
    Task<ViaDto?> GetByIdAsync(int id);
    Task<ServiceResult<Via>> CreateAsync(Via via);
    Task<ServiceResult<Via>> DeleteAsync(int id);
}
