namespace FrontOne.Domain.DTOs;

// Shape del picker de Lote en la captura de una línea de Pallet: solo Lotes cuya Corrida sigue
// En Proceso, con el saldo disponible ya calculado (KilosAProcesar − KilosProcesados) y los
// datos de Huerta/Sagarpa/Productor autocompletables sin otra llamada al servidor.
public record LoteEnProcesoParaPalletDto(
    int CorridaId,
    int LoteId,
    string LoteFolio,
    string CodigoTrazabilidad,
    int LineaProduccionId,
    decimal PorcentajeMateriaSeca,
    decimal KilosAProcesar,
    decimal KilosProcesados,
    decimal KilosDisponibles,
    string? HuertaNombre,
    string? RegistroSagarpa,
    string? ProductorNombre);
