package com.frontone.android.data.sqlserver

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.sql.CallableStatement
import java.sql.SQLException

/**
 * Equivalente Kotlin de FrontOne.Infrastructure.SqlServer/Repositories/SqlRepositoryBase.cs.
 *
 * Regla dura: todo adaptador de :data hereda de esta clase y llama Stored Procedures
 * exclusivamente vía [ejecutarProcedimiento] — nunca arma una sentencia SQL a mano.
 * Los errores de SQL Server se traducen a español aquí, en un solo lugar (mismo
 * criterio que SqlRepositoryBase.RunAsync detectando SqlException.Number == 547 en C#).
 */
abstract class SqlRepositoryBase(
    protected val connectionFactory: ConnectionFactory
) {
    protected suspend fun <T> ejecutarProcedimiento(
        nombreProcedimiento: String,
        cantidadParametros: Int,
        asignarParametros: (CallableStatement) -> Unit = {},
        leerResultado: (CallableStatement) -> T
    ): T = withContext(Dispatchers.IO) {
        val marcadores = List(cantidadParametros) { "?" }.joinToString(",")
        val llamada = "{call $nombreProcedimiento($marcadores)}"
        try {
            connectionFactory.abrirConexion().use { conexion ->
                conexion.prepareCall(llamada).use { statement ->
                    asignarParametros(statement)
                    leerResultado(statement)
                }
            }
        } catch (ex: SQLException) {
            throw traducirError(ex)
        }
    }

    private fun traducirError(ex: SQLException): SqlRepositoryException {
        // Código 547 = violación de FK, idéntico significado que en SQL Server vía Dapper/C#.
        val mensaje = when (ex.errorCode) {
            547 -> "No se puede eliminar este registro porque ya está siendo utilizado en otra pantalla del sistema."
            2627, 2601 -> "Ya existe un registro con esos datos."
            else -> "Ocurrió un error al comunicarse con el servidor. Intenta de nuevo."
        }
        return SqlRepositoryException(mensaje, ex)
    }
}

class SqlRepositoryException(message: String, cause: Throwable? = null) : Exception(message, cause)
