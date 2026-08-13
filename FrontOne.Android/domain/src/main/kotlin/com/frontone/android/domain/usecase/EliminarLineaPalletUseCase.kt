package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.PalletPort

class EliminarLineaPalletUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(id: Int) = palletPort.eliminarDetalle(id)
}
