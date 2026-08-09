namespace LimaFlow.Api.Repositories;

/// <summary>
/// Contrato genérico de acceso a datos. Desacopla los controladores/servicios
/// de Entity Framework Core: nadie fuera de esta capa toca el DbContext directamente.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Devuelve la consulta base como IQueryable para poder seguir componiendo
    /// filtros dinámicos, .Include() y paginación (.Skip()/.Take()) sin ejecutarla aún.
    /// </summary>
    IQueryable<T> GetAll();

    /// <summary>Busca una entidad por su clave primaria.</summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>Marca una entidad para inserción (se persiste con SaveChangesAsync).</summary>
    Task AddAsync(T entity);

    /// <summary>Marca una entidad como modificada.</summary>
    void Update(T entity);

    /// <summary>Marca una entidad para eliminación.</summary>
    void Remove(T entity);
}
