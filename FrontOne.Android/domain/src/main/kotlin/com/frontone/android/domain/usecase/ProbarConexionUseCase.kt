package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.ResultadoConexion
import com.frontone.android.domain.port.ConexionSqlServerPort

/**
 * Caso de uso piloto del scaffold: verifica que el dispositivo puede llegar a la
 * misma base de datos SQL Server que usa FrontOne de escritorio.
 *
 * Sirve como plantilla para casos de uso reales — equivalente en espíritu a un
 * método de {Entidad}Service.cs en FrontOne.Application: orquesta el puerto,
 * sin saber nada de JDBC/Android/Compose.
 */
class ProbarConexionUseCase(
    private val conexionSqlServerPort: ConexionSqlServerPort
) {
    suspend operator fun invoke(): ResultadoConexion = conexionSqlServerPort.probarConexion()
}
