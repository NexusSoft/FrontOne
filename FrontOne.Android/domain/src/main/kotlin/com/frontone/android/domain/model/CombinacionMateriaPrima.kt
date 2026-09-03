package com.frontone.android.domain.model

/**
 * Combinación real de Categoría×Calibre APEAM que existe en `Catalogos.MateriaPrima` —
 * universo de filas que la Calculadora (simulador de bandas de Acopio) muestra al abrir,
 * antes de cargar cualquier precio. Equivalente a `CombinacionMateriaPrimaDto` (C#). Ver
 * `Acopio.sp_ListaPrecioFruta_ObtenerCombinacionesMateriaPrima`.
 */
data class CombinacionMateriaPrima(
    val categoriaId: Int,
    val categoriaNombre: String,
    val calibreApeamId: Int,
    val calibreApeamNombre: String
)
