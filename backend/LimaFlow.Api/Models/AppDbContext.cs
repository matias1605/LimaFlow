using Microsoft.EntityFrameworkCore;

namespace LimaFlow.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Via> Vias { get; set; } // Lo dejamos con mayúscula por convención de C#
    public DbSet<Zona> Zonas { get; set; } // <--- Esta es la nueva línea

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Esto le dice a EF: "Cuando busques la tabla 'Vias', ve a la tabla que en Postgres se llama exactamente 'vias'"
        modelBuilder.Entity<Via>().ToTable("vias"); 

        // 2. Agrega la configuración de la relación (Foreign Key)
    modelBuilder.Entity<Via>()
        .HasOne<Zona>()
        .WithMany(z => z.Vias)
        .HasForeignKey(v => v.ZonaId);
    
    }
}