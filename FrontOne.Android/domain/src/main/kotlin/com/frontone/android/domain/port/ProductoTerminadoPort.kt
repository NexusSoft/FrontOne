package com.frontone.android.domain.port

import com.frontone.android.domain.model.ProductoTerminado

/** Equivalente a IProductoTerminadoRepository (FrontOne C#) — subconjunto de métodos que consume Pallets. */
interface ProductoTerminadoPort {
    suspend fun obtenerTop1000(): List<ProductoTerminado>
    suspend fun buscar(filtro: String): List<ProductoTerminado>
}
