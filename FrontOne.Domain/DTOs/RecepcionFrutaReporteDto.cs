namespace FrontOne.Domain.DTOs;

// Datos para el reporte "Recepción de Fruta" — un solo registro ancho (encabezado + datos de la
// Orden de Corte/Acuerdo/Producto), porque la regla de negocio limita a una sola Orden de Corte
// por Recepción.
public record RecepcionFrutaReporteDto(
    int Id,
    string Folio,
    string? NoLote,
    DateTime Fecha,
    string Chofer,
    string? Placas,
    string? Observaciones,
    string? NumeroTicket,
    string? CoprefBico,
    decimal PesoBruto,
    decimal PesoTara,
    decimal TaraCajas,
    decimal PesoMuestra,
    decimal PesoNeto,
    decimal PesoProductor,
    decimal PorcentajeMateriaSeca,
    short CajasPorEntregar,
    short CajasEntregadas,
    short CajasCortadas,
    short CajasRecibidasVacias,
    short CajasDiferencia,
    bool CamionDestarado,
    string HuertaNombre,
    string ProductorNombre,
    string TipoCorteNombre,
    string AcuerdoCorteFolio,
    string TransportistaNombre,
    string EmpresaCorteNombre,
    string? NoCandado,
    string? OrdenObservaciones,
    string ProductoNombre,
    string VariedadNombre,
    decimal Kilogramos);
