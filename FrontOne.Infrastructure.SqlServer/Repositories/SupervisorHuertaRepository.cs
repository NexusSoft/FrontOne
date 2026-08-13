using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class SupervisorHuertaRepository : SqlRepositoryBase, ISupervisorHuertaRepository
{
    public SupervisorHuertaRepository(IConnectionFactory connectionFactory, ILogger<SupervisorHuertaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<SupervisorHuerta>> ObtenerAsync(int? id = null)
        => QueryAsync<SupervisorHuerta>("Acopio.sp_SupervisorHuerta_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(SupervisorHuerta supervisorHuerta)
        => await QueryFirstAsync<int>("Acopio.sp_SupervisorHuerta_Insertar", new { supervisorHuerta.Nombre, supervisorHuerta.Activo });

    public Task ActualizarAsync(SupervisorHuerta supervisorHuerta)
        => ExecuteAsync("Acopio.sp_SupervisorHuerta_Actualizar", new { supervisorHuerta.Id, supervisorHuerta.Nombre, supervisorHuerta.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_SupervisorHuerta_Eliminar", new { Id = id });
}
