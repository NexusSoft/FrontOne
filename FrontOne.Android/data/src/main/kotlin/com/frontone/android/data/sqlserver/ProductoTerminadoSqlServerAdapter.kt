package com.frontone.android.data.sqlserver

import com.frontone.android.domain.model.PresentacionProducto
import com.frontone.android.domain.model.ProductoTerminado
import com.frontone.android.domain.port.ProductoTerminadoPort
import java.sql.ResultSet

/**
 * Adaptador contra Catalogos.sp_ProductoTerminado_ObtenerTop1000/_Buscar — solo los dos métodos
 * que consume Pallets (subconjunto de columnas, el catálogo completo se administra en escritorio).
 * Ver Database/Catalogos/041_Alter_ProductoTerminado_Presentacion.sql.
 */
class ProductoTerminadoSqlServerAdapter(
    connectionFactory: ConnectionFactory
) : SqlRepositoryBase(connectionFactory), ProductoTerminadoPort {

    override suspend fun obtenerTop1000(): List<ProductoTerminado> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Catalogos.sp_ProductoTerminado_ObtenerTop1000",
            cantidadParametros = 0,
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList { while (resultado.next()) add(resultado.aProductoTerminado()) }
                }
            }
        )

    override suspend fun buscar(filtro: String): List<ProductoTerminado> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Catalogos.sp_ProductoTerminado_Buscar",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setString(1, filtro) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList { while (resultado.next()) add(resultado.aProductoTerminado()) }
                }
            }
        )

    private fun ResultSet.aProductoTerminado(): ProductoTerminado = ProductoTerminado(
        id = getInt("Id"),
        codigoSap = getString("CodigoSap") ?: "",
        descripcionSap = getString("DescripcionSap") ?: "",
        activo = getBoolean("Activo"),
        pesoNeto = getBigDecimal("PesoNeto"),
        cajasPorPallet = getNullableInt("CajasPorPallet"),
        presentacion = PresentacionProducto.desde(getInt("Presentacion"))
    )
}
