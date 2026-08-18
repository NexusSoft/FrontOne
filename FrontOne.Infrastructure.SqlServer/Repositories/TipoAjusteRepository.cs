using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class TipoAjusteRepository : SqlRepositoryBase, ITipoAjusteRepository
{
    public TipoAjusteRepository(IConnectionFactory connectionFactory, ILogger<TipoAjusteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<TipoAjuste>> ObtenerAsync(int? id = null)
        => QueryAsync<TipoAjuste>("Gastos.sp_TipoAjuste_Obtener", new { Id = id });

    public Task<int> InsertarAsync(TipoAjuste tipoAjuste)
        => ExecuteScalarAsync<int>("Gastos.sp_TipoAjuste_Insertar", new { tipoAjuste.Nombre, tipoAjuste.TipoGasto, tipoAjuste.Signo })!;

    public Task ActualizarAsync(TipoAjuste tipoAjuste)
        => ExecuteAsync("Gastos.sp_TipoAjuste_Actualizar", new { tipoAjuste.Id, tipoAjuste.Nombre, tipoAjuste.TipoGasto, tipoAjuste.Signo, tipoAjuste.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Gastos.sp_TipoAjuste_Eliminar", new { Id = id });
}
