using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ListaPrecioCorteRepository : SqlRepositoryBase, IListaPrecioCorteRepository
{
    public ListaPrecioCorteRepository(IConnectionFactory connectionFactory, ILogger<ListaPrecioCorteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<ListaPrecioCorte>> ObtenerAsync(int? id = null)
        => QueryAsync<ListaPrecioCorte>("Acopio.sp_ListaPrecioCorte_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(ListaPrecioCorte lista)
        => await QueryFirstAsync<int>("Acopio.sp_ListaPrecioCorte_Insertar", new
        {
            lista.CardCode,
            lista.CardName,
            lista.PrecioKg,
            lista.PrecioDia,
            lista.CuadrillaApoyo,
            lista.Activo,
        });

    public Task ActualizarAsync(ListaPrecioCorte lista)
        => ExecuteAsync("Acopio.sp_ListaPrecioCorte_Actualizar", new
        {
            lista.Id,
            lista.PrecioKg,
            lista.PrecioDia,
            lista.CuadrillaApoyo,
        });

    public Task ActualizarNombreAsync(int id, string cardName)
        => ExecuteAsync("Acopio.sp_ListaPrecioCorte_ActualizarNombre", new { Id = id, CardName = cardName });

    public Task ActualizarActivoAsync(int id, bool activo)
        => ExecuteAsync("Acopio.sp_ListaPrecioCorte_ActualizarActivo", new { Id = id, Activo = activo });
}
