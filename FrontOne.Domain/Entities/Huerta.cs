namespace FrontOne.Domain.Entities;

public class Huerta
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int ProductorId { get; set; }
    public string? Ubicacion { get; set; }
    public int? PoblacionId { get; set; }
    public int? MunicipioId { get; set; }
    public int? EstadoId { get; set; }
    public int? ProductoId { get; set; }
    public string? EncargadoNombre { get; set; }
    public string? EncargadoTelefono { get; set; }
    public string? Observaciones { get; set; }
    public string? RegistroSagarpa { get; set; }
    public bool CertificadoGlobalGap { get; set; }
    public string? RegistroFda { get; set; }
    public string? NumeroGlobalGap { get; set; }
    public decimal? Superficie { get; set; }
    public decimal? Altura { get; set; }
    public int? NumeroArboles { get; set; }
    public int? EdadArboles { get; set; }
    public int? SistemaRiegoId { get; set; }
    public decimal? PorcentajeMecanizacion { get; set; }
    public int? StatusHuertaId { get; set; }
    public DateTime? FechaCambioStatus { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public bool Activo { get; set; }
    public decimal? AniosEnStatusActual { get; set; }
    public int? DiasParaVencimiento { get; set; }
}
