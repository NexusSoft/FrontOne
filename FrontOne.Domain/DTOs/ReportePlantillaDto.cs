namespace FrontOne.Domain.DTOs;

public record ReportePlantillaDto(
    int Id,
    string Codigo,
    string Nombre,
    string? DefinicionXml,
    bool EsPredeterminado,
    DateTime FechaModificacion);
