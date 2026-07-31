namespace FrontOne.Domain.DTOs;

public record ReportePermisoDto(
    string ReporteCodigo,
    bool VistaPrevia,
    bool Impresion,
    bool Exportacion,
    bool Diseno);
