using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class TipoPagoRepository : SqlRepositoryBase, ITipoPagoRepository
{
    public TipoPagoRepository(IConnectionFactory connectionFactory, ILogger<TipoPagoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<TipoPago>> ObtenerAsync(int? id = null)
        => QueryAsync<TipoPago>("Acopio.sp_TipoPago_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(TipoPago tipoPago)
        => await QueryFirstAsync<int>("Acopio.sp_TipoPago_Insertar", new { tipoPago.Nombre, tipoPago.NecesitaListaPrecios, tipoPago.Activo });

    public Task ActualizarAsync(TipoPago tipoPago)
        => ExecuteAsync("Acopio.sp_TipoPago_Actualizar", new { tipoPago.Id, tipoPago.Nombre, tipoPago.NecesitaListaPrecios, tipoPago.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_TipoPago_Eliminar", new { Id = id });
}
