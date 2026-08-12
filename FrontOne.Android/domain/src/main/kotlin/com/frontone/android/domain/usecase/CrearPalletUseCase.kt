package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.PalletPort
import java.math.BigDecimal

/** Da de alta el encabezado — equivalente a PalletService.CrearAsync (FrontOne C#). Devuelve el Id nuevo. */
class CrearPalletUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(
        lineaProduccionId: Int,
        esMixto: Boolean,
        productoTerminadoId: Int?,
        pesoReal: BigDecimal?
    ): Int = palletPort.insertar(lineaProduccionId, esMixto, productoTerminadoId, pesoReal)
}
