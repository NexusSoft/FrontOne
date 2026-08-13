package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.ProductoTerminado
import com.frontone.android.domain.port.ProductoTerminadoPort

/** Sin filtro trae el TOP 1000 (mismo criterio de "buscador embebido" del proyecto); con filtro, Buscar (TOP 500). */
class ObtenerProductosTerminadosUseCase(private val productoTerminadoPort: ProductoTerminadoPort) {
    suspend operator fun invoke(filtro: String = ""): List<ProductoTerminado> =
        if (filtro.isBlank()) productoTerminadoPort.obtenerTop1000() else productoTerminadoPort.buscar(filtro)
}
