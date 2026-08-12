package com.frontone.android.ui.pallets

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.Button
import androidx.compose.material3.CheckboxDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.Checkbox
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ExposedDropdownMenuBox
import androidx.compose.material3.ExposedDropdownMenuDefaults
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import com.frontone.android.domain.model.LineaProduccion
import com.frontone.android.domain.model.ProductoTerminado

/**
 * Encabezado del Pallet — equivalente al panel superior de PalletEditarForm.cs (Línea de
 * Producción, Es Mixto, Producto de referencia, Peso Real, Guardar). Todo deshabilitado si
 * [bloqueado] (mismo criterio de escritorio: un pallet bloqueado no se puede modificar).
 */
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun PalletEncabezadoSeccion(
    lineasProduccion: List<LineaProduccion>,
    productos: List<ProductoTerminado>,
    lineaProduccionId: Int?,
    onLineaProduccionChange: (Int) -> Unit,
    esMixto: Boolean,
    onEsMixtoChange: (Boolean) -> Unit,
    productoTerminadoId: Int?,
    onProductoChange: (Int) -> Unit,
    pesoReal: String,
    onPesoRealChange: (String) -> Unit,
    bloqueado: Boolean,
    puedeGuardar: Boolean,
    guardando: Boolean,
    onGuardarClick: () -> Unit
) {
    Card(modifier = Modifier.fillMaxWidth().padding(16.dp)) {
        Column(modifier = Modifier.padding(16.dp)) {
            Text("Encabezado", style = MaterialTheme.typography.titleMedium, modifier = Modifier.padding(bottom = 12.dp))

            var expandidoLinea by remember { mutableStateOf(false) }
            val lineaSeleccionada = lineasProduccion.firstOrNull { it.id == lineaProduccionId }
            ExposedDropdownMenuBox(
                expanded = expandidoLinea && !bloqueado,
                onExpandedChange = { if (!bloqueado) expandidoLinea = it },
                modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
            ) {
                OutlinedTextField(
                    value = lineaSeleccionada?.nombre ?: "",
                    onValueChange = {},
                    readOnly = true,
                    enabled = !bloqueado,
                    label = { Text("Línea de Producción") },
                    trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(expanded = expandidoLinea) },
                    modifier = Modifier.fillMaxWidth().menuAnchor()
                )
                ExposedDropdownMenu(
                    expanded = expandidoLinea,
                    onDismissRequest = { expandidoLinea = false }
                ) {
                    lineasProduccion.forEach { linea ->
                        DropdownMenuItem(
                            text = { Text(linea.nombre) },
                            onClick = {
                                onLineaProduccionChange(linea.id)
                                expandidoLinea = false
                            }
                        )
                    }
                }
            }

            Row(
                modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp),
                verticalAlignment = Alignment.CenterVertically
            ) {
                Checkbox(
                    checked = esMixto,
                    onCheckedChange = { if (!bloqueado) onEsMixtoChange(it) },
                    enabled = !bloqueado,
                    colors = CheckboxDefaults.colors()
                )
                Text("Pallet mixto (varios productos por línea)")
            }

            if (!esMixto) {
                var expandidoProducto by remember { mutableStateOf(false) }
                val productoSeleccionado = productos.firstOrNull { it.id == productoTerminadoId }
                ExposedDropdownMenuBox(
                    expanded = expandidoProducto && !bloqueado,
                    onExpandedChange = { if (!bloqueado) expandidoProducto = it },
                    modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
                ) {
                    OutlinedTextField(
                        value = productoSeleccionado?.let { "${it.codigoSap} - ${it.descripcionSap}" } ?: "",
                        onValueChange = {},
                        readOnly = true,
                        enabled = !bloqueado,
                        label = { Text("Producto") },
                        trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(expanded = expandidoProducto) },
                        modifier = Modifier.fillMaxWidth().menuAnchor()
                    )
                    ExposedDropdownMenu(
                        expanded = expandidoProducto,
                        onDismissRequest = { expandidoProducto = false }
                    ) {
                        productos.forEach { producto ->
                            DropdownMenuItem(
                                text = { Text("${producto.codigoSap} - ${producto.descripcionSap}") },
                                onClick = {
                                    onProductoChange(producto.id)
                                    expandidoProducto = false
                                }
                            )
                        }
                    }
                }
            }

            OutlinedTextField(
                value = pesoReal,
                onValueChange = { if (!bloqueado) onPesoRealChange(it) },
                enabled = !bloqueado,
                label = { Text("Peso Real (opcional)") },
                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Decimal),
                modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
            )

            if (!bloqueado && puedeGuardar) {
                Button(onClick = onGuardarClick, enabled = !guardando, modifier = Modifier.fillMaxWidth()) {
                    if (guardando) {
                        CircularProgressIndicator(modifier = Modifier.padding(end = 8.dp), color = MaterialTheme.colorScheme.onPrimary)
                    }
                    Text("Guardar")
                }
            }
        }
    }
}
