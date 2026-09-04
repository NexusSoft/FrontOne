using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ContenedorRepository : SqlRepositoryBase, IContenedorRepository
{
    public ContenedorRepository(IConnectionFactory connectionFactory, ILogger<ContenedorRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<ContenedorDto>> ObtenerAsync(int? id = null)
        => QueryAsync<ContenedorDto>("Embarques.sp_Contenedor_Obtener", new { Id = id });

    public Task<int> InsertarAsync(DateTime fecha, int sapDocEntry, int sapDocNum, string? folioFronterra, string cardCode, string cardName, string? observaciones)
        => ExecuteScalarAsync<int>("Embarques.sp_Contenedor_Insertar", new
        {
            Fecha = fecha,
            SapDocEntry = sapDocEntry,
            SapDocNum = sapDocNum,
            FolioFronterra = folioFronterra,
            CardCode = cardCode,
            CardName = cardName,
            Observaciones = observaciones,
        })!;

    public Task ActualizarAsync(int id, DateTime fecha, string? observaciones)
        => ExecuteAsync("Embarques.sp_Contenedor_Actualizar", new { Id = id, Fecha = fecha, Observaciones = observaciones });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Embarques.sp_Contenedor_Eliminar", new { Id = id });

    public Task<IReadOnlyList<ContenedorPalletDto>> ObtenerPalletsAsync(int contenedorId)
        => QueryAsync<ContenedorPalletDto>("Embarques.sp_Contenedor_ObtenerPallets", new { ContenedorId = contenedorId });

    public Task<IReadOnlyList<ContenedorResumenCalibreDto>> ObtenerResumenAsync(int contenedorId)
        => QueryAsync<ContenedorResumenCalibreDto>("Embarques.sp_Contenedor_ObtenerResumen", new { ContenedorId = contenedorId });

    public Task<IReadOnlyList<ContenedorSurtidoDto>> ObtenerSurtidoAsync(int contenedorId)
        => QueryAsync<ContenedorSurtidoDto>("Embarques.sp_Contenedor_ObtenerSurtido", new { ContenedorId = contenedorId });

    public Task<IReadOnlyList<PalletDisponibleEmbarqueDto>> ObtenerPalletsDisponiblesAsync(string? folio = null, string? codigosSapPermitidos = null)
        => QueryAsync<PalletDisponibleEmbarqueDto>("Embarques.sp_Contenedor_ObtenerPalletsDisponibles", new { Folio = folio, CodigosSap = codigosSapPermitidos });

    public Task AgregarPalletAsync(int contenedorId, int palletId, int posicion, decimal? temperatura)
        => ExecuteAsync("Embarques.sp_Contenedor_AgregarPallet", new
        {
            ContenedorId = contenedorId,
            PalletId = palletId,
            Posicion = posicion,
            Temperatura = temperatura,
        });

    public Task QuitarPalletAsync(int contenedorPalletId)
        => ExecuteAsync("Embarques.sp_Contenedor_QuitarPallet", new { ContenedorPalletId = contenedorPalletId });
}
