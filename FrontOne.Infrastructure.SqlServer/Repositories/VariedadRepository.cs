using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class VariedadRepository : SqlRepositoryBase, IVariedadRepository
{
    public VariedadRepository(IConnectionFactory connectionFactory, ILogger<VariedadRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Variedad>> ObtenerAsync(int? id = null)
        => QueryAsync<Variedad>("Acopio.sp_Variedad_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(Variedad variedad)
        => await QueryFirstAsync<int>("Acopio.sp_Variedad_Insertar", new { variedad.Nombre, variedad.Activo });

    public Task ActualizarAsync(Variedad variedad)
        => ExecuteAsync("Acopio.sp_Variedad_Actualizar", new { variedad.Id, variedad.Nombre, variedad.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_Variedad_Eliminar", new { Id = id });
}
