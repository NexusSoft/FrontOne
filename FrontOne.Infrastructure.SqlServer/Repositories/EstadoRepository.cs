using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class EstadoRepository : SqlRepositoryBase, IEstadoRepository
{
    public EstadoRepository(IConnectionFactory connectionFactory, ILogger<EstadoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Estado>> ObtenerAsync(int? paisId = null, int? id = null)
        => QueryAsync<Estado>("Catalogos.sp_Estado_Obtener", new { PaisId = paisId, Id = id });

    public Task<int> InsertarAsync(Estado estado)
        => ExecuteScalarAsync<int>("Catalogos.sp_Estado_Insertar", new { estado.PaisId, estado.Clave, estado.Nombre })!;

    public Task ActualizarAsync(Estado estado)
        => ExecuteAsync("Catalogos.sp_Estado_Actualizar", new { estado.Id, estado.PaisId, estado.Clave, estado.Nombre, estado.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Estado_Eliminar", new { Id = id });
}
