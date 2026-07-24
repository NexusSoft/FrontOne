using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class MonedaRepository : SqlRepositoryBase, IMonedaRepository
{
    public MonedaRepository(IConnectionFactory connectionFactory, ILogger<MonedaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Moneda>> ObtenerAsync(int? id = null)
        => QueryAsync<Moneda>("Acopio.sp_Moneda_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(Moneda moneda)
        => await QueryFirstAsync<int>("Acopio.sp_Moneda_Insertar", new { moneda.Nombre, moneda.Nomenclatura, moneda.Activo });

    public Task ActualizarAsync(Moneda moneda)
        => ExecuteAsync("Acopio.sp_Moneda_Actualizar", new { moneda.Id, moneda.Nombre, moneda.Nomenclatura, moneda.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_Moneda_Eliminar", new { Id = id });
}
