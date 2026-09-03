package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.CombinacionMateriaPrima
import com.frontone.android.domain.port.ListaPrecioFrutaPort

class ObtenerCombinacionesMateriaPrimaUseCase(private val port: ListaPrecioFrutaPort) {
    suspend operator fun invoke(): List<CombinacionMateriaPrima> = port.obtenerCombinacionesMateriaPrima()
}
