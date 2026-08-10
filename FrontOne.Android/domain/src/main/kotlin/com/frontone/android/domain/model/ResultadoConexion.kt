package com.frontone.android.domain.model

/**
 * Resultado de probar la conexión a SQL Server.
 * Nunca expone la excepción cruda de JDBC al resto de las capas — el adaptador
 * en :data la traduce a un mensaje en español, mismo criterio que SqlRepositoryBase
 * en FrontOne (C#) traduce SqlException a mensajes claros para el usuario.
 */
sealed interface ResultadoConexion {
    data class Exitosa(val versionServidor: String) : ResultadoConexion
    data class Fallida(val mensaje: String) : ResultadoConexion
}
