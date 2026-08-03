using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class CategoriaRepository : SqlRepositoryBase, ICategoriaRepository
{
    public CategoriaRepository(IConnectionFactory connectionFactory, ILogger<CategoriaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Categoria>> ObtenerAsync(int? id = null)
        => QueryAsync<Categoria>("Catalogos.sp_Categoria_Obtener", new { Id = id });

    public Task<int> InsertarAsync(Categoria categoria)
        => ExecuteScalarAsync<int>("Catalogos.sp_Categoria_Insertar", new { categoria.Nombre })!;

    public Task ActualizarAsync(Categoria categoria)
        => ExecuteAsync("Catalogos.sp_Categoria_Actualizar", new { categoria.Id, categoria.Nombre, categoria.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Categoria_Eliminar", new { Id = id });
}
