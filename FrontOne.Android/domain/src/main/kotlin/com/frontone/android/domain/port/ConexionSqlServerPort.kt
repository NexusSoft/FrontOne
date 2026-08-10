package com.frontone.android.domain.port

import com.frontone.android.domain.model.ResultadoConexion

/**
 * Puerto (interfaz) que el dominio necesita para verificar la conexión a SQL Server.
 * El adaptador concreto vive en :data (ConexionSqlServerAdapter, vía JDBC).
 *
 * Regla dura del proyecto: todo acceso a datos real pasa por un puerto definido aquí,
 * el dominio nunca conoce JDBC ni el nombre del driver — mismo principio que
 * FrontOne.Domain/Interfaces/I{Entidad}Repository.cs en el proyecto de escritorio.
 */
interface ConexionSqlServerPort {
    suspend fun probarConexion(): ResultadoConexion
}
