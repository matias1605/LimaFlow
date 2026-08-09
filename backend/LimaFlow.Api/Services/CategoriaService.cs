using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Repositories;

namespace LimaFlow.Api.Services;

public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
    {
        return await _unitOfWork.Categorias.GetAll()
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre
            })
            .ToListAsync();
    }

    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        await _unitOfWork.Categorias.AddAsync(categoria);
        await _unitOfWork.SaveChangesAsync();
        return categoria;
    }
}
