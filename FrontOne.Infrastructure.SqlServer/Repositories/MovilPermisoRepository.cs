using System.Data;
using Dapper;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class MovilPermisoRepository : SqlRepositoryBase, IMovilPermisoRepository
{
    public MovilPermisoRepository(IConnectionFactory connectionFactory, ILogger<MovilPermisoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<MovilPermiso>> ObtenerPorRolAsync(int rolId)
        => QueryAsync<MovilPermiso>("Seguridad.sp_MovilPermiso_ObtenerPorRol", new { RolId = rolId });

    public Task SincronizarAsync(int rolId, IReadOnlyList<MovilPermiso> filas)
        => ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await connection.ExecuteAsync(new CommandDefinition(
                "Seguridad.sp_MovilPermiso_EliminarPorRol",
                new { RolId = rolId },
                transaction,
                commandType: CommandType.StoredProcedure));

            foreach (var fila in filas)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    "Seguridad.sp_MovilPermiso_Insertar",
                    new
                    {
                        RolId = rolId,
                        fila.PantallaCodigo,
                        fila.Consultar,
                        fila.Crear,
                        fila.Modificar,
                        fila.Eliminar,
                    },
                    transaction,
                    commandType: CommandType.StoredProcedure));
            }
        }, "SincronizarMovilPermisosRol");
}
