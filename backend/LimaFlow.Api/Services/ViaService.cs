using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Repositories;

namespace LimaFlow.Api.Services;

public class ViaService : IViaService
{
    private readonly IUnitOfWork _unitOfWork;

    public ViaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ViaDto>> GetAllAsync()
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

    public async Task<ViaDto?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Vias.GetAll()
            .Include(v => v.Zona)
            .Include(v => v.Categoria)
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
    }

    public async Task<ServiceResult<Via>> CreateAsync(Via via)
    {
        // Regla de negocio: si viene una categoría, debe existir.
        if (via.CategoriaId.HasValue)
        {
            var categoriaExiste = await _unitOfWork.Categorias.GetAll()
                .AnyAsync(c => c.Id == via.CategoriaId.Value);
            if (!categoriaExiste)
            {
                return ServiceResult<Via>.Failure($"La categoría con ID {via.CategoriaId} no existe.");
            }
        }

        await _unitOfWork.Vias.AddAsync(via);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<Via>.Success(via);
    }

    public async Task<ServiceResult<Via>> DeleteAsync(int id)
    {
        var via = await _unitOfWork.Vias.GetByIdAsync(id);
        if (via == null)
        {
            return ServiceResult<Via>.NotFound("La vía no existe.");
        }

        _unitOfWork.Vias.Remove(via);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<Via>.Success(via);
    }
}
