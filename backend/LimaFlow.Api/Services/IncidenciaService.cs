using FluentValidation;
using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Repositories;

namespace LimaFlow.Api.Services;

/// <summary>
/// Lógica de negocio de incidencias. El controlador solo orquesta HTTP y
/// llama a estos métodos. Toda validación, filtrado, paginación, cálculo de
/// FechaRegistro y cambio de estado vive acá.
/// </summary>
public class IncidenciaService : IIncidenciaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<Incidencia> _validator;

    public IncidenciaService(IUnitOfWork unitOfWork, IValidator<Incidencia> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<PagedResultDto<IncidenciaDto>> GetPagedAsync(
        EstadoIncidencia? estado, int? viaId, int pageNumber, int pageSize)
    {
        // Saneamos parámetros de paginación (misma regla que tenía el controlador).
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1 || pageSize > 50) pageSize = 10;

        IQueryable<Incidencia> query = _unitOfWork.Incidencias.GetAll()
            .Include(i => i.Via)
            .Include(i => i.Usuario);

        if (estado.HasValue)
            query = query.Where(i => i.Estado == estado.Value);

        if (viaId.HasValue)
            query = query.Where(i => i.ViaId == viaId.Value);

        var totalRecords = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.FechaRegistro)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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

        return new PagedResultDto<IncidenciaDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords
        };
    }

    public async Task<ServiceResult<Incidencia>> CreateAsync(Incidencia incidencia)
    {
        var validationResult = await _validator.ValidateAsync(incidencia);
        if (!validationResult.IsValid)
        {
            return ServiceResult<Incidencia>.ValidationFailure(validationResult.ToDictionary());
        }

        // Regla de negocio: la fecha de registro siempre en UTC, la fija el servidor.
        incidencia.FechaRegistro = DateTime.UtcNow;

        await _unitOfWork.Incidencias.AddAsync(incidencia);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<Incidencia>.Success(incidencia);
    }

    public async Task<ServiceResult<Incidencia>> ChangeEstadoAsync(int id, EstadoIncidencia nuevoEstado)
    {
        var incidencia = await _unitOfWork.Incidencias.GetByIdAsync(id);
        if (incidencia == null)
        {
            return ServiceResult<Incidencia>.NotFound($"No se encontró la incidencia con ID {id}");
        }

        incidencia.Estado = nuevoEstado;
        _unitOfWork.Incidencias.Update(incidencia);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<Incidencia>.Success(incidencia);
    }
}
