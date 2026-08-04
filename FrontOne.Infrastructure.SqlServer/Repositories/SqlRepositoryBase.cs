using System.Data;
using Dapper;
using FrontOne.Infrastructure.SqlServer.Factories;
using FrontOne.Shared.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public abstract class SqlRepositoryBase
{
    private readonly IConnectionFactory _connectionFactory;
    protected readonly ILogger Logger;

    protected SqlRepositoryBase(IConnectionFactory connectionFactory, ILogger logger)
    {
        _connectionFactory = connectionFactory;
        Logger = logger;
    }

    protected Task<int> ExecuteAsync(string storedProcedure, object? parameters = null)
        => RunAsync(storedProcedure, connection =>
            connection.ExecuteAsync(new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure)));

    protected Task<T?> ExecuteScalarAsync<T>(string storedProcedure, object? parameters = null)
        => RunAsync(storedProcedure, connection =>
            connection.ExecuteScalarAsync<T>(new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure)));

    protected async Task<IReadOnlyList<T>> QueryAsync<T>(string storedProcedure, object? parameters = null)
    {
        var rows = await RunAsync(storedProcedure, connection =>
            connection.QueryAsync<T>(new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure)));
        return rows.AsList();
    }

    protected Task<T?> QueryFirstAsync<T>(string storedProcedure, object? parameters = null)
        => RunAsync(storedProcedure, connection =>
            connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure)));

    protected Task<TResult> QueryMultipleAsync<TResult>(
        string storedProcedure,
        object? parameters,
        Func<SqlMapper.GridReader, Task<TResult>> map)
        => RunAsync(storedProcedure, async connection =>
        {
            using var reader = await connection.QueryMultipleAsync(
                new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure));
            return await map(reader);
        });

    protected async Task ExecuteInTransactionAsync(Func<SqlConnection, SqlTransaction, Task> action, string operationName)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            await action(connection, transaction);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Logger.LogError(ex, "Error en transacción {Operation}", operationName);
            throw new SqlRepositoryException($"Error en transacción: {operationName}", ex, operationName);
        }
    }

    private async Task<T> RunAsync<T>(string storedProcedure, Func<SqlConnection, Task<T>> operation)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            return await operation(connection);
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            // Violación de FK (DELETE/UPDATE que dejaría una referencia huérfana): el registro ya fue
            // seleccionado/usado en otra pantalla del sistema y la integridad referencial lo bloquea.
            Logger.LogError(ex, "Violación de integridad referencial ejecutando {StoredProcedure}", storedProcedure);
            throw new SqlRepositoryException(
                "No se puede eliminar este registro porque ya está siendo utilizado en otra pantalla del sistema.",
                ex,
                storedProcedure);
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            // THROW 50000 de negocio dentro del SP (ej. "Este lote ya fue capturado en una corrida."):
            // el mensaje ya viene listo para mostrar al usuario tal cual, sin envolver genérico.
            Logger.LogError(ex, "Regla de negocio violada ejecutando {StoredProcedure}", storedProcedure);
            throw new SqlRepositoryException(ex.Message, ex, storedProcedure);
        }
        catch (SqlException ex)
        {
            Logger.LogError(ex, "Error SQL ejecutando {StoredProcedure}", storedProcedure);
            throw new SqlRepositoryException($"Error ejecutando {storedProcedure}", ex, storedProcedure);
        }
    }
}
