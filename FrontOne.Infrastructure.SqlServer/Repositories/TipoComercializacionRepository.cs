using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class TipoComercializacionRepository : SqlRepositoryBase, ITipoComercializacionRepository
{
    public TipoComercializacionRepository(IConnectionFactory connectionFactory, ILogger<TipoComercializacionRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<TipoComercializacion>> ObtenerAsync(int? id = null)
        => QueryAsync<TipoComercializacion>("Acopio.sp_TipoComercializacion_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(TipoComercializacion tipoComercializacion)
        => await QueryFirstAsync<int>("Acopio.sp_TipoComercializacion_Insertar", new { tipoComercializacion.Nombre, tipoComercializacion.Activo });

    public Task ActualizarAsync(TipoComercializacion tipoComercializacion)
        => ExecuteAsync("Acopio.sp_TipoComercializacion_Actualizar", new { tipoComercializacion.Id, tipoComercializacion.Nombre, tipoComercializacion.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_TipoComercializacion_Eliminar", new { Id = id });
}
