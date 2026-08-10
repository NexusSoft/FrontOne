package com.frontone.android.data.sqlserver

import com.frontone.android.domain.port.EmpresaPort

/**
 * Adaptador contra Configuracion.sp_Empresa_Obtener — el mismo SP que ya usa
 * FrontOne de escritorio (Database/Configuracion/002_SP_Empresa.sql, redefinido por
 * última vez en 005_Alter_Empresa_NumeroEmpaque.sql). Sin parámetros: la tabla es
 * singleton (Id=1), no hace falta filtrar nada.
 */
class EmpresaSqlServerAdapter(
    connectionFactory: ConnectionFactory
) : SqlRepositoryBase(connectionFactory), EmpresaPort {

    override suspend fun obtenerLogo(): ByteArray? = ejecutarProcedimiento(
        nombreProcedimiento = "Configuracion.sp_Empresa_Obtener",
        cantidadParametros = 0,
        leerResultado = { statement ->
            statement.executeQuery().use { resultado ->
                if (resultado.next()) resultado.getBytes("Logo") else null
            }
        }
    )
}
