using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class MunicipioRepository : SqlRepositoryBase, IMunicipioRepository
{
    public MunicipioRepository(IConnectionFactory connectionFactory, ILogger<MunicipioRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Municipio>> ObtenerAsync(int? estadoId = null, int? id = null)
        => QueryAsync<Municipio>("Catalogos.sp_Municipio_Obtener", new { EstadoId = estadoId, Id = id });

    public Task<int> InsertarAsync(Municipio municipio)
        => ExecuteScalarAsync<int>("Catalogos.sp_Municipio_Insertar", new { municipio.Nombre, municipio.EstadoId, municipio.Activo })!;

    public Task ActualizarAsync(Municipio municipio)
        => ExecuteAsync("Catalogos.sp_Municipio_Actualizar", new { municipio.Id, municipio.Nombre, municipio.EstadoId, municipio.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Municipio_Eliminar", new { Id = id });
}
