namespace FrontOne.Domain.DTOs;

// Diferencia y TextoEstatus son calculados, no columnas persistidas — mismo criterio que
// CorridaDto.KilosRestantes.
public record LoteProduccionResumenDto(
    int Id,
    string Folio,
    DateTime Fecha,
    string? HuertaNombre,
    string? ProductorNombre,
    string? Beneficiario,
    decimal KilosRecibidos,
    decimal KilosProcesados,
    int Recepciones,
    decimal PorcentajeMateriaSeca,
    byte Estatus)
{
    public decimal Diferencia => KilosRecibidos - KilosProcesados;

    public string TextoEstatus => Estatus switch
    {
        1 => "En Proceso",
        2 => "Procesado",
        _ => "Sin Iniciar",
    };
}
