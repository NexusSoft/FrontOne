using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class GastoRecepcionRepository : SqlRepositoryBase, IGastoRecepcionRepository
{
    private record PrecioCorteResult(decimal PrecioUnitario, decimal Importe);

    public GastoRecepcionRepository(IConnectionFactory connectionFactory, ILogger<GastoRecepcionRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<GastoRecepcionBaseDto>> ObtenerBaseAsync(int gastoLoteId, byte tipoGasto)
        => QueryAsync<GastoRecepcionBaseDto>("Gastos.sp_GastoRecepcion_ObtenerBase", new { GastoLoteId = gastoLoteId, TipoGasto = tipoGasto });

    public Task ActualizarCargoAAsync(int id, byte cargoA)
        => ExecuteAsync("Gastos.sp_GastoRecepcion_ActualizarCargoA", new { Id = id, CargoA = cargoA });

    public async Task<(decimal PrecioUnitario, decimal Importe)> ActualizarPrecioCorteAsync(int gastoRecepcionId)
    {
        var resultado = await QueryFirstAsync<PrecioCorteResult>("Gastos.sp_GastoRecepcion_ActualizarPrecioCorte", new { GastoRecepcionId = gastoRecepcionId });
        return (resultado!.PrecioUnitario, resultado.Importe);
    }

    public async Task<(decimal PrecioUnitario, decimal Importe)> ActualizarPrecioAcarreoAsync(int gastoRecepcionId)
    {
        var resultado = await QueryFirstAsync<PrecioCorteResult>("Gastos.sp_GastoRecepcion_ActualizarPrecioAcarreo", new { GastoRecepcionId = gastoRecepcionId });
        return (resultado!.PrecioUnitario, resultado.Importe);
    }

    public Task<IReadOnlyList<RelacionGastoDto>> ObtenerParaReporteAsync(int gastoLoteId)
        => QueryAsync<RelacionGastoDto>("Gastos.sp_GastoRecepcion_ObtenerParaReporte", new { GastoLoteId = gastoLoteId });
}
