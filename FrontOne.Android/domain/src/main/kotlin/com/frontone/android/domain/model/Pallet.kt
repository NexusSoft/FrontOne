package com.frontone.android.domain.model

import java.math.BigDecimal
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.LocalTime

/** Espejo de Produccion.Pallet.Estatus — siempre recalculado por el servidor (sp_Pallet_Recalcular), nunca capturado a mano. */
enum class EstatusPallet(val valor: Int) {
    VACIO(1),
    INCOMPLETO(2),
    COMPLETO(3),
    EXCEDIDO(4),
    EMPACADO(5),
    REEMPACADO(6);

    companion object {
        fun desde(valor: Int): EstatusPallet = entries.first { it.valor == valor }
    }
}

/** Equivalente a PalletDto.cs (FrontOne C#) — encabezado, tal como lo entrega sp_Pallet_Obtener. */
data class Pallet(
    val id: Int,
    val folio: String,
    val fechaCreacion: LocalDate,
    val horaCreacion: LocalTime,
    val estatus: EstatusPallet,
    val lineaProduccionId: Int,
    val lineaProduccionNombre: String,
    val esMixto: Boolean,
    val productoTerminadoId: Int?,
    val porcentajeMateriaSeca: BigDecimal,
    val pesoReal: BigDecimal?,
    val bloqueado: Boolean,
    val fechaBloqueo: LocalDateTime?,
    val noReempaque: Int?,
    val primeraCorrida: Boolean,
    val totalCajas: Int,
    val totalKilogramos: BigDecimal,
    val productoDescripcion: String,
    val productoCodigoSap: String,
    val fechaCreacionRegistro: LocalDateTime,
    val esNeutro: Boolean
)

/** Equivalente a PalletDetalleDto.cs — una línea de detalle, tal como la entrega sp_Pallet_ObtenerDetalle. */
data class PalletDetalle(
    val id: Int,
    val palletId: Int,
    val corridaId: Int,
    val loteId: Int,
    val loteFolio: String,
    val productoTerminadoId: Int,
    val productoCodigoSap: String,
    val productoDescripcion: String,
    val cajas: Int?,
    val kilogramos: BigDecimal,
    val porcentajeMateriaSeca: BigDecimal,
    val cajasPorPallet: Int?,
    val loteEnProceso: Boolean
)

/** Fila del picker de Lote en Proceso al agregar una línea de detalle (sp_Pallet_ObtenerLotesEnProceso). */
data class LoteEnProceso(
    val corridaId: Int,
    val loteId: Int,
    val loteFolio: String,
    val codigoTrazabilidad: String,
    val lineaProduccionId: Int,
    val lineaProduccionNombre: String,
    val porcentajeMateriaSeca: BigDecimal,
    val kilosAProcesar: BigDecimal,
    val kilosProcesados: BigDecimal,
    val kilosDisponibles: BigDecimal,
    val huertaNombre: String?,
    val registroSagarpa: String?,
    val productorNombre: String?
)
