namespace FrontOne.Domain.DTOs;

// Activo mapea el campo "Valid" de SAP, que llega como cadena "tYES"/"tNO" en vez de booleano.
public record SapProductoTerminadoDto(string ItemCode, string ItemName, string? ForeignName, bool Activo);
