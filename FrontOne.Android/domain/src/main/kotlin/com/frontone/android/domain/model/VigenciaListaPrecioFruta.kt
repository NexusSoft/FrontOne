package com.frontone.android.domain.model

import java.time.LocalDate

/**
 * Una fecha (+ productor opcional, `null` = lista general) con precios ya guardados en
 * `Acopio.ListaPrecioFruta` — universo de vigencias que el selector "Cargar Precios" de
 * la Calculadora ofrece para elegir. Ver `Acopio.sp_ListaPrecioFruta_ObtenerFechas`.
 */
data class VigenciaListaPrecioFruta(
    val fecha: LocalDate,
    val productorId: Int?,
    val productorNombre: String?
)
