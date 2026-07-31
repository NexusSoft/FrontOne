using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class LoteRepository : SqlRepositoryBase, ILoteRepository
{
    private record InsertResult(int Id, string Folio);

    public LoteRepository(IConnectionFactory connectionFactory, ILogger<LoteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Lote>> ObtenerAsync(int? id = null)
        => QueryAsync<Lote>("Lotes.sp_Lote_Obtener", new { Id = id });

    public async Task<(int Id, string Folio)> InsertarAsync(Lote lote)
    {
        // Referencia se deja en NULL en el insert (todavía no se conoce el Folio que la
        // fórmula juliana necesita) y se completa con un UPDATE inmediato — ver LoteService.
        var resultado = await QueryFirstAsync<InsertResult>("Lotes.sp_Lote_Insertar", new
        {
            lote.Fecha,
            Referencia = string.IsNullOrEmpty(lote.Referencia) ? null : lote.Referencia,
            lote.Observaciones,
            lote.Kilogramos,
            lote.Personalizado,
            lote.LineaProduccionId,
            lote.PorcentajeMateriaSeca,
            lote.Estatus,
        });

        return (resultado!.Id, resultado.Folio);
    }

    public Task ActualizarAsync(Lote lote)
        => ExecuteAsync("Lotes.sp_Lote_Actualizar", new
        {
            lote.Id,
            lote.Fecha,
            lote.Referencia,
            lote.Observaciones,
            lote.Kilogramos,
            lote.Personalizado,
            lote.LineaProduccionId,
            lote.PorcentajeMateriaSeca,
            lote.Estatus,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Lotes.sp_Lote_Eliminar", new { Id = id });

    public Task<IReadOnlyList<LoteRecepcion>> ObtenerDetalleAsync(int loteId)
        => QueryAsync<LoteRecepcion>("Lotes.sp_LoteRecepcion_Obtener", new { LoteId = loteId });

    public async Task<int> InsertarDetalleAsync(int loteId, int recepcionFrutaId)
        => await QueryFirstAsync<int>("Lotes.sp_LoteRecepcion_Insertar", new { LoteId = loteId, RecepcionFrutaId = recepcionFrutaId });

    public Task<int?> EliminarDetalleAsync(int id)
        => QueryFirstAsync<int?>("Lotes.sp_LoteRecepcion_Eliminar", new { Id = id });

    public async Task<bool> RecepcionEstaEnLoteAsync(int recepcionFrutaId)
        => await QueryFirstAsync<bool>("Lotes.sp_RecepcionFruta_EstaEnLote", new { RecepcionFrutaId = recepcionFrutaId });
}
