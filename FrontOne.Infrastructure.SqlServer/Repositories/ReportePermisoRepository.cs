using System.Data;
using Dapper;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ReportePermisoRepository : SqlRepositoryBase, IReportePermisoRepository
{
    public ReportePermisoRepository(IConnectionFactory connectionFactory, ILogger<ReportePermisoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<ReportePermiso>> ObtenerPorRolAsync(int rolId)
        => QueryAsync<ReportePermiso>("Seguridad.sp_ReportePermiso_ObtenerPorRol", new { RolId = rolId });

    public Task SincronizarAsync(int rolId, IReadOnlyList<ReportePermiso> filas)
        => ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await connection.ExecuteAsync(new CommandDefinition(
                "Seguridad.sp_ReportePermiso_EliminarPorRol",
                new { RolId = rolId },
                transaction,
                commandType: CommandType.StoredProcedure));

            foreach (var fila in filas)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    "Seguridad.sp_ReportePermiso_Insertar",
                    new
                    {
                        RolId = rolId,
                        fila.ReporteCodigo,
                        fila.VistaPrevia,
                        fila.Impresion,
                        fila.Exportacion,
                        fila.Diseno,
                    },
                    transaction,
                    commandType: CommandType.StoredProcedure));
            }
        }, "SincronizarReportePermisosRol");
}
