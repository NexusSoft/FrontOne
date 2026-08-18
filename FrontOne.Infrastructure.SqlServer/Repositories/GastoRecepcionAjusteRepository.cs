using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class GastoRecepcionAjusteRepository : SqlRepositoryBase, IGastoRecepcionAjusteRepository
{
    public GastoRecepcionAjusteRepository(IConnectionFactory connectionFactory, ILogger<GastoRecepcionAjusteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<GastoRecepcionAjusteDto>> ObtenerAsync(int gastoLoteId, byte tipoGasto)
        => QueryAsync<GastoRecepcionAjusteDto>("Gastos.sp_GastoRecepcionAjuste_Obtener", new { GastoLoteId = gastoLoteId, TipoGasto = tipoGasto });

    public Task<int> InsertarAsync(GastoRecepcionAjuste ajuste)
        => ExecuteScalarAsync<int>("Gastos.sp_GastoRecepcionAjuste_Insertar", new
        {
            ajuste.GastoLoteId,
            ajuste.LoteRecepcionId,
            ajuste.TipoAjusteId,
            ajuste.Monto,
            ajuste.CargoA,
        })!;

    public Task ActualizarAsync(GastoRecepcionAjuste ajuste)
        => ExecuteAsync("Gastos.sp_GastoRecepcionAjuste_Actualizar", new { ajuste.Id, ajuste.TipoAjusteId, ajuste.Monto, ajuste.CargoA });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Gastos.sp_GastoRecepcionAjuste_Eliminar", new { Id = id });
}
