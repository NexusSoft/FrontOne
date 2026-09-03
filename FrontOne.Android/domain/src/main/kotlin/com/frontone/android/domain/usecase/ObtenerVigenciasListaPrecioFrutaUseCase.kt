package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.VigenciaListaPrecioFruta
import com.frontone.android.domain.port.ListaPrecioFrutaPort

class ObtenerVigenciasListaPrecioFrutaUseCase(private val port: ListaPrecioFrutaPort) {
    suspend operator fun invoke(): List<VigenciaListaPrecioFruta> = port.obtenerVigencias()
}
