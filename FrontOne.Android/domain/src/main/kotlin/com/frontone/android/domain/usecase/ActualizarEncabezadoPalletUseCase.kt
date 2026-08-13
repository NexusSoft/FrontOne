package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.PalletPort
import java.math.BigDecimal

class ActualizarEncabezadoPalletUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(
        id: Int,
        lineaProduccionId: Int,
        esMixto: Boolean,
        productoTerminadoId: Int?,
        pesoReal: BigDecimal?
    ) = palletPort.actualizarEncabezado(id, lineaProduccionId, esMixto, productoTerminadoId, pesoReal)
}
