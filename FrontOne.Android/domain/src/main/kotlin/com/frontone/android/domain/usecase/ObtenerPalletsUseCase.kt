package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.port.PalletPort

class ObtenerPalletsUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(id: Int? = null): List<Pallet> = palletPort.obtener(id)
}
