using FrontOne.Domain.Enums;

namespace FrontOne.Domain.DTOs;

public record ProductoTerminadoDto(
    int Id,
    string CodigoSap,
    string DescripcionSap,
    string? DescripcionExtranjeraSap,
    bool Activo,
    string? CodigoUpc,
    string? CodigoPlu,
    string? CodigoGtin,
    string? MateriaPrimaItemCode,
    int? CategoriaId,
    int? TipoProductoId,
    int? CalibreApeamId,
    string? CalibreCodigoExterno,
    int? MercadoDestinoPaisId,
    int? MarcaId,
    int? VariedadId,
    int? PesoEstandarId,
    decimal? PesoNeto,
    decimal? PesoPromedio,
    int? CajasPorPallet,
    PresentacionProducto Presentacion,
    DateTime FechaCreacion)
{
    // Categoría es obligatoria en el validador junto con el resto de los campos de negocio
    // (Tipo de Producto, Calibre, Mercado Destino, Marca, Variedad) — basta con revisarla para
    // saber si el producto ya se completó tras la sincronización con SAP.
    public bool DatosCapturados => CategoriaId is not null;
}
