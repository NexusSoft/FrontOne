package com.frontone.android.domain.model

import java.math.BigDecimal

/**
 * Huerta con coordenadas capturadas — proyección ligera para el mapa del submódulo
 * Acopio > Huertas (no es el DTO completo de escritorio, solo lo que necesita un pin
 * + su tarjeta de info). Ver `Catalogos.sp_Huerta_ObtenerTop100ConCoordenadas`/
 * `sp_Huerta_BuscarConCoordenadas`.
 */
data class HuertaMapa(
    val id: Int,
    val nombre: String,
    val registroSagarpa: String?,
    val productorNombre: String,
    val latitud: BigDecimal,
    val longitud: BigDecimal
)
