using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ZonaRepository : SqlRepositoryBase, IZonaRepository
{
    public ZonaRepository(IConnectionFactory connectionFactory, ILogger<ZonaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Zona>> ObtenerAsync(int? id = null)
        => QueryAsync<Zona>("Acarreo.sp_Zona_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(Zona zona)
        => await QueryFirstAsync<int>("Acarreo.sp_Zona_Insertar", new
        {
            zona.Nombre,
            zona.KgMinimo300,
            zona.KgMinimo400,
            zona.KgMinimo500,
            zona.Activo,
        });

    public Task ActualizarAsync(Zona zona)
        => ExecuteAsync("Acarreo.sp_Zona_Actualizar", new
        {
            zona.Id,
            zona.Nombre,
            zona.KgMinimo300,
            zona.KgMinimo400,
            zona.KgMinimo500,
            zona.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acarreo.sp_Zona_Eliminar", new { Id = id });
}
