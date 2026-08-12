package com.frontone.android.domain.model

/** Equivalente a LineaProduccionDto.cs (FrontOne C#) — catálogo simple, sin captura móvil. */
data class LineaProduccion(
    val id: Int,
    val nombre: String,
    val activo: Boolean
)
