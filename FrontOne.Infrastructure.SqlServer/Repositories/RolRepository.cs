using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class RolRepository : SqlRepositoryBase, IRolRepository
{
    public RolRepository(IConnectionFactory connectionFactory, ILogger<RolRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Rol>> ObtenerAsync(int? id = null)
        => QueryAsync<Rol>("Seguridad.sp_Rol_Obtener", new { Id = id });

    public Task<int> InsertarAsync(Rol rol)
        => ExecuteScalarAsync<int>("Seguridad.sp_Rol_Insertar", new { rol.Nombre, rol.Descripcion, rol.Activo })!;

    public Task ActualizarAsync(Rol rol)
        => ExecuteAsync("Seguridad.sp_Rol_Actualizar", new { rol.Id, rol.Nombre, rol.Descripcion, rol.Activo });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Seguridad.sp_Rol_Eliminar", new { Id = id });
}
