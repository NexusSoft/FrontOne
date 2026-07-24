using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class SistemaRiegoRepository : SqlRepositoryBase, ISistemaRiegoRepository
{
    public SistemaRiegoRepository(IConnectionFactory connectionFactory, ILogger<SistemaRiegoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<SistemaRiego>> ObtenerAsync(int? id = null)
        => QueryAsync<SistemaRiego>("Catalogos.sp_SistemaRiego_Obtener", new { Id = id });

    public Task<int> InsertarAsync(SistemaRiego sistemaRiego)
        => ExecuteScalarAsync<int>("Catalogos.sp_SistemaRiego_Insertar", new { sistemaRiego.Nombre, sistemaRiego.Activo })!;

    public Task ActualizarAsync(SistemaRiego sistemaRiego)
        => ExecuteAsync("Catalogos.sp_SistemaRiego_Actualizar", new { sistemaRiego.Id, sistemaRiego.Nombre, sistemaRiego.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_SistemaRiego_Eliminar", new { Id = id });
}
