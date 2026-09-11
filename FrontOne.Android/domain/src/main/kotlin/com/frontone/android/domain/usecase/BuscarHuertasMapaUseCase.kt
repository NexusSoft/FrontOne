package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.HuertaMapa
import com.frontone.android.domain.port.HuertaPort

class BuscarHuertasMapaUseCase(private val port: HuertaPort) {
    suspend operator fun invoke(filtro: String): List<HuertaMapa> = port.buscarConCoordenadas(filtro)
}
