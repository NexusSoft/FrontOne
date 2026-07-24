namespace FrontOne.Domain.Entities;

public class AcuerdoCorte
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public int ProductorId { get; set; }
    public string ProductorNombre { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public int VariedadId { get; set; }
    public string VariedadNombre { get; set; } = string.Empty;
    public int TipoComercializacionId { get; set; }
    public string TipoComercializacionNombre { get; set; } = string.Empty;
    public int TipoCorteId { get; set; }
    public string TipoCorteNombre { get; set; } = string.Empty;
    // Exactamente uno de los dos según si el TipoCorte elegido necesita lista de precios.
    public decimal? Precio { get; set; }
    public DateTime? ListaPrecioFecha { get; set; }
    public int? ListaPrecioProductorId { get; set; }
    public string? ListaPrecioProductorNombre { get; set; }
    // Cuál de las 3 columnas de precio (Lista1/2/3) de la vigencia aplica a este acuerdo.
    public int? ListaPrecioNumero { get; set; }
    public int MonedaId { get; set; }
    public string MonedaNombre { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
}
