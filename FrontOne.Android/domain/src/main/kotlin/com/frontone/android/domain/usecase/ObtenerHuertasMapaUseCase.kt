package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.HuertaMapa
import com.frontone.android.domain.port.HuertaPort

class ObtenerHuertasMapaUseCase(private val port: HuertaPort) {
    suspend operator fun invoke(): List<HuertaMapa> = port.obtenerTop100ConCoordenadas()
}
