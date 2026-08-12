package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.LineaProduccion
import com.frontone.android.domain.port.LineaProduccionPort

class ObtenerLineasProduccionUseCase(private val lineaProduccionPort: LineaProduccionPort) {
    suspend operator fun invoke(): List<LineaProduccion> = lineaProduccionPort.obtener()
}
