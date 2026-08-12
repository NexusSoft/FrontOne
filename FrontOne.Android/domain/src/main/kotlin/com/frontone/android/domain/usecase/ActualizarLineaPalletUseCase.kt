package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.PalletPort
import java.math.BigDecimal

/** Edita una línea ya existente — a diferencia de Agregar, aquí no hay regla de "sumar", se sobreescribe. */
class ActualizarLineaPalletUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(
        id: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal
    ) = palletPort.actualizarDetalle(id, productoTerminadoId, cajas, kilogramos, porcentajeMateriaSeca)
}
