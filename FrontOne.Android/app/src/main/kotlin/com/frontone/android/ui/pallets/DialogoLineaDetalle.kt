package com.frontone.android.ui.pallets

import androidx.activity.compose.BackHandler
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.frontone.android.domain.model.LoteEnProceso
import com.frontone.android.domain.model.PalletDetalle
import com.frontone.android.domain.model.PresentacionProducto
import com.frontone.android.domain.model.ProductoTerminado
import java.math.BigDecimal

/**
 * Agregar/editar una línea de detalle — equivalente a PalletDetalleCapturaForm.cs,
 * reconstruido como hoja inferior sobre el diseño real (`Pallets.dc.html`) en vez del
 * `AlertDialog` genérico anterior. El picker de Lote en Proceso solo aparece al agregar (nunca al
 * editar, mismo criterio de escritorio: el Lote/Corrida de una línea ya capturada no se puede
 * cambiar). El producto queda fijo al del encabezado si el pallet no es mixto; si es mixto, se
 * busca por texto (Catalogos.sp_ProductoTerminado_Buscar) — al editar una línea de un pallet
 * mixto, el campo llega precargado con el producto actual de esa línea (antes se abría vacío,
 * obligando a rebuscarlo aunque no se fuera a cambiar). El campo de cantidad es condicional:
 * Cajas si la línea es Presentación Caja, Kilogramos si es Granel — nunca ambos a la vez.
 */
@Composable
fun DialogoLineaDetalle(
    lineaExistente: PalletDetalle?,
    esMixto: Boolean,
    productoEncabezado: ProductoTerminado?,
    lotesEnProceso: List<LoteEnProceso>,
    productosBusqueda: List<ProductoTerminado>,
    onBuscarProducto: (String) -> Unit,
    guardando: Boolean,
    mensajeError: String?,
    onDismiss: () -> Unit,
    onGuardar: (corridaId: Int?, productoTerminadoId: Int, cajas: Int?, kilogramos: BigDecimal?, porcentajeMateriaSeca: BigDecimal) -> Unit
) {
    val esEdicion = lineaExistente != null

    // A diferencia del AlertDialog anterior (que Compose cerraba solo con el gesto de regresar),
    // esta hoja inferior es un overlay propio dentro de la misma pantalla — sin este BackHandler
    // el gesto navegaría PalletCapturaScreen hacia atrás con el modal todavía "abierto" en estado.
    BackHandler(onBack = onDismiss)

    var loteSeleccionado by remember { mutableStateOf<LoteEnProceso?>(null) }
    var expandidoLote by remember { mutableStateOf(false) }

    // Producto de la línea: fijo al del encabezado si no es mixto; si es mixto y se está editando,
    // precargado con el producto actual de la línea (id + etiqueta, sin objeto ProductoTerminado
    // completo porque PalletDetalle no trae Presentación); si es mixto y es alta nueva, se busca.
    var productoNuevoSeleccionado by remember { mutableStateOf<ProductoTerminado?>(null) }
    var textoProducto by remember {
        mutableStateOf(
            when {
                !esMixto -> productoEncabezado?.let { "${it.codigoSap} - ${it.descripcionSap}" } ?: ""
                esEdicion -> "${lineaExistente?.productoCodigoSap} - ${lineaExistente?.productoDescripcion}"
                else -> ""
            }
        )
    }
    var expandidoProducto by remember { mutableStateOf(false) }

    val productoIdEfectivo = when {
        !esMixto -> productoEncabezado?.id
        esEdicion -> productoNuevoSeleccionado?.id ?: lineaExistente?.productoTerminadoId
        else -> productoNuevoSeleccionado?.id
    }

    var cajasTexto by remember { mutableStateOf(lineaExistente?.cajas?.toString() ?: "") }
    var kilogramosTexto by remember { mutableStateOf(if (lineaExistente?.cajas == null) lineaExistente?.kilogramos?.toString() ?: "" else "") }

    val porcentajeMateriaSeca = loteSeleccionado?.porcentajeMateriaSeca
        ?: lineaExistente?.porcentajeMateriaSeca
        ?: BigDecimal.ZERO

    // Al editar, la Presentación real de la línea se infiere de qué campo trae valor (Cajas o
    // Kilogramos) — no depende de tener el ProductoTerminado completo, que PalletDetalle no trae.
    // Al agregar, sí depende del producto elegido (encabezado si no es mixto, buscado si es mixto).
    val presentacion = when {
        esEdicion -> if (lineaExistente?.cajas != null) PresentacionProducto.CAJA else PresentacionProducto.GRANEL
        !esMixto -> productoEncabezado?.presentacion ?: PresentacionProducto.CAJA
        else -> productoNuevoSeleccionado?.presentacion ?: PresentacionProducto.CAJA
    }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(Color.Black.copy(alpha = 0.4f))
            .clickable(onClick = onDismiss),
        contentAlignment = Alignment.BottomCenter
    ) {
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .clip(RoundedCornerShape(topStart = 24.dp, topEnd = 24.dp))
                .background(PalFondoModal)
                .clickable(enabled = false, onClick = {})
                .padding(start = 22.dp, end = 22.dp, top = 24.dp, bottom = 28.dp)
        ) {
            Text(if (esEdicion) "Editar línea" else "Agregar línea", fontSize = 19.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)

            if (!esEdicion) {
                CampoSelector(
                    etiqueta = "Lote en proceso",
                    textoSeleccionado = loteSeleccionado?.let { "${it.loteFolio} · ${it.kilosDisponibles} kg disponibles" } ?: "",
                    placeholder = "Selecciona un lote",
                    expandido = expandidoLote,
                    onExpandidoChange = { expandidoLote = it },
                    opciones = lotesEnProceso,
                    etiquetaOpcion = { "${it.loteFolio} · ${it.kilosDisponibles} kg disponibles" },
                    onSeleccionar = { loteSeleccionado = it },
                    habilitado = true,
                    modifier = Modifier.fillMaxWidth().padding(top = 14.dp)
                )
            }

            if (esMixto) {
                CampoBuscableProducto(
                    etiqueta = "Producto",
                    texto = textoProducto,
                    onTextoChange = {
                        textoProducto = it
                        onBuscarProducto(it)
                    },
                    expandido = expandidoProducto,
                    onExpandidoChange = { expandidoProducto = it },
                    opciones = productosBusqueda,
                    onSeleccionar = { producto ->
                        productoNuevoSeleccionado = producto
                        textoProducto = "${producto.codigoSap} - ${producto.descripcionSap}"
                    },
                    habilitado = true,
                    modifier = Modifier.fillMaxWidth().padding(top = 14.dp)
                )
            } else {
                Text(
                    "Producto: $textoProducto",
                    fontSize = 13.5.sp,
                    fontWeight = FontWeight.SemiBold,
                    color = PalTextoProducto,
                    modifier = Modifier.padding(top = 14.dp)
                )
            }

            Column(modifier = Modifier.fillMaxWidth().padding(top = 14.dp)) {
                val etiquetaCampo = if (presentacion == PresentacionProducto.GRANEL) "Kilogramos" else "Cajas"
                Text(etiquetaCampo.uppercase(), fontSize = 12.sp, fontWeight = FontWeight.Bold, letterSpacing = 0.4.sp, color = PalTextoSecundario)
                Box(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(top = 6.dp)
                        .height(48.dp)
                        .clip(RoundedCornerShape(12.dp))
                        .background(Color.White)
                        .border(2.dp, PalAcentoAzul, RoundedCornerShape(12.dp))
                        .padding(horizontal = 14.dp),
                    contentAlignment = Alignment.CenterStart
                ) {
                    if (presentacion == PresentacionProducto.GRANEL) {
                        BasicTextField(
                            value = kilogramosTexto,
                            onValueChange = { kilogramosTexto = it },
                            singleLine = true,
                            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Decimal),
                            textStyle = TextStyle(fontSize = 14.sp, fontWeight = FontWeight.Bold, color = PalTextoTitulo),
                            modifier = Modifier.fillMaxWidth()
                        )
                    } else {
                        BasicTextField(
                            value = cajasTexto,
                            onValueChange = { cajasTexto = it },
                            singleLine = true,
                            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                            textStyle = TextStyle(fontSize = 14.sp, fontWeight = FontWeight.Bold, color = PalTextoTitulo),
                            modifier = Modifier.fillMaxWidth()
                        )
                    }
                }
            }

            Text(
                "% Materia Seca del lote: $porcentajeMateriaSeca",
                fontSize = 13.5.sp,
                fontWeight = FontWeight.SemiBold,
                color = PalTextoProducto,
                modifier = Modifier.padding(top = 14.dp)
            )

            mensajeError?.let {
                Text(it, fontSize = 13.sp, fontWeight = FontWeight.SemiBold, color = PalErrorCorridaFinalizada, modifier = Modifier.padding(top = 10.dp))
            }

            Row(
                modifier = Modifier.fillMaxWidth().padding(top = 16.dp),
                horizontalArrangement = Arrangement.End
            ) {
                Text(
                    "Cancelar",
                    fontSize = 14.5.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color = PalAcentoAzul,
                    modifier = Modifier.clickable(enabled = !guardando, onClick = onDismiss).padding(vertical = 8.dp)
                )
                Text(
                    "Guardar",
                    fontSize = 14.5.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color = PalAcentoAzul,
                    modifier = Modifier
                        .padding(start = 24.dp)
                        .clickable(enabled = !guardando) {
                            val productoId = productoIdEfectivo ?: return@clickable
                            val corridaId = if (esEdicion) null else loteSeleccionado?.corridaId
                            if (!esEdicion && corridaId == null) return@clickable
                            val cajas = if (presentacion == PresentacionProducto.CAJA) cajasTexto.toIntOrNull() else null
                            val kilogramos = if (presentacion == PresentacionProducto.GRANEL) kilogramosTexto.aBigDecimalCapturado() else null
                            onGuardar(corridaId, productoId, cajas, kilogramos, porcentajeMateriaSeca)
                        }
                        .padding(vertical = 8.dp)
                )
            }
        }
    }
}

private fun String.aBigDecimalCapturado(): BigDecimal? = try {
    if (isBlank()) null else BigDecimal(this)
} catch (ex: NumberFormatException) {
    null
}
