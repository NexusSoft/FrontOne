package com.frontone.android.ui.pallets

import androidx.compose.ui.graphics.Color
import com.frontone.android.domain.model.EstatusPallet

/** Texto y color de cada Estatus — mismos 6 valores que el combo de escritorio (PalletEditarForm.cs). */
fun EstatusPallet.etiqueta(): String = when (this) {
    EstatusPallet.VACIO -> "Vacío"
    EstatusPallet.INCOMPLETO -> "Incompleto"
    EstatusPallet.COMPLETO -> "Completo"
    EstatusPallet.EXCEDIDO -> "Excedido"
    EstatusPallet.EMPACADO -> "Empacado"
    EstatusPallet.REEMPACADO -> "Reempacado"
}

fun EstatusPallet.color(): Color = when (this) {
    EstatusPallet.VACIO -> Color(0xFF9A9EB0)
    EstatusPallet.INCOMPLETO -> Color(0xFFE0A72E)
    EstatusPallet.COMPLETO -> Color(0xFF2F9E6E)
    EstatusPallet.EXCEDIDO -> Color(0xFFD1495B)
    EstatusPallet.EMPACADO -> Color(0xFF4E6D9C)
    EstatusPallet.REEMPACADO -> Color(0xFFA45FBF)
}
