package com.frontone.android.domain.port

import com.frontone.android.domain.model.CombinacionMateriaPrima
import com.frontone.android.domain.model.PrecioCombinacion
import com.frontone.android.domain.model.VigenciaListaPrecioFruta
import java.time.LocalDate

/**
 * Equivalente a `IListaPrecioFrutaRepository` (FrontOne C#) — solo los 3 métodos de
 * lectura que necesita la Calculadora (simulador de bandas), no el CRUD completo del
 * módulo "Lista de Precio Fruta" de escritorio (esa captura se queda exclusivamente en
 * WinForms, ver `CLAUDE.md`).
 */
interface ListaPrecioFrutaPort {
    suspend fun obtenerCombinacionesMateriaPrima(): List<CombinacionMateriaPrima>
    suspend fun obtenerVigencias(): List<VigenciaListaPrecioFruta>
    suspend fun obtenerPorFecha(fecha: LocalDate, productorId: Int?): List<PrecioCombinacion>
}
