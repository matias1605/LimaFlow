using LimaFlow.Api.Models;

namespace LimaFlow.Api.Repositories;

/// <summary>
/// Implementación del Unit of Work. Comparte un mismo AppDbContext entre todos
/// los repositorios (todos scoped por petición) y expone un único punto de guardado.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Vias = new Repository<Via>(_context);
        Zonas = new Repository<Zona>(_context);
        Categorias = new Repository<Categoria>(_context);
        Usuarios = new Repository<Usuario>(_context);
        Incidencias = new Repository<Incidencia>(_context);
    }

    public IRepository<Via> Vias { get; }
    public IRepository<Zona> Zonas { get; }
    public IRepository<Categoria> Categorias { get; }
    public IRepository<Usuario> Usuarios { get; }
    public IRepository<Incidencia> Incidencias { get; }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
