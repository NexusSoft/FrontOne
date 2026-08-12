package com.frontone.android.ui.pallets

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material.icons.filled.Edit
import androidx.compose.material3.Card
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.frontone.android.domain.model.PalletDetalle

/**
 * Líneas de detalle del Pallet — equivalente a la lista de PalletDetalleCapturaForm.cs. Botón
 * "Agregar" deshabilitado si [bloqueado], si todavía no hay encabezado guardado
 * ([palletExiste] = false, alta nueva sin guardar), o si el usuario no tiene el permiso
 * "Crear" de la pantalla móvil "Pallets" ([puedeCrear]). Cada fila deshabilitada si
 * `!linea.loteEnProceso` (la corrida detrás de esa línea ya no está abierta), y además Editar
 * requiere "Modificar" ([puedeModificar]) y Eliminar requiere "Eliminar" ([puedeEliminar]).
 */
@Composable
fun PalletDetalleLista(
    detalle: List<PalletDetalle>,
    palletExiste: Boolean,
    bloqueado: Boolean,
    puedeCrear: Boolean,
    puedeModificar: Boolean,
    puedeEliminar: Boolean,
    onAgregarClick: () -> Unit,
    onEditarClick: (PalletDetalle) -> Unit,
    onEliminarClick: (PalletDetalle) -> Unit
) {
    Card(modifier = Modifier.fillMaxWidth().padding(16.dp)) {
        Column(modifier = Modifier.padding(16.dp)) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                Text("Líneas de detalle", style = MaterialTheme.typography.titleMedium)
                if (!bloqueado && palletExiste && puedeCrear) {
                    OutlinedButton(onClick = onAgregarClick) {
                        Icon(Icons.Filled.Add, contentDescription = null, modifier = Modifier.padding(end = 4.dp))
                        Text("Agregar")
                    }
                }
            }

            if (!palletExiste) {
                Text(
                    "Guarda el encabezado primero para poder capturar líneas.",
                    color = MaterialTheme.colorScheme.onSurfaceVariant,
                    modifier = Modifier.padding(top = 12.dp)
                )
            } else if (detalle.isEmpty()) {
                Text(
                    "Sin líneas capturadas todavía.",
                    color = MaterialTheme.colorScheme.onSurfaceVariant,
                    modifier = Modifier.padding(top = 12.dp)
                )
            } else {
                Column(modifier = Modifier.padding(top = 8.dp), verticalArrangement = Arrangement.spacedBy(4.dp)) {
                    detalle.forEach { linea ->
                        FilaDetalle(
                            linea = linea,
                            bloqueado = bloqueado,
                            puedeModificar = puedeModificar,
                            puedeEliminar = puedeEliminar,
                            onEditarClick = { onEditarClick(linea) },
                            onEliminarClick = { onEliminarClick(linea) }
                        )
                    }
                }
            }
        }
    }
}

@Composable
private fun FilaDetalle(
    linea: PalletDetalle,
    bloqueado: Boolean,
    puedeModificar: Boolean,
    puedeEliminar: Boolean,
    onEditarClick: () -> Unit,
    onEliminarClick: () -> Unit
) {
    val disponible = !bloqueado && linea.loteEnProceso
    Row(
        modifier = Modifier.fillMaxWidth().padding(vertical = 6.dp),
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        Column(modifier = Modifier.weight(1f)) {
            Text("${linea.productoCodigoSap} - ${linea.productoDescripcion}", fontWeight = FontWeight.SemiBold)
            Text("Lote ${linea.loteFolio}", color = MaterialTheme.colorScheme.onSurfaceVariant)
            val cantidad = linea.cajas?.let { "$it cajas" } ?: "${linea.kilogramos} kg"
            Text(
                "$cantidad · ${linea.kilogramos} kg · MS ${linea.porcentajeMateriaSeca}%",
                color = MaterialTheme.colorScheme.onSurfaceVariant
            )
            if (!linea.loteEnProceso) {
                Text("Corrida ya finalizada", color = MaterialTheme.colorScheme.error)
            }
        }
        Row {
            IconButton(onClick = onEditarClick, enabled = disponible && puedeModificar) {
                Icon(Icons.Filled.Edit, contentDescription = "Editar línea")
            }
            IconButton(onClick = onEliminarClick, enabled = disponible && puedeEliminar) {
                Icon(Icons.Filled.Delete, contentDescription = "Eliminar línea")
            }
        }
    }
}
