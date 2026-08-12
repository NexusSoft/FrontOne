package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.PalletPort

class EliminarPalletUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(id: Int) = palletPort.eliminar(id)
}
