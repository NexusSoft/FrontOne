package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.PalletPort
import java.math.BigDecimal

/**
 * Agrega una línea de detalle — equivalente a PalletService.AgregarLineaAsync (FrontOne C#).
 *
 * Regla real de escritorio que hay que replicar aquí (no vive en el SP): si ya existe una línea
 * del mismo Pallet con el mismo CorridaId + ProductoTerminadoId, no se duplica la fila — se suma
 * a la existente (Cajas o Kilogramos, según cuál use la presentación del producto) vía
 * actualizarDetalle. Solo se inserta una fila nueva cuando no hay coincidencia.
 */
class AgregarLineaPalletUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(
        palletId: Int,
        corridaId: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal
    ) {
        val detalleActual = palletPort.obtenerDetalle(palletId)
        val existente = detalleActual.firstOrNull {
            it.corridaId == corridaId && it.productoTerminadoId == productoTerminadoId
        }

        if (existente == null) {
            palletPort.insertarDetalle(palletId, corridaId, productoTerminadoId, cajas, kilogramos, porcentajeMateriaSeca)
            return
        }

        if (cajas != null) {
            val cajasSumadas = (existente.cajas ?: 0) + cajas
            palletPort.actualizarDetalle(existente.id, productoTerminadoId, cajasSumadas, null, porcentajeMateriaSeca)
        } else {
            val kilosSumados = existente.kilogramos + (kilogramos ?: BigDecimal.ZERO)
            palletPort.actualizarDetalle(existente.id, productoTerminadoId, null, kilosSumados, porcentajeMateriaSeca)
        }
    }
}
