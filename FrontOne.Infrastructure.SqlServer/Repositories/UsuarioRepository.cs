using System.Data;
using Dapper;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class UsuarioRepository : SqlRepositoryBase, IUsuarioRepository
{
    public UsuarioRepository(IConnectionFactory connectionFactory, ILogger<UsuarioRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
        => QueryFirstAsync<Usuario>("Seguridad.sp_Usuario_ObtenerPorNombreUsuario", new { NombreUsuario = nombreUsuario });

    public Task<IReadOnlyList<PermisoDto>> ObtenerPermisosAsync(int usuarioId)
        => QueryAsync<PermisoDto>("Seguridad.sp_Usuario_ObtenerPermisos", new { UsuarioId = usuarioId });

    public Task<IReadOnlyList<ReportePermisoDto>> ObtenerPermisosReporteAsync(int usuarioId)
        => QueryAsync<ReportePermisoDto>("Seguridad.sp_Usuario_ObtenerPermisosReporte", new { UsuarioId = usuarioId });

    public Task<IReadOnlyList<Usuario>> ObtenerAsync(int? id = null)
        => QueryAsync<Usuario>("Seguridad.sp_Usuario_Obtener", new { Id = id });

    public Task<int> InsertarAsync(Usuario usuario)
        => ExecuteScalarAsync<int>("Seguridad.sp_Usuario_Insertar", new
        {
            usuario.NombreUsuario,
            usuario.NombreCompleto,
            usuario.Email,
            usuario.PasswordEncriptado,
            usuario.Activo,
        })!;

    public Task ActualizarAsync(Usuario usuario)
        => ExecuteAsync("Seguridad.sp_Usuario_Actualizar", new
        {
            usuario.Id,
            usuario.NombreUsuario,
            usuario.NombreCompleto,
            usuario.Email,
            usuario.PasswordEncriptado,
            usuario.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Seguridad.sp_Usuario_Eliminar", new { Id = id });

    public Task<IReadOnlyList<int>> ObtenerRolesAsync(int usuarioId)
        => QueryAsync<int>("Seguridad.sp_Usuario_ObtenerRoles", new { UsuarioId = usuarioId });

    public Task AsignarRolesAsync(int usuarioId, IReadOnlyList<int> rolIds)
        => ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await connection.ExecuteAsync(new CommandDefinition(
                "Seguridad.sp_UsuarioRol_EliminarPorUsuario",
                new { UsuarioId = usuarioId },
                transaction,
                commandType: CommandType.StoredProcedure));

            foreach (var rolId in rolIds)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    "Seguridad.sp_UsuarioRol_Insertar",
                    new { UsuarioId = usuarioId, RolId = rolId },
                    transaction,
                    commandType: CommandType.StoredProcedure));
            }
        }, "AsignarRolesUsuario");
}
