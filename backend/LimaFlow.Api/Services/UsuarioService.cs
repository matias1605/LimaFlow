using Microsoft.EntityFrameworkCore;
using LimaFlow.Api.DTOs;
using LimaFlow.Api.Models;
using LimaFlow.Api.Repositories;

namespace LimaFlow.Api.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        return await _unitOfWork.Usuarios.GetAll()
            .Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre
            })
            .ToListAsync();
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        await _unitOfWork.Usuarios.AddAsync(usuario);
        await _unitOfWork.SaveChangesAsync();
        return usuario;
    }
}
