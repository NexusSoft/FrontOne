namespace FrontOne.Domain.DTOs;

public record ReportePermisoFilaDto(
    string Codigo,
    string Nombre,
    bool VistaPrevia,
    bool Impresion,
    bool Exportacion,
    bool Diseno);
