namespace FrontOne.Domain.Entities;

public class ReportePermiso
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public string ReporteCodigo { get; set; } = string.Empty;
    public bool VistaPrevia { get; set; }
    public bool Impresion { get; set; }
    public bool Exportacion { get; set; }
    public bool Diseno { get; set; }
}
