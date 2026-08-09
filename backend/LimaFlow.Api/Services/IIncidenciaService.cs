using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Services;

public interface IIncidenciaService
{
    Task<PagedResultDto<IncidenciaDto>> GetPagedAsync(
        EstadoIncidencia? estado, int? viaId, int pageNumber, int pageSize);

    Task<ServiceResult<Incidencia>> CreateAsync(Incidencia incidencia);

    Task<ServiceResult<Incidencia>> ChangeEstadoAsync(int id, EstadoIncidencia nuevoEstado);
}
