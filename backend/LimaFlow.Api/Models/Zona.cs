using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimaFlow.Api.Models;

[Table("zonas")]
public class Zona
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la zona es obligatorio.")]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    // Relación: Una zona tiene muchas vías
    public ICollection<Via> Vias { get; set; } = new List<Via>();
}