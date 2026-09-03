package com.frontone.android.domain.model

import java.math.BigDecimal

/**
 * Precio de una combinación Categoría×Calibre para una vigencia concreta de
 * `Acopio.ListaPrecioFruta` ya guardada — las 3 columnas (Convencional/Orgánica/Nacional)
 * llegan juntas, [ListaPrecioFrutaTipo] decide cuál de las 3 usa la Calculadora. Ver
 * `Acopio.sp_ListaPrecioFruta_ObtenerPorFecha`.
 */
data class PrecioCombinacion(
    val categoriaId: Int,
    val calibreApeamId: Int,
    val convencional: BigDecimal,
    val organico: BigDecimal,
    val nacional: BigDecimal
)
