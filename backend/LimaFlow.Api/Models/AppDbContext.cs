using Microsoft.EntityFrameworkCore;

namespace LimaFlow.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Via> Vias { get; set; } // Lo dejamos con mayúscula por convención de C#
    public DbSet<Zona> Zonas { get; set; } // <--- Esta es la nueva línea
    public DbSet<Categoria> Categorias { get; set; } // Nueva
    public DbSet<Usuario> Usuarios { get; set; }     // Nueva
    public DbSet<Incidencia> Incidencias { get; set; } // Nueva

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Esto le dice a EF: "Cuando busques la tabla 'Vias', ve a la tabla que en Postgres se llama exactamente 'vias'"
        modelBuilder.Entity<Via>().ToTable("vias"); 

        // 2. Agrega la configuración de la relación (Foreign Key)
    modelBuilder.Entity<Via>()
        .HasOne(v => v.Zona)
        .WithMany(z => z.Vias)
        .HasForeignKey(v => v.ZonaId);

        modelBuilder.Entity<Via>()
    .HasOne(v => v.Categoria)
    .WithMany(c => c.Vias)
    .HasForeignKey(v => v.CategoriaId)
    .IsRequired(false);
    
    // Configuración de Incidencia
    modelBuilder.Entity<Incidencia>()
        .HasOne(i => i.Via)
        .WithMany() // Si no quieres lista de incidencias en Via
        .HasForeignKey(i => i.ViaId);

    modelBuilder.Entity<Incidencia>()
        .HasOne(i => i.Usuario)
        .WithMany(u => u.Incidencias)
        .HasForeignKey(i => i.UsuarioId);

    }
}