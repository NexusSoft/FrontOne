using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class MateriaPrimaRepository : SqlRepositoryBase, IMateriaPrimaRepository
{
    public MateriaPrimaRepository(IConnectionFactory connectionFactory, ILogger<MateriaPrimaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<MateriaPrima>> ObtenerAsync(int? id = null)
        => QueryAsync<MateriaPrima>("Catalogos.sp_MateriaPrima_Obtener", new { Id = id });

    public Task<IReadOnlyList<MateriaPrima>> ObtenerTop1000Async()
        => QueryAsync<MateriaPrima>("Catalogos.sp_MateriaPrima_ObtenerTop1000");

    public Task<IReadOnlyList<MateriaPrima>> BuscarAsync(string filtro)
        => QueryAsync<MateriaPrima>("Catalogos.sp_MateriaPrima_Buscar", new { Filtro = filtro });

    public Task<int> InsertarAsync(MateriaPrima materiaPrima)
        => ExecuteScalarAsync<int>("Catalogos.sp_MateriaPrima_Insertar", new
        {
            materiaPrima.CodigoSap,
            materiaPrima.DescripcionSap,
            materiaPrima.Activo,
        })!;

    public Task ActualizarDatosSapAsync(int id, string descripcionSap)
        => ExecuteAsync("Catalogos.sp_MateriaPrima_ActualizarDatosSap", new { Id = id, DescripcionSap = descripcionSap });

    public Task ActualizarActivoAsync(int id, bool activo)
        => ExecuteAsync("Catalogos.sp_MateriaPrima_ActualizarActivo", new { Id = id, Activo = activo });

    public Task ActualizarAsync(MateriaPrima materiaPrima)
        => ExecuteAsync("Catalogos.sp_MateriaPrima_Actualizar", new
        {
            materiaPrima.Id,
            materiaPrima.CategoriaId,
            materiaPrima.CalibreApeamId,
        });
}
