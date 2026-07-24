using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class StatusHuertaRepository : SqlRepositoryBase, IStatusHuertaRepository
{
    public StatusHuertaRepository(IConnectionFactory connectionFactory, ILogger<StatusHuertaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<StatusHuerta>> ObtenerAsync(int? id = null)
        => QueryAsync<StatusHuerta>("Catalogos.sp_StatusHuerta_Obtener", new { Id = id });

    public Task<int> InsertarAsync(StatusHuerta statusHuerta)
        => ExecuteScalarAsync<int>("Catalogos.sp_StatusHuerta_Insertar", new { statusHuerta.Nombre, statusHuerta.Activo })!;

    public Task ActualizarAsync(StatusHuerta statusHuerta)
        => ExecuteAsync("Catalogos.sp_StatusHuerta_Actualizar", new { statusHuerta.Id, statusHuerta.Nombre, statusHuerta.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_StatusHuerta_Eliminar", new { Id = id });
}
