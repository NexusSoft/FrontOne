namespace FrontOne.Domain.DTOs;

// Fila unificada de la sección "Relación de Gastos" del Reporte de Proceso (Cosecha + Acarreo,
// base + ajustes). CXP = Con cargo a Empresa, CAP = Con cargo a Productor.
public record RelacionGastoDto(
    string TipoGasto,
    string? Proveedor,
    decimal Cantidad,
    decimal PrecioUnitario,
    decimal Importe,
    bool CXP,
    bool CAP);
