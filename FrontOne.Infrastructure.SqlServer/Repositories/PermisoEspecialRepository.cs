using System.Data;
using Dapper;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class PermisoEspecialRepository : SqlRepositoryBase, IPermisoEspecialRepository
{
    public PermisoEspecialRepository(IConnectionFactory connectionFactory, ILogger<PermisoEspecialRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<PermisoEspecial>> ObtenerPorRolAsync(int rolId)
        => QueryAsync<PermisoEspecial>("Seguridad.sp_PermisoEspecial_ObtenerPorRol", new { RolId = rolId });

    public Task SincronizarAsync(int rolId, IReadOnlyList<PermisoEspecial> filas)
        => ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await connection.ExecuteAsync(new CommandDefinition(
                "Seguridad.sp_PermisoEspecial_EliminarPorRol",
                new { RolId = rolId },
                transaction,
                commandType: CommandType.StoredProcedure));

            foreach (var fila in filas)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    "Seguridad.sp_PermisoEspecial_Insertar",
                    new
                    {
                        RolId = rolId,
                        fila.Codigo,
                        fila.Habilitado,
                    },
                    transaction,
                    commandType: CommandType.StoredProcedure));
            }
        }, "SincronizarPermisosEspecialesRol");

    public Task<IReadOnlyList<string>> ObtenerCodigosHabilitadosAsync(int usuarioId)
        => QueryAsync<string>("Seguridad.sp_Usuario_ObtenerPermisosEspeciales", new { UsuarioId = usuarioId });
}
