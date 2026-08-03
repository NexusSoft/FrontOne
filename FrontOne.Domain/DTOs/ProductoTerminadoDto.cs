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
    DateTime FechaCreacion);
