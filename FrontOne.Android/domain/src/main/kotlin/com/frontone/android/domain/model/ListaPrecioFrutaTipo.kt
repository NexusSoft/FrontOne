package com.frontone.android.domain.model

import java.math.BigDecimal

/**
 * Las 3 columnas de precio de `Acopio.ListaPrecioFruta` — equivalente a
 * `FrontOne.Domain.Constants.ListasPrecioFruta.Nombres` (C#). El selector "Lista a
 * cargar" de la Calculadora decide cuál de las 3 se copia a la columna Precio del
 * simulador; no es un dato propio de la Calculadora, solo elige qué columna leer.
 */
enum class ListaPrecioFrutaTipo(val etiqueta: String) {
    CONVENCIONAL("Convencional"),
    ORGANICA("Orgánica"),
    NACIONAL("Nacional");

    fun precioDe(precio: PrecioCombinacion): BigDecimal = when (this) {
        CONVENCIONAL -> precio.convencional
        ORGANICA -> precio.organico
        NACIONAL -> precio.nacional
    }
}
