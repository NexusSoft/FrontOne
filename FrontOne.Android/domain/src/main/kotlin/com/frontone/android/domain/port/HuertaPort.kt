package com.frontone.android.domain.port

import com.frontone.android.domain.model.HuertaMapa
import java.math.BigDecimal

/**
 * Equivalente a IHuertaRepository (FrontOne C#) — pero solo los 3 métodos que necesita
 * el mapa móvil (consulta con coordenadas + corrección de ubicación). El resto del CRUD
 * de Huerta (alta, edición completa) se queda exclusivo de escritorio, ver CLAUDE.md.
 */
interface HuertaPort {
    suspend fun obtenerTop100ConCoordenadas(): List<HuertaMapa>
    suspend fun buscarConCoordenadas(filtro: String): List<HuertaMapa>
    suspend fun actualizarUbicacion(id: Int, latitud: BigDecimal, longitud: BigDecimal)
}
