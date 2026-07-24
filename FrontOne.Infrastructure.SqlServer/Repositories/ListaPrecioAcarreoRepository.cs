using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ListaPrecioAcarreoRepository : SqlRepositoryBase, IListaPrecioAcarreoRepository
{
    public ListaPrecioAcarreoRepository(IConnectionFactory connectionFactory, ILogger<ListaPrecioAcarreoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<ListaPrecioAcarreo>> ObtenerAsync(int? id = null)
        => QueryAsync<ListaPrecioAcarreo>("Acarreo.sp_ListaPrecioAcarreo_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(ListaPrecioAcarreo lista)
        => await QueryFirstAsync<int>("Acarreo.sp_ListaPrecioAcarreo_Insertar", new
        {
            lista.MunicipioId,
            lista.ZonaId,
            lista.Precio300,
            lista.Precio400,
            lista.Precio500,
            lista.ToleranciaKgFaltante,
            lista.Activo,
        });

    public Task ActualizarAsync(ListaPrecioAcarreo lista)
        => ExecuteAsync("Acarreo.sp_ListaPrecioAcarreo_Actualizar", new
        {
            lista.Id,
            lista.MunicipioId,
            lista.ZonaId,
            lista.Precio300,
            lista.Precio400,
            lista.Precio500,
            lista.ToleranciaKgFaltante,
            lista.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acarreo.sp_ListaPrecioAcarreo_Eliminar", new { Id = id });
}
