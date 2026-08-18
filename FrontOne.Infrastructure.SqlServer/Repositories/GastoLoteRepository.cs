using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class GastoLoteRepository : SqlRepositoryBase, IGastoLoteRepository
{
    public GastoLoteRepository(IConnectionFactory connectionFactory, ILogger<GastoLoteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<GastoLoteListadoDto>> ObtenerLotesCosteablesAsync()
        => QueryAsync<GastoLoteListadoDto>("Gastos.sp_GastoLote_ObtenerLotesCosteables");

    public Task<GastoLoteEncabezado?> ObtenerEncabezadoAsync(int loteId)
        => QueryFirstAsync<GastoLoteEncabezado>("Gastos.sp_GastoLote_ObtenerEncabezado", new { LoteId = loteId });

    public Task<int> ObtenerOCrearAsync(int loteId)
        => ExecuteScalarAsync<int>("Gastos.sp_GastoLote_ObtenerOCrear", new { LoteId = loteId })!;

    public Task<GastoLote?> ObtenerPorIdAsync(int id)
        => QueryFirstAsync<GastoLote>("Gastos.sp_GastoLote_ObtenerPorId", new { Id = id });

    public Task ActualizarVigenciaEstimadoAsync(GastoLote gastoLote)
        => ExecuteAsync("Gastos.sp_GastoLote_ActualizarVigenciaEstimado", new
        {
            gastoLote.Id,
            gastoLote.CostoEstimadoListaPrecioFecha,
            gastoLote.CostoEstimadoListaPrecioProductorId,
            gastoLote.CostoEstimadoListaPrecioNumero,
        });

    public Task<GastoLoteReporteDto?> ObtenerParaReporteAsync(int gastoLoteId)
        => QueryFirstAsync<GastoLoteReporteDto>("Gastos.sp_GastoLote_ObtenerParaReporte", new { GastoLoteId = gastoLoteId });
}
