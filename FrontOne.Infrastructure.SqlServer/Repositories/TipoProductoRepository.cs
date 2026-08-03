using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class TipoProductoRepository : SqlRepositoryBase, ITipoProductoRepository
{
    public TipoProductoRepository(IConnectionFactory connectionFactory, ILogger<TipoProductoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<TipoProducto>> ObtenerAsync(int? id = null)
        => QueryAsync<TipoProducto>("Catalogos.sp_TipoProducto_Obtener", new { Id = id });

    public Task<int> InsertarAsync(TipoProducto tipoProducto)
        => ExecuteScalarAsync<int>("Catalogos.sp_TipoProducto_Insertar", new { tipoProducto.Nombre })!;

    public Task ActualizarAsync(TipoProducto tipoProducto)
        => ExecuteAsync("Catalogos.sp_TipoProducto_Actualizar", new { tipoProducto.Id, tipoProducto.Nombre, tipoProducto.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_TipoProducto_Eliminar", new { Id = id });
}
