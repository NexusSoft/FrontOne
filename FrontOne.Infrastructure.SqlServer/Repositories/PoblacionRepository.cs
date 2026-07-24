using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class PoblacionRepository : SqlRepositoryBase, IPoblacionRepository
{
    public PoblacionRepository(IConnectionFactory connectionFactory, ILogger<PoblacionRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Poblacion>> ObtenerAsync(int? municipioId = null, int? id = null)
        => QueryAsync<Poblacion>("Catalogos.sp_Poblacion_Obtener", new { MunicipioId = municipioId, Id = id });

    public Task<int> InsertarAsync(Poblacion poblacion)
        => ExecuteScalarAsync<int>("Catalogos.sp_Poblacion_Insertar", new { poblacion.Nombre, poblacion.MunicipioId, poblacion.Activo })!;

    public Task ActualizarAsync(Poblacion poblacion)
        => ExecuteAsync("Catalogos.sp_Poblacion_Actualizar", new { poblacion.Id, poblacion.Nombre, poblacion.MunicipioId, poblacion.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Poblacion_Eliminar", new { Id = id });
}
