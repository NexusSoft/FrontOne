using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ProductoTerminadoRepository : SqlRepositoryBase, IProductoTerminadoRepository
{
    public ProductoTerminadoRepository(IConnectionFactory connectionFactory, ILogger<ProductoTerminadoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<ProductoTerminado>> ObtenerAsync(int? id = null)
        => QueryAsync<ProductoTerminado>("Catalogos.sp_ProductoTerminado_Obtener", new { Id = id });

    public Task<IReadOnlyList<ProductoTerminado>> ObtenerTop1000Async()
        => QueryAsync<ProductoTerminado>("Catalogos.sp_ProductoTerminado_ObtenerTop1000");

    public Task<IReadOnlyList<ProductoTerminado>> BuscarAsync(string filtro)
        => QueryAsync<ProductoTerminado>("Catalogos.sp_ProductoTerminado_Buscar", new { Filtro = filtro });

    public Task<int> InsertarAsync(ProductoTerminado producto)
        => ExecuteScalarAsync<int>("Catalogos.sp_ProductoTerminado_Insertar", new
        {
            producto.CodigoSap,
            producto.DescripcionSap,
            producto.DescripcionExtranjeraSap,
            producto.Activo,
        })!;

    public Task ActualizarDatosSapAsync(int id, string descripcionSap, string? descripcionExtranjeraSap)
        => ExecuteAsync("Catalogos.sp_ProductoTerminado_ActualizarDatosSap", new
        {
            Id = id,
            DescripcionSap = descripcionSap,
            DescripcionExtranjeraSap = descripcionExtranjeraSap,
        });

    public Task ActualizarActivoAsync(int id, bool activo)
        => ExecuteAsync("Catalogos.sp_ProductoTerminado_ActualizarActivo", new { Id = id, Activo = activo });

    public Task ActualizarAsync(ProductoTerminado producto)
        => ExecuteAsync("Catalogos.sp_ProductoTerminado_Actualizar", new
        {
            producto.Id,
            producto.CodigoUpc,
            producto.CodigoPlu,
            producto.CodigoGtin,
            producto.MateriaPrimaItemCode,
            producto.CategoriaId,
            producto.TipoProductoId,
            producto.CalibreApeamId,
            producto.CalibreCodigoExterno,
            producto.MercadoDestinoPaisId,
            producto.MarcaId,
            producto.VariedadId,
            producto.PesoEstandarId,
            producto.PesoNeto,
            producto.PesoPromedio,
            producto.CajasPorPallet,
            producto.Presentacion,
        });
}
