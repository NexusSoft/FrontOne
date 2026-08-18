namespace FrontOne.Domain.DTOs;

// Une la fila base (no borrable) y los ajustes capturados de una pestaña (Cosecha o Acarreo)
// del Lote, con los totales Con cargo a Empresa / Con cargo a Productor ya calculados.
public record GastoRecepcionResumenDto(
    IReadOnlyList<GastoRecepcionBaseDto> Base,
    IReadOnlyList<GastoRecepcionAjusteDto> Ajustes,
    decimal TotalEmpresa,
    decimal TotalProductor);
