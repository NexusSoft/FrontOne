using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class CalibreApeamRepository : SqlRepositoryBase, ICalibreApeamRepository
{
    public CalibreApeamRepository(IConnectionFactory connectionFactory, ILogger<CalibreApeamRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<CalibreApeam>> ObtenerAsync(int? id = null)
        => QueryAsync<CalibreApeam>("Catalogos.sp_CalibreApeam_Obtener", new { Id = id });

    public Task<int> InsertarAsync(CalibreApeam calibreApeam)
        => ExecuteScalarAsync<int>("Catalogos.sp_CalibreApeam_Insertar", new { calibreApeam.Nombre })!;

    public Task ActualizarAsync(CalibreApeam calibreApeam)
        => ExecuteAsync("Catalogos.sp_CalibreApeam_Actualizar", new { calibreApeam.Id, calibreApeam.Nombre, calibreApeam.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_CalibreApeam_Eliminar", new { Id = id });
}
