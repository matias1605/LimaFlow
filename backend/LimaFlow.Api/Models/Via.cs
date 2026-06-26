using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimaFlow.Api.Models;

[Table("vias")] // El nombre de la tabla en Postgres
public class Via
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la vía es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El zonaid es obligatorio.")]
    public int ZonaId { get; set; }
}