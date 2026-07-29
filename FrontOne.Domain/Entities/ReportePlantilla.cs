namespace FrontOne.Domain.Entities;

public class ReportePlantilla
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? DefinicionXml { get; set; }
    public DateTime FechaModificacion { get; set; }
}
