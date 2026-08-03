using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class MarcaRepository : SqlRepositoryBase, IMarcaRepository
{
    public MarcaRepository(IConnectionFactory connectionFactory, ILogger<MarcaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Marca>> ObtenerAsync(int? id = null)
        => QueryAsync<Marca>("Catalogos.sp_Marca_Obtener", new { Id = id });

    public Task<int> InsertarAsync(Marca marca)
        => ExecuteScalarAsync<int>("Catalogos.sp_Marca_Insertar", new { marca.Nombre })!;

    public Task ActualizarAsync(Marca marca)
        => ExecuteAsync("Catalogos.sp_Marca_Actualizar", new { marca.Id, marca.Nombre, marca.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Marca_Eliminar", new { Id = id });
}
