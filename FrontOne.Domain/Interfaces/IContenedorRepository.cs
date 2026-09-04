using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface IContenedorRepository
{
    Task<IReadOnlyList<ContenedorDto>> ObtenerAsync(int? id = null);

    Task<int> InsertarAsync(DateTime fecha, int sapDocEntry, int sapDocNum, string? folioFronterra, string cardCode, string cardName, string? observaciones);

    Task ActualizarAsync(int id, DateTime fecha, string? observaciones);

    Task EliminarAsync(int id);

    Task<IReadOnlyList<ContenedorPalletDto>> ObtenerPalletsAsync(int contenedorId);

    Task<IReadOnlyList<ContenedorResumenCalibreDto>> ObtenerResumenAsync(int contenedorId);

    Task<IReadOnlyList<ContenedorSurtidoDto>> ObtenerSurtidoAsync(int contenedorId);

    // codigosSapPermitidos: CSV de CodigoSap; NULL/vacío = sin filtro de producto (buscador genérico).
    Task<IReadOnlyList<PalletDisponibleEmbarqueDto>> ObtenerPalletsDisponiblesAsync(string? folio = null, string? codigosSapPermitidos = null);

    Task AgregarPalletAsync(int contenedorId, int palletId, int posicion, decimal? temperatura);

    Task QuitarPalletAsync(int contenedorPalletId);
}
