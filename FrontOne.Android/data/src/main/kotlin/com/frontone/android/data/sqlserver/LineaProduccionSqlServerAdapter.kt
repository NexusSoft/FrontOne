package com.frontone.android.data.sqlserver

import com.frontone.android.domain.model.LineaProduccion
import com.frontone.android.domain.port.LineaProduccionPort

/** Adaptador contra Catalogos.sp_LineaProduccion_Obtener. Ver Database/Catalogos/027_SP_LineaProduccion.sql. */
class LineaProduccionSqlServerAdapter(
    connectionFactory: ConnectionFactory
) : SqlRepositoryBase(connectionFactory), LineaProduccionPort {

    override suspend fun obtener(): List<LineaProduccion> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Catalogos.sp_LineaProduccion_Obtener",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setNullableInt(1, null) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList {
                        while (resultado.next()) {
                            add(
                                LineaProduccion(
                                    id = resultado.getInt("Id"),
                                    nombre = resultado.getString("Nombre") ?: "",
                                    activo = resultado.getBoolean("Activo")
                                )
                            )
                        }
                    }
                }
            }
        )
}
