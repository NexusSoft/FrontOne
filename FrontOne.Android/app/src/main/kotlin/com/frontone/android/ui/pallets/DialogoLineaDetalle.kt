package com.frontone.android.ui.pallets

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ExposedDropdownMenuBox
import androidx.compose.material3.ExposedDropdownMenuDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import com.frontone.android.domain.model.LoteEnProceso
import com.frontone.android.domain.model.PalletDetalle
import com.frontone.android.domain.model.PresentacionProducto
import com.frontone.android.domain.model.ProductoTerminado
import java.math.BigDecimal

/**
 * Agregar/editar una línea de detalle — equivalente a PalletDetalleCapturaForm.cs. El picker de
 * Lote en Proceso solo aparece al agregar (nunca al editar, mismo criterio de escritorio: el
 * Lote/Corrida de una línea ya capturada no se puede cambiar). El producto queda fijo al del
 * encabezado si el pallet no es mixto; si es mixto, se busca por texto (Catalogos.sp_
 * ProductoTerminado_Buscar). El campo de cantidad es condicional: Cajas si el producto es
 * Presentación Caja, Kilogramos si es Granel — nunca ambos a la vez (PalletDetalleCapturaForm.cs).
 */
@OptIn(ExperimentalMaterial3Api::class)
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

    var loteSeleccionado by remember { mutableStateOf<LoteEnProceso?>(null) }
    var productoSeleccionado by remember { mutableStateOf(productoEncabezado) }
    var textoBusquedaProducto by remember { mutableStateOf("") }
    var cajasTexto by remember { mutableStateOf(lineaExistente?.cajas?.toString() ?: "") }
    var kilogramosTexto by remember { mutableStateOf(if (lineaExistente?.cajas == null) lineaExistente?.kilogramos?.toString() ?: "" else "") }

    val porcentajeMateriaSeca = loteSeleccionado?.porcentajeMateriaSeca
        ?: lineaExistente?.porcentajeMateriaSeca
        ?: BigDecimal.ZERO

    val presentacion = productoSeleccionado?.presentacion ?: PresentacionProducto.CAJA

    AlertDialog(
        onDismissRequest = onDismiss,
        title = { Text(if (esEdicion) "Editar línea" else "Agregar línea") },
        text = {
            Column {
                if (!esEdicion) {
                    var expandidoLote by remember { mutableStateOf(false) }
                    ExposedDropdownMenuBox(
                        expanded = expandidoLote,
                        onExpandedChange = { expandidoLote = it },
                        modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
                    ) {
                        OutlinedTextField(
                            value = loteSeleccionado?.let { "${it.loteFolio} · ${it.kilosDisponibles} kg disponibles" } ?: "",
                            onValueChange = {},
                            readOnly = true,
                            label = { Text("Lote en proceso") },
                            trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(expanded = expandidoLote) },
                            modifier = Modifier.fillMaxWidth().menuAnchor()
                        )
                        ExposedDropdownMenu(expanded = expandidoLote, onDismissRequest = { expandidoLote = false }) {
                            if (lotesEnProceso.isEmpty()) {
                                DropdownMenuItem(text = { Text("Sin lotes en proceso") }, onClick = {})
                            }
                            lotesEnProceso.forEach { lote ->
                                DropdownMenuItem(
                                    text = { Text("${lote.loteFolio} · ${lote.kilosDisponibles} kg disponibles") },
                                    onClick = {
                                        loteSeleccionado = lote
                                        expandidoLote = false
                                    }
                                )
                            }
                        }
                    }
                }

                if (esMixto) {
                    OutlinedTextField(
                        value = textoBusquedaProducto,
                        onValueChange = {
                            textoBusquedaProducto = it
                            onBuscarProducto(it)
                        },
                        label = { Text("Buscar producto") },
                        modifier = Modifier.fillMaxWidth().padding(bottom = 8.dp)
                    )
                    LazyColumn(modifier = Modifier.fillMaxWidth().heightIn(max = 160.dp).padding(bottom = 12.dp)) {
                        items(productosBusqueda) { producto ->
                            DropdownMenuItem(
                                text = { Text("${producto.codigoSap} - ${producto.descripcionSap}") },
                                onClick = { productoSeleccionado = producto }
                            )
                        }
                    }
                    productoSeleccionado?.let {
                        Text("Seleccionado: ${it.codigoSap} - ${it.descripcionSap}", modifier = Modifier.padding(bottom = 12.dp))
                    }
                } else {
                    Text(
                        "Producto: ${productoEncabezado?.codigoSap ?: ""} - ${productoEncabezado?.descripcionSap ?: ""}",
                        modifier = Modifier.padding(bottom = 12.dp)
                    )
                }

                if (presentacion == PresentacionProducto.GRANEL) {
                    OutlinedTextField(
                        value = kilogramosTexto,
                        onValueChange = { kilogramosTexto = it },
                        label = { Text("Kilogramos") },
                        keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Decimal),
                        modifier = Modifier.fillMaxWidth().padding(bottom = 8.dp)
                    )
                } else {
                    OutlinedTextField(
                        value = cajasTexto,
                        onValueChange = { cajasTexto = it },
                        label = { Text("Cajas") },
                        keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                        modifier = Modifier.fillMaxWidth().padding(bottom = 8.dp)
                    )
                }

                Text(
                    "% Materia Seca del lote: $porcentajeMateriaSeca",
                    color = MaterialTheme.colorScheme.onSurfaceVariant,
                    modifier = Modifier.padding(bottom = 8.dp)
                )

                mensajeError?.let {
                    Text(it, color = MaterialTheme.colorScheme.error)
                }
            }
        },
        confirmButton = {
            TextButton(
                enabled = !guardando,
                onClick = {
                    val productoId = productoSeleccionado?.id ?: return@TextButton
                    val corridaId = if (esEdicion) null else loteSeleccionado?.corridaId
                    if (!esEdicion && corridaId == null) return@TextButton
                    val cajas = if (presentacion == PresentacionProducto.CAJA) cajasTexto.toIntOrNull() else null
                    val kilogramos = if (presentacion == PresentacionProducto.GRANEL) kilogramosTexto.aBigDecimalCapturado() else null
                    onGuardar(corridaId, productoId, cajas, kilogramos, porcentajeMateriaSeca)
                }
            ) {
                Text("Guardar")
            }
        },
        dismissButton = {
            TextButton(onClick = onDismiss, enabled = !guardando) { Text("Cancelar") }
        }
    )
}

private fun String.aBigDecimalCapturado(): BigDecimal? = try {
    if (isBlank()) null else BigDecimal(this)
} catch (ex: NumberFormatException) {
    null
}
