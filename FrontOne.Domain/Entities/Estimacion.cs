namespace FrontOne.Domain.Entities;

public class Estimacion
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int HuertaId { get; set; }
    public string HuertaNombre { get; set; } = string.Empty;
    public string? RegistroSagarpa { get; set; }
    public decimal Kilos { get; set; }
    public int? AcopiadorId { get; set; }
    public string? AcopiadorNombre { get; set; }
    public decimal PorcentajeCat1 { get; set; }
    public decimal PorcentajeCat2 { get; set; }
    public decimal PorcentajeNal { get; set; }
    public decimal PorcentajeCalibre32 { get; set; }
    public decimal PorcentajeCalibre36 { get; set; }
    public decimal PorcentajeCalibre40 { get; set; }
    public decimal PorcentajeCalibre48 { get; set; }
    public decimal PorcentajeCalibre60 { get; set; }
    public decimal PorcentajeCalibre70 { get; set; }
    public decimal PorcentajeCalibre84 { get; set; }
    public decimal PorcentajeCalibre90 { get; set; }
    public decimal PorcentajeBorona { get; set; }
    public decimal PorcentajeCanica { get; set; }
    public decimal PorcentajeCuarta { get; set; }
    public decimal PorcentajeDesecho { get; set; }
    public decimal PorcentajeProceso { get; set; }
    public DateTime ListaPrecioFecha { get; set; }
    public int? ListaPrecioProductorId { get; set; }
    public byte TipoLista { get; set; }
    public decimal PrecioSugerido { get; set; }
    public bool Cerrada { get; set; }
    public DateTime FechaCreacion { get; set; }
}
