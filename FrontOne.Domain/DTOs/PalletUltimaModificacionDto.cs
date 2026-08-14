namespace FrontOne.Domain.DTOs;

// Huella ligera para saber si el listado de Pallets tiene cambios que el grid todavía no
// refleja — ver Produccion.sp_Pallet_ObtenerUltimaModificacion.
public record PalletUltimaModificacionDto(int Total, DateTime? UltimaModificacion);
