namespace FrontOne.Domain.DTOs;

// Datos crudos que regresan sp_PalletDetalle_Insertar/Actualizar (GTIN del Producto Terminado +
// CodigoTrazabilidad/Fecha del Lote ya resueltos por el SP) — el caller (PalletService) los usa
// para calcular el VoiceCode (VoicePickCodeCalculator) sin una segunda consulta a la base.
public record PalletDetalleInsertadoDto(int Id, string? CodigoGtin, string? CodigoTrazabilidad, DateTime? FechaLote);

public record PalletDetalleCodigosDto(string? CodigoGtin, string? CodigoTrazabilidad, DateTime? FechaLote);
