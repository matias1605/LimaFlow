namespace LimaFlow.Api.DTOs;

public class IncidenciaDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string NombreVia { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public string Estado { get; set; } = string.Empty;
}