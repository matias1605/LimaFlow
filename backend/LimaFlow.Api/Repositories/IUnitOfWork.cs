using LimaFlow.Api.Models;

namespace LimaFlow.Api.Repositories;

/// <summary>
/// Agrupa los repositorios de cada entidad y centraliza el guardado.
/// Garantiza que todas las operaciones de una petición se confirmen en una sola
/// transacción lógica mediante un único SaveChangesAsync.
/// </summary>
public interface IUnitOfWork
{
    IRepository<Via> Vias { get; }
    IRepository<Zona> Zonas { get; }
    IRepository<Categoria> Categorias { get; }
    IRepository<Usuario> Usuarios { get; }
    IRepository<Incidencia> Incidencias { get; }

    /// <summary>Confirma en la base de datos todos los cambios pendientes.</summary>
    Task<int> SaveChangesAsync();
}
