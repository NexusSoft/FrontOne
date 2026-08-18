using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class GastoFrutaCategoriaRepository : SqlRepositoryBase, IGastoFrutaCategoriaRepository
{
    public GastoFrutaCategoriaRepository(IConnectionFactory connectionFactory, ILogger<GastoFrutaCategoriaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<GastoFrutaCategoriaLineaDto>> ObtenerAsync(int gastoLoteId)
        => QueryAsync<GastoFrutaCategoriaLineaDto>("Gastos.sp_GastoFrutaCategoria_Obtener", new { GastoLoteId = gastoLoteId });

    public Task<IReadOnlyList<ResumenMercadoDto>> ObtenerResumenMercadoAsync(int gastoLoteId)
        => QueryAsync<ResumenMercadoDto>("Gastos.sp_GastoFrutaCategoria_ObtenerResumenMercado", new { GastoLoteId = gastoLoteId });

    public Task GuardarCostoRealAsync(int gastoLoteId, string materiaPrimaItemCode, decimal? costoRealUnitario)
        => ExecuteAsync("Gastos.sp_GastoFrutaCategoria_GuardarCostoReal",
            new { GastoLoteId = gastoLoteId, MateriaPrimaItemCode = materiaPrimaItemCode, CostoRealUnitario = costoRealUnitario });

    public Task GuardarCostoEstimadoAsync(int gastoLoteId, string materiaPrimaItemCode, decimal? costoEstimadoUnitario)
        => ExecuteAsync("Gastos.sp_GastoFrutaCategoria_GuardarCostoEstimado",
            new { GastoLoteId = gastoLoteId, MateriaPrimaItemCode = materiaPrimaItemCode, CostoEstimadoUnitario = costoEstimadoUnitario });
}
