using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class LineaProduccionRepository : SqlRepositoryBase, ILineaProduccionRepository
{
    public LineaProduccionRepository(IConnectionFactory connectionFactory, ILogger<LineaProduccionRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<LineaProduccion>> ObtenerAsync(int? id = null)
        => QueryAsync<LineaProduccion>("Catalogos.sp_LineaProduccion_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(LineaProduccion lineaProduccion)
        => await QueryFirstAsync<int>("Catalogos.sp_LineaProduccion_Insertar", new { lineaProduccion.Nombre, lineaProduccion.Activo });

    public Task ActualizarAsync(LineaProduccion lineaProduccion)
        => ExecuteAsync("Catalogos.sp_LineaProduccion_Actualizar", new { lineaProduccion.Id, lineaProduccion.Nombre, lineaProduccion.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_LineaProduccion_Eliminar", new { Id = id });
}
