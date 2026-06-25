using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimaFlow.Api.Models;

[Table("vias")] // El nombre de la tabla en Postgres
public class Via
{
    [Key]
    [Column("id")] // Mapeo exacto a la columna minúscula
    public int Id { get; set; }

    [Column("nombre")] // Mapeo exacto
    public string Nombre { get; set; } = string.Empty;

    [Column("zonaid")] // Mapeo exacto
    public int ZonaId { get; set; }
}