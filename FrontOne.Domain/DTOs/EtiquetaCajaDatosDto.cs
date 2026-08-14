namespace FrontOne.Domain.DTOs;

// Nombres de propiedad calcan exactos los alias de Produccion.sp_Pallet_ObtenerEtiquetaCaja, para
// que el binding declarativo del Diseñador ([Campo]) apunte sin traducción.
public record EtiquetaCajaDatosDto(
    string NoPallet,
    DateTime FechaPallet,
    DateTime FechaEmpacado,
    string? NumeroEmpaque,
    string NoLote,
    string? CodigoTrazabilidad,
    DateTime FechaLote,
    DateTime? FechaOrdenCorteMax,
    decimal MateriaSecaLote,
    string? Productor,
    string? Huerta,
    string? RegistroSagarpa,
    string? RegistroGgn,
    string? Municipio,
    string NombreProductoTerminado,
    string? TipoProducto,
    string? Categoria,
    string? CalibreApeam,
    string? MercadoDestino,
    string? Marca,
    string? Variedad,
    decimal? PesoEstandar,
    string? CodigoCalibreExterno,
    string? CodigoUpc,
    string? CodigoPlu,
    string? CodigoGtin,
    string? CodigoGs1128,
    string? VoiceCodeLow,
    string? VoiceCodeHigh);
