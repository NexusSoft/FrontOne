package com.frontone.android.data.sqlserver

import com.frontone.android.domain.model.EstatusPallet
import com.frontone.android.domain.model.LoteEnProceso
import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.model.PalletDetalle
import com.frontone.android.domain.port.PalletPort
import java.math.BigDecimal
import java.sql.CallableStatement
import java.sql.ResultSet
import java.sql.Types
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.LocalTime

/**
 * Adaptador contra Produccion.sp_Pallet_* / Produccion.sp_PalletDetalle_* — mismos SPs que ya
 * consume FrontOne.WinForms (PalletRepository.cs). Ver Database/Produccion/004_Schema_SP_Pallet.sql,
 * 005_Alter_Pallet_ProductoEncabezado.sql, 006_Alter_Pallet_BloquearSoloCompleto.sql y
 * 009_Alter_PalletDetalle_Granel.sql para las firmas vigentes.
 */
class PalletSqlServerAdapter(
    connectionFactory: ConnectionFactory
) : SqlRepositoryBase(connectionFactory), PalletPort {

    override suspend fun obtener(id: Int?): List<Pallet> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Produccion.sp_Pallet_Obtener",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setNullableInt(1, id) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList { while (resultado.next()) add(resultado.aPallet()) }
                }
            }
        )

    override suspend fun obtenerDetalle(palletId: Int): List<PalletDetalle> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Produccion.sp_Pallet_ObtenerDetalle",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setInt(1, palletId) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList { while (resultado.next()) add(resultado.aPalletDetalle()) }
                }
            }
        )

    override suspend fun obtenerLotesEnProceso(lineaProduccionId: Int?): List<LoteEnProceso> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Produccion.sp_Pallet_ObtenerLotesEnProceso",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setNullableInt(1, lineaProduccionId) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList { while (resultado.next()) add(resultado.aLoteEnProceso()) }
                }
            }
        )

    override suspend fun insertar(
        lineaProduccionId: Int,
        esMixto: Boolean,
        productoTerminadoId: Int?,
        pesoReal: BigDecimal?
    ): Int =
        ejecutarProcedimiento(
            nombreProcedimiento = "Produccion.sp_Pallet_Insertar",
            cantidadParametros = 4,
            asignarParametros = { statement ->
                statement.setInt(1, lineaProduccionId)
                statement.setBoolean(2, esMixto)
                statement.setNullableInt(3, productoTerminadoId)
                statement.setNullableBigDecimal(4, pesoReal)
            },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    resultado.next()
                    resultado.getInt("Id")
                }
            }
        )

    override suspend fun actualizarEncabezado(
        id: Int,
        lineaProduccionId: Int,
        esMixto: Boolean,
        productoTerminadoId: Int?,
        pesoReal: BigDecimal?
    ) {
        ejecutarProcedimiento<Unit>(
            nombreProcedimiento = "Produccion.sp_Pallet_ActualizarEncabezado",
            cantidadParametros = 5,
            asignarParametros = { statement ->
                statement.setInt(1, id)
                statement.setInt(2, lineaProduccionId)
                statement.setBoolean(3, esMixto)
                statement.setNullableInt(4, productoTerminadoId)
                statement.setNullableBigDecimal(5, pesoReal)
            },
            leerResultado = { statement -> statement.execute() }
        )
    }

    override suspend fun eliminar(id: Int) {
        ejecutarProcedimiento<Unit>(
            nombreProcedimiento = "Produccion.sp_Pallet_Eliminar",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setInt(1, id) },
            leerResultado = { statement -> statement.execute() }
        )
    }

    override suspend fun insertarDetalle(
        palletId: Int,
        corridaId: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal
    ): Int =
        ejecutarProcedimiento(
            nombreProcedimiento = "Produccion.sp_PalletDetalle_Insertar",
            cantidadParametros = 6,
            asignarParametros = { statement ->
                statement.setInt(1, palletId)
                statement.setInt(2, corridaId)
                statement.setInt(3, productoTerminadoId)
                statement.setNullableInt(4, cajas)
                statement.setNullableBigDecimal(5, kilogramos)
                statement.setBigDecimal(6, porcentajeMateriaSeca)
            },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    resultado.next()
                    resultado.getInt("Id")
                }
            }
        )

    override suspend fun actualizarDetalle(
        id: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal
    ) {
        ejecutarProcedimiento<Unit>(
            nombreProcedimiento = "Produccion.sp_PalletDetalle_Actualizar",
            cantidadParametros = 5,
            asignarParametros = { statement ->
                statement.setInt(1, id)
                statement.setInt(2, productoTerminadoId)
                statement.setNullableInt(3, cajas)
                statement.setNullableBigDecimal(4, kilogramos)
                statement.setBigDecimal(5, porcentajeMateriaSeca)
            },
            leerResultado = { statement -> statement.execute() }
        )
    }

    override suspend fun eliminarDetalle(id: Int) {
        ejecutarProcedimiento<Unit>(
            nombreProcedimiento = "Produccion.sp_PalletDetalle_Eliminar",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setInt(1, id) },
            leerResultado = { statement -> statement.execute() }
        )
    }

    private fun ResultSet.aPallet(): Pallet = Pallet(
        id = getInt("Id"),
        folio = getString("Folio"),
        fechaCreacion = getDate("FechaCreacion").aLocalDate(),
        horaCreacion = getTime("HoraCreacion").aLocalTime(),
        estatus = EstatusPallet.desde(getInt("Estatus")),
        lineaProduccionId = getInt("LineaProduccionId"),
        lineaProduccionNombre = getString("LineaProduccionNombre"),
        esMixto = getBoolean("EsMixto"),
        productoTerminadoId = getNullableInt("ProductoTerminadoId"),
        porcentajeMateriaSeca = getBigDecimal("PorcentajeMateriaSeca") ?: BigDecimal.ZERO,
        pesoReal = getBigDecimal("PesoReal"),
        bloqueado = getBoolean("Bloqueado"),
        fechaBloqueo = getTimestamp("FechaBloqueo")?.aLocalDateTime(),
        noReempaque = getNullableInt("NoReempaque"),
        primeraCorrida = getBoolean("PrimeraCorrida"),
        totalCajas = getInt("TotalCajas"),
        totalKilogramos = getBigDecimal("TotalKilogramos") ?: BigDecimal.ZERO,
        productoDescripcion = getString("ProductoDescripcion") ?: "",
        productoCodigoSap = getString("ProductoCodigoSap") ?: "",
        fechaCreacionRegistro = getTimestamp("FechaCreacionRegistro").aLocalDateTime(),
        esNeutro = getBoolean("EsNeutro")
    )

    private fun ResultSet.aPalletDetalle(): PalletDetalle = PalletDetalle(
        id = getInt("Id"),
        palletId = getInt("PalletId"),
        corridaId = getInt("CorridaId"),
        loteId = getInt("LoteId"),
        loteFolio = getString("LoteFolio"),
        productoTerminadoId = getInt("ProductoTerminadoId"),
        productoCodigoSap = getString("ProductoCodigoSap") ?: "",
        productoDescripcion = getString("ProductoDescripcion") ?: "",
        cajas = getNullableInt("Cajas"),
        kilogramos = getBigDecimal("Kilogramos") ?: BigDecimal.ZERO,
        porcentajeMateriaSeca = getBigDecimal("PorcentajeMateriaSeca") ?: BigDecimal.ZERO,
        cajasPorPallet = getNullableInt("CajasPorPallet"),
        loteEnProceso = getBoolean("LoteEnProceso")
    )

    private fun ResultSet.aLoteEnProceso(): LoteEnProceso = LoteEnProceso(
        corridaId = getInt("CorridaId"),
        loteId = getInt("LoteId"),
        loteFolio = getString("LoteFolio"),
        codigoTrazabilidad = getString("CodigoTrazabilidad") ?: "",
        lineaProduccionId = getInt("LineaProduccionId"),
        lineaProduccionNombre = getString("LineaProduccionNombre"),
        porcentajeMateriaSeca = getBigDecimal("PorcentajeMateriaSeca") ?: BigDecimal.ZERO,
        kilosAProcesar = getBigDecimal("KilosAProcesar") ?: BigDecimal.ZERO,
        kilosProcesados = getBigDecimal("KilosProcesados") ?: BigDecimal.ZERO,
        kilosDisponibles = getBigDecimal("KilosDisponibles") ?: BigDecimal.ZERO,
        huertaNombre = getString("HuertaNombre"),
        registroSagarpa = getString("RegistroSagarpa"),
        productorNombre = getString("ProductorNombre")
    )
}

/** [CallableStatement.setInt] o `setNull(Types.INTEGER)` según corresponda — evita repetir el if/else en cada adaptador. */
internal fun CallableStatement.setNullableInt(indice: Int, valor: Int?) {
    if (valor == null) setNull(indice, Types.INTEGER) else setInt(indice, valor)
}

/** Equivalente a [setNullableInt] para `DECIMAL`. */
internal fun CallableStatement.setNullableBigDecimal(indice: Int, valor: BigDecimal?) {
    if (valor == null) setNull(indice, Types.DECIMAL) else setBigDecimal(indice, valor)
}

/** `ResultSet.getInt` no distingue 0 de NULL — hay que preguntar [ResultSet.wasNull] después. */
internal fun ResultSet.getNullableInt(columna: String): Int? {
    val valor = getInt(columna)
    return if (wasNull()) null else valor
}

/**
 * `java.sql.Date/Time/Timestamp.toLocalDate()/toLocalTime()/toLocalDateTime()` existen en el JDK
 * de escritorio pero NO en el `java.sql` recortado de Android (`core-oj.jar`) — llamarlos crashea
 * en tiempo de ejecución con `NoSuchMethodError` aunque compile sin avisos, porque `java.sql.Date`
 * sigue extendiendo `java.util.Date` en Android, solo que sin esos métodos default de conveniencia.
 * Se parsea en su lugar el formato de `toString()` que el contrato JDBC garantiza fijo
 * (`yyyy-MM-dd`, `HH:mm:ss`, `yyyy-MM-dd HH:mm:ss[.fffffffff]`) — no depende de la plataforma.
 */
internal fun java.sql.Date.aLocalDate(): LocalDate = LocalDate.parse(toString())

internal fun java.sql.Time.aLocalTime(): LocalTime = LocalTime.parse(toString())

internal fun java.sql.Timestamp.aLocalDateTime(): LocalDateTime {
    val (fecha, hora) = toString().split(" ", limit = 2)
    return LocalDateTime.of(LocalDate.parse(fecha), LocalTime.parse(hora.substringBefore(".")))
}
