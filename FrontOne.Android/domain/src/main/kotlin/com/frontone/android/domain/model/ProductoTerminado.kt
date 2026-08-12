package com.frontone.android.domain.model

import java.math.BigDecimal

/** Espejo de Catalogos.ProductoTerminado.Presentacion — determina si una línea de Pallet captura Cajas o Kilogramos. */
enum class PresentacionProducto(val valor: Int) {
    CAJA(1),
    GRANEL(2);

    companion object {
        fun desde(valor: Int): PresentacionProducto = entries.first { it.valor == valor }
    }
}

/**
 * Subconjunto de Catalogos.ProductoTerminado relevante para Pallets — solo los campos que
 * consume la captura móvil (no es el catálogo completo, ese se queda en escritorio).
 */
data class ProductoTerminado(
    val id: Int,
    val codigoSap: String,
    val descripcionSap: String,
    val activo: Boolean,
    val pesoNeto: BigDecimal?,
    val cajasPorPallet: Int?,
    val presentacion: PresentacionProducto
)
