using System.Data;
using Dapper;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class WebPermisoRepository : SqlRepositoryBase, IWebPermisoRepository
{
    public WebPermisoRepository(IConnectionFactory connectionFactory, ILogger<WebPermisoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<WebPermiso>> ObtenerPorRolAsync(int rolId)
        => QueryAsync<WebPermiso>("Seguridad.sp_WebPermiso_ObtenerPorRol", new { RolId = rolId });

    public Task SincronizarAsync(int rolId, IReadOnlyList<WebPermiso> filas)
        => ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await connection.ExecuteAsync(new CommandDefinition(
                "Seguridad.sp_WebPermiso_EliminarPorRol",
                new { RolId = rolId },
                transaction,
                commandType: CommandType.StoredProcedure));

            foreach (var fila in filas)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    "Seguridad.sp_WebPermiso_Insertar",
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
        }, "SincronizarWebPermisosRol");
}
