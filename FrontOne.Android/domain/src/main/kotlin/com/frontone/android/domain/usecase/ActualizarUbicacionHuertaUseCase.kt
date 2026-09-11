package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.HuertaPort
import java.math.BigDecimal

/**
 * Corrige la Latitud/Longitud de una huerta ya existente desde el mapa móvil — única
 * escritura permitida sobre Huerta desde Android (el resto del catálogo se queda
 * exclusivo de escritorio, ver CLAUDE.md). Valida el mismo rango que
 * `HuertaService.cs` en escritorio (mismos mensajes, para que la regla de negocio se
 * vea igual en ambas plataformas aunque hoy vivan duplicadas).
 */
class ActualizarUbicacionHuertaUseCase(private val port: HuertaPort) {
    suspend operator fun invoke(id: Int, latitud: BigDecimal, longitud: BigDecimal) {
        require(latitud >= BigDecimal(-90) && latitud <= BigDecimal(90)) {
            "La latitud debe estar entre -90 y 90 grados"
        }
        require(longitud >= BigDecimal(-180) && longitud <= BigDecimal(180)) {
            "La longitud debe estar entre -180 y 180 grados"
        }
        port.actualizarUbicacion(id, latitud, longitud)
    }
}
