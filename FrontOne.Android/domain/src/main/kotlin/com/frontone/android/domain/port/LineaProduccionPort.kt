package com.frontone.android.domain.port

import com.frontone.android.domain.model.LineaProduccion

/** Equivalente a ILineaProduccionRepository (FrontOne C#). */
interface LineaProduccionPort {
    suspend fun obtener(): List<LineaProduccion>
}
