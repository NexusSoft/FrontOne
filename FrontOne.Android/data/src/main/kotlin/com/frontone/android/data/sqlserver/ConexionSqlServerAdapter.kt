package com.frontone.android.data.sqlserver

import com.frontone.android.domain.model.ResultadoConexion
import com.frontone.android.domain.port.ConexionSqlServerPort
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.sql.SQLException

/**
 * Adaptador secundario que implementa ConexionSqlServerPort (domain) contra el
 * SQL Server real de FrontOne.
 *
 * Excepción documentada a la regla "todo por Stored Procedure": esta única consulta
 * (SELECT @@VERSION) no es dato de negocio, es solo una verificación de conectividad
 * para el piloto del scaffold — no crea un SP nuevo para esto. Cualquier adaptador de
 * datos de negocio real debe extender SqlRepositoryBase y usar ejecutarProcedimiento.
 */
class ConexionSqlServerAdapter(
    private val connectionFactory: ConnectionFactory
) : ConexionSqlServerPort {

    override suspend fun probarConexion(): ResultadoConexion = withContext(Dispatchers.IO) {
        try {
            connectionFactory.abrirConexion().use { conexion ->
                conexion.createStatement().use { statement ->
                    statement.executeQuery("SELECT @@VERSION AS Version").use { resultado ->
                        resultado.next()
                        ResultadoConexion.Exitosa(resultado.getString("Version").lineSequence().first())
                    }
                }
            }
        } catch (ex: SQLException) {
            ResultadoConexion.Fallida(mensajeParaUsuario(ex))
        }
    }

    private fun mensajeParaUsuario(ex: SQLException): String = when {
        ex.message?.contains("timeout", ignoreCase = true) == true ->
            "No se pudo contactar al servidor — verifica que el dispositivo esté en la red/VPN correcta."
        ex.message?.contains("Login failed", ignoreCase = true) == true ->
            "Usuario o contraseña de conexión incorrectos."
        else -> "No se pudo conectar a SQL Server: ${ex.message}"
    }
}
