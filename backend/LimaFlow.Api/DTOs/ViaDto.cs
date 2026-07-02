namespace LimaFlow.Api.DTOs;

public class ViaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string NombreZona { get; set; } = string.Empty; // Aquí llegará el nombre de la zona
}