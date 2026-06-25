using Microsoft.EntityFrameworkCore;

namespace LimaFlow.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Via> Vias { get; set; }
}