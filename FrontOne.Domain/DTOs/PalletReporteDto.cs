namespace FrontOne.Domain.DTOs;

// Encabezado de la Papeleta de Pallet. El detalle va aparte (PalletDetalleDto), igual que en el
// resto de reportes maestro-detalle del proyecto.
public record PalletReporteDto(
    int Id,
    string Folio,
    DateTime FechaCreacion,
    TimeSpan HoraCreacion,
    byte Estatus,
    string LineaProduccionNombre,
    bool EsMixto,
    decimal PorcentajeMateriaSeca,
    decimal? PesoReal,
    bool Bloqueado,
    string? NoReempaque,
    bool PrimeraCorrida,
    int TotalCajas,
    decimal TotalKilogramos);
