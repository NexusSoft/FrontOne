using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ProductoRepository : SqlRepositoryBase, IProductoRepository
{
    public ProductoRepository(IConnectionFactory connectionFactory, ILogger<ProductoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Producto>> ObtenerAsync(int? id = null)
        => QueryAsync<Producto>("Catalogos.sp_Producto_Obtener", new { Id = id });

    public Task<int> InsertarAsync(Producto producto)
        => ExecuteScalarAsync<int>("Catalogos.sp_Producto_Insertar", new { producto.Nombre, producto.Activo })!;

    public Task ActualizarAsync(Producto producto)
        => ExecuteAsync("Catalogos.sp_Producto_Actualizar", new { producto.Id, producto.Nombre, producto.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Producto_Eliminar", new { Id = id });
}
