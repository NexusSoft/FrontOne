namespace FrontOne.Domain.Entities;

public class OrdenCorte
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int AcuerdoCorteId { get; set; }
    public string AcuerdoCorteFolio { get; set; } = string.Empty;
    public int ProductorId { get; set; }
    public string ProductorNombre { get; set; } = string.Empty;
    public int HuertaId { get; set; }
    public string HuertaNombre { get; set; } = string.Empty;
    public int FloracionId { get; set; }
    public string FloracionNombre { get; set; } = string.Empty;
    public string? RegistroSagarpa { get; set; }
    public int VariedadId { get; set; }
    public string VariedadNombre { get; set; } = string.Empty;
    public string TipoCorteNombre { get; set; } = string.Empty;
    public string TipoPagoNombre { get; set; } = string.Empty;
    public string PagarCorteACardCode { get; set; } = string.Empty;
    public string PagarCorteANombre { get; set; } = string.Empty;
    public string TransportistaCardCode { get; set; } = string.Empty;
    public string TransportistaNombre { get; set; } = string.Empty;
    public decimal PrecioAcarreo { get; set; }
    public string? NoCandado { get; set; }
    public int CajasEntregadas { get; set; }
    public string JefeCuadrillaCardCode { get; set; } = string.Empty;
    public string JefeCuadrillaNombre { get; set; } = string.Empty;
    public decimal CostoKg { get; set; }
    public decimal PagoDia { get; set; }
    public decimal CuadrillaApoyo { get; set; }
    public decimal KgMinimo { get; set; }
    public int JefeAcopioId { get; set; }
    public string JefeAcopioNombre { get; set; } = string.Empty;
    public string? PuntoReunion { get; set; }
    public string? Observaciones { get; set; }
    public bool Cancelado { get; set; }
    public int? CajaCampoId { get; set; }
    public string? CajaCampoNombre { get; set; }

    // true si ya está referenciada en Recepcion.RecepcionFrutaOrdenCorte — a partir de ahí se
    // bloquea su edición (ver OrdenCorteService.ActualizarAsync), mismo criterio que
    // RecepcionFruta.EstaEnLote.
    public bool EstaEnRecepcion { get; set; }
    public DateTime FechaCreacion { get; set; }
}
