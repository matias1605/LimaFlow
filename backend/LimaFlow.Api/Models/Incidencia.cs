using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimaFlow.Api.Models;

[Table("incidencias")]
public class Incidencia
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Column("descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    // Relación con Vía
    [Column("via_id")]
    public int ViaId { get; set; }
    public Via? Via { get; set; } = null!;

    // Relación con Usuario
    [Column("usuario_id")]
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; } = null!;

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Añade esto dentro de la clase Incidencia en Incidencia.cs
    [Required]
    [Column("estado")]
    public EstadoIncidencia Estado { get; set; } = EstadoIncidencia.Reportada;
}