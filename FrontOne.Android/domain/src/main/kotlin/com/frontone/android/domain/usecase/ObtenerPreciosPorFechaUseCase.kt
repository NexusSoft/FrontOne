package com.frontone.android.domain.usecase

import com.frontone.android.domain.model.PrecioCombinacion
import com.frontone.android.domain.port.ListaPrecioFrutaPort
import java.time.LocalDate

class ObtenerPreciosPorFechaUseCase(private val port: ListaPrecioFrutaPort) {
    suspend operator fun invoke(fecha: LocalDate, productorId: Int?): List<PrecioCombinacion> =
        port.obtenerPorFecha(fecha, productorId)
}
