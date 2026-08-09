using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LimaFlow.Api.Models;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Via> Vias { get; set; }
    public DbSet<Zona> Zonas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Incidencia> Incidencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // IMPORTANTE: llamar a base primero para que Identity registre sus tablas.
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Via>().ToTable("vias");

        modelBuilder.Entity<Via>()
            .HasOne(v => v.Zona)
            .WithMany(z => z.Vias)
            .HasForeignKey(v => v.ZonaId);

        modelBuilder.Entity<Via>()
            .HasOne(v => v.Categoria)
            .WithMany(c => c.Vias)
            .HasForeignKey(v => v.CategoriaId)
            .IsRequired(false);

        modelBuilder.Entity<Incidencia>()
            .HasOne(i => i.Via)
            .WithMany()
            .HasForeignKey(i => i.ViaId);

        modelBuilder.Entity<Incidencia>()
            .HasOne(i => i.Usuario)
            .WithMany(u => u.Incidencias)
            .HasForeignKey(i => i.UsuarioId);
    }
}
