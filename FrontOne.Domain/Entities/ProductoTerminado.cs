namespace FrontOne.Domain.Entities;

// Las FKs de catálogo (CategoriaId, TipoProductoId, CalibreApeamId, MercadoDestinoPaisId,
// MarcaId, VariedadId, PesoEstandarId) y los campos de negocio derivados (PesoNeto,
// PesoPromedio, CajasPorPallet) son NULLABLE a propósito: un Producto Terminado nace
// únicamente de la sincronización con SAP (SincronizarConSapAsync), momento en el que
// todavía no existe ninguna captura de negocio en FrontOne. El usuario completa esos
// campos después, editando el registro desde ProductoTerminadoEditarForm.
public class ProductoTerminado
{
    public int Id { get; set; }
    public string CodigoSap { get; set; } = string.Empty;
    public string DescripcionSap { get; set; } = string.Empty;
    public string? DescripcionExtranjeraSap { get; set; }
    public bool Activo { get; set; }
    public string? CodigoUpc { get; set; }
    public string? CodigoPlu { get; set; }
    public string? CodigoGtin { get; set; }
    public string? MateriaPrimaItemCode { get; set; }
    public int? CategoriaId { get; set; }
    public int? TipoProductoId { get; set; }
    public int? CalibreApeamId { get; set; }
    public string? CalibreCodigoExterno { get; set; }
    public int? MercadoDestinoPaisId { get; set; }
    public int? MarcaId { get; set; }
    public int? VariedadId { get; set; }
    public int? PesoEstandarId { get; set; }
    public decimal? PesoNeto { get; set; }
    public decimal? PesoPromedio { get; set; }
    public int? CajasPorPallet { get; set; }
    public DateTime FechaCreacion { get; set; }
}
