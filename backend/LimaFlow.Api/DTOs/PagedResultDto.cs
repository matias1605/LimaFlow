namespace LimaFlow.Api.DTOs;

public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    
    // Cálculo automático del total de páginas
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}