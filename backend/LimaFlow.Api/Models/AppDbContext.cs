using Microsoft.EntityFrameworkCore;

namespace LimaFlow.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Via> Vias { get; set; } // Lo dejamos con mayúscula por convención de C#

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Esto le dice a EF: "Cuando busques la tabla 'Vias', ve a la tabla que en Postgres se llama exactamente 'vias'"
        modelBuilder.Entity<Via>().ToTable("vias"); 
    }
}