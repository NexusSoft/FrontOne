package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.PalletDetalle
import com.frontone.android.domain.port.PalletPort

class ObtenerPalletDetalleUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(palletId: Int): List<PalletDetalle> = palletPort.obtenerDetalle(palletId)
}
