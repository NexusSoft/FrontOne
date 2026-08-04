using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class CajaCampoRepository : SqlRepositoryBase, ICajaCampoRepository
{
    public CajaCampoRepository(IConnectionFactory connectionFactory, ILogger<CajaCampoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<CajaCampo>> ObtenerAsync(int? id = null)
        => QueryAsync<CajaCampo>("Catalogos.sp_CajaCampo_Obtener", new { Id = id });

    public Task<int> InsertarAsync(CajaCampo cajaCampo)
        => ExecuteScalarAsync<int>("Catalogos.sp_CajaCampo_Insertar", new { cajaCampo.Nombre })!;

    public Task ActualizarAsync(CajaCampo cajaCampo)
        => ExecuteAsync("Catalogos.sp_CajaCampo_Actualizar", new { cajaCampo.Id, cajaCampo.Nombre, cajaCampo.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_CajaCampo_Eliminar", new { Id = id });
}
