using System.Data;
using Dapper;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class PermisoRepository : SqlRepositoryBase, IPermisoRepository
{
    private record EstructuraRow(int ModuloId, string ModuloNombre, int PantallaId, string PantallaNombre, int AccionId, string AccionNombre);

    private record OtorgadoRow(int PantallaId, int AccionId);

    public PermisoRepository(IConnectionFactory connectionFactory, ILogger<PermisoRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<(int ModuloId, string ModuloNombre, int PantallaId, string PantallaNombre, int AccionId, string AccionNombre)>> ObtenerEstructuraAsync()
    {
        var filas = await QueryAsync<EstructuraRow>("Seguridad.sp_Permiso_ObtenerEstructura");
        return filas.Select(f => (f.ModuloId, f.ModuloNombre, f.PantallaId, f.PantallaNombre, f.AccionId, f.AccionNombre)).ToList();
    }

    public async Task<IReadOnlyList<(int PantallaId, int AccionId)>> ObtenerPorRolAsync(int rolId)
    {
        var filas = await QueryAsync<OtorgadoRow>("Seguridad.sp_Permiso_ObtenerPorRol", new { RolId = rolId });
        return filas.Select(f => (f.PantallaId, f.AccionId)).ToList();
    }

    public Task SincronizarAsync(int rolId, IReadOnlyList<(int PantallaId, int AccionId)> otorgados)
        => ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await connection.ExecuteAsync(new CommandDefinition(
                "Seguridad.sp_Permiso_EliminarPorRol",
                new { RolId = rolId },
                transaction,
                commandType: CommandType.StoredProcedure));

            foreach (var (pantallaId, accionId) in otorgados)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    "Seguridad.sp_Permiso_Insertar",
                    new { RolId = rolId, PantallaId = pantallaId, AccionId = accionId },
                    transaction,
                    commandType: CommandType.StoredProcedure));
            }
        }, "SincronizarPermisosRol");
}
