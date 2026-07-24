using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class FloracionRepository : SqlRepositoryBase, IFloracionRepository
{
    public FloracionRepository(IConnectionFactory connectionFactory, ILogger<FloracionRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Floracion>> ObtenerAsync(int? id = null)
        => QueryAsync<Floracion>("Acopio.sp_Floracion_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(Floracion floracion)
        => await QueryFirstAsync<int>("Acopio.sp_Floracion_Insertar", new { floracion.Nombre, floracion.Activo });

    public Task ActualizarAsync(Floracion floracion)
        => ExecuteAsync("Acopio.sp_Floracion_Actualizar", new { floracion.Id, floracion.Nombre, floracion.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_Floracion_Eliminar", new { Id = id });
}
