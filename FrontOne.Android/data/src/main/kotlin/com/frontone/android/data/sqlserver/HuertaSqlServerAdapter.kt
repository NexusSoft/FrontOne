package com.frontone.android.data.sqlserver

import com.frontone.android.domain.model.HuertaMapa
import com.frontone.android.domain.port.HuertaPort
import java.math.BigDecimal

/**
 * Adaptador contra `Catalogos.sp_Huerta_*ConCoordenadas`/`sp_Huerta_ActualizarUbicacion`
 * — únicos 3 SPs de Huerta que consume Android. Ver
 * `Database/Catalogos/045_SP_Huerta_MapaMovil.sql`.
 */
class HuertaSqlServerAdapter(
    connectionFactory: ConnectionFactory
) : SqlRepositoryBase(connectionFactory), HuertaPort {

    override suspend fun obtenerTop100ConCoordenadas(): List<HuertaMapa> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Catalogos.sp_Huerta_ObtenerTop100ConCoordenadas",
            cantidadParametros = 0,
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList { while (resultado.next()) add(resultado.aHuertaMapa()) }
                }
            }
        )

    override suspend fun buscarConCoordenadas(filtro: String): List<HuertaMapa> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Catalogos.sp_Huerta_BuscarConCoordenadas",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setString(1, filtro) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList { while (resultado.next()) add(resultado.aHuertaMapa()) }
                }
            }
        )

    override suspend fun actualizarUbicacion(id: Int, latitud: BigDecimal, longitud: BigDecimal) {
        ejecutarProcedimiento<Unit>(
            nombreProcedimiento = "Catalogos.sp_Huerta_ActualizarUbicacion",
            cantidadParametros = 3,
            asignarParametros = { statement ->
                statement.setInt(1, id)
                statement.setBigDecimal(2, latitud)
                statement.setBigDecimal(3, longitud)
            },
            leerResultado = { statement -> statement.execute() }
        )
    }

    private fun java.sql.ResultSet.aHuertaMapa(): HuertaMapa = HuertaMapa(
        id = getInt("Id"),
        nombre = getString("Nombre") ?: "",
        registroSagarpa = getString("RegistroSagarpa"),
        productorNombre = getString("ProductorNombre") ?: "",
        latitud = getBigDecimal("Latitud") ?: BigDecimal.ZERO,
        longitud = getBigDecimal("Longitud") ?: BigDecimal.ZERO
    )
}
