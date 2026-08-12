package com.frontone.android.domain.port

import com.frontone.android.domain.model.LoteEnProceso
import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.model.PalletDetalle
import java.math.BigDecimal

/** Equivalente a IPalletRepository (FrontOne C#) — un método por Stored Procedure de Produccion.Pallet/PalletDetalle. */
interface PalletPort {
    suspend fun obtener(id: Int? = null): List<Pallet>
    suspend fun obtenerDetalle(palletId: Int): List<PalletDetalle>
    suspend fun obtenerLotesEnProceso(lineaProduccionId: Int? = null): List<LoteEnProceso>

    suspend fun insertar(
        lineaProduccionId: Int,
        esMixto: Boolean,
        productoTerminadoId: Int?,
        pesoReal: BigDecimal?
    ): Int

    suspend fun actualizarEncabezado(
        id: Int,
        lineaProduccionId: Int,
        esMixto: Boolean,
        productoTerminadoId: Int?,
        pesoReal: BigDecimal?
    )

    suspend fun bloquear(id: Int)
    suspend fun eliminar(id: Int)

    suspend fun insertarDetalle(
        palletId: Int,
        corridaId: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal
    ): Int

    suspend fun actualizarDetalle(
        id: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal
    )

    suspend fun eliminarDetalle(id: Int)
}
