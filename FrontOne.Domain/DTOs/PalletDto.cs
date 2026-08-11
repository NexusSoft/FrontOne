namespace FrontOne.Domain.DTOs;

public record PalletDto(
    int Id,
    string Folio,
    DateTime FechaCreacion,
    TimeSpan HoraCreacion,
    byte Estatus,
    int LineaProduccionId,
    string LineaProduccionNombre,
    bool EsMixto,
    int? ProductoTerminadoId,
    decimal PorcentajeMateriaSeca,
    decimal? PesoReal,
    bool Bloqueado,
    DateTime? FechaBloqueo,
    string? NoReempaque,
    bool PrimeraCorrida,
    bool EsNeutro,
    int TotalCajas,
    decimal TotalKilogramos,
    string ProductoDescripcion,
    string ProductoCodigoSap,
    DateTime FechaCreacionRegistro);
