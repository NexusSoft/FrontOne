using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class PaisRepository : SqlRepositoryBase, IPaisRepository
{
    public PaisRepository(IConnectionFactory connectionFactory, ILogger<PaisRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Pais>> ObtenerAsync(int? id = null)
        => QueryAsync<Pais>("Catalogos.sp_Pais_Obtener", new { Id = id });

    public Task<int> InsertarAsync(Pais pais)
        => ExecuteScalarAsync<int>("Catalogos.sp_Pais_Insertar", new { pais.Clave, pais.Nombre })!;

    public Task ActualizarAsync(Pais pais)
        => ExecuteAsync("Catalogos.sp_Pais_Actualizar", new { pais.Id, pais.Clave, pais.Nombre, pais.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Pais_Eliminar", new { Id = id });
}
