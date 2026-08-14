using FrontOne.Domain.Enums;

namespace FrontOne.Domain.DTOs;

public record EtiquetaDto(
    int Id,
    string Nombre,
    decimal AnchoPulgadas,
    decimal AltoPulgadas,
    TipoEtiqueta Tipo,
    string? DefinicionXml,
    bool Activo,
    DateTime FechaCreacion,
    DateTime FechaModificacion);
