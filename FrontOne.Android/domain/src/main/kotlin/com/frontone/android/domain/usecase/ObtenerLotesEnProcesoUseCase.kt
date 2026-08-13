package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.LoteEnProceso
import com.frontone.android.domain.port.PalletPort

class ObtenerLotesEnProcesoUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(lineaProduccionId: Int? = null): List<LoteEnProceso> =
        palletPort.obtenerLotesEnProceso(lineaProduccionId)
}
