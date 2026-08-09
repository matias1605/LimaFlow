using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.Models;
using LimaFlow.Api.Repositories;

namespace LimaFlow.Api.Services;

public class ZonaService : IZonaService
{
    private readonly IUnitOfWork _unitOfWork;

    public ZonaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Zona>> GetAllAsync() =>
        await _unitOfWork.Zonas.GetAll().ToListAsync();

    public async Task<Zona> CreateAsync(Zona zona)
    {
        await _unitOfWork.Zonas.AddAsync(zona);
        await _unitOfWork.SaveChangesAsync();
        return zona;
    }

    public async Task<ServiceResult<Zona>> DeleteAsync(int id)
    {
        var zona = await _unitOfWork.Zonas.GetByIdAsync(id);
        if (zona == null)
        {
            return ServiceResult<Zona>.NotFound($"No se puede eliminar: La zona con ID {id} no existe.");
        }

        _unitOfWork.Zonas.Remove(zona);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResult<Zona>.Success(zona);
    }
}
