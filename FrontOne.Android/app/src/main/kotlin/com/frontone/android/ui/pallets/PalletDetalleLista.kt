package com.frontone.android.ui.pallets

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material.icons.filled.Edit
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.alpha
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.frontone.android.domain.model.PalletDetalle
import java.math.RoundingMode

/**
 * Líneas de detalle del Pallet — equivalente a la lista de PalletDetalleCapturaForm.cs,
 * reconstruido sobre el diseño real (`Pallets.dc.html`): tarjeta gris-lavanda, botón "Agregar"
 * en pastilla, filas con ícono de editar/eliminar. Botón "Agregar" deshabilitado si [bloqueado],
 * si todavía no hay encabezado guardado ([palletExiste] = false, alta nueva sin guardar), o si
 * el usuario no tiene el permiso "Crear" de la pantalla móvil "Pallets" ([puedeCrear]). Cada fila
 * deshabilitada si `!linea.loteEnProceso` (la corrida detrás de esa línea ya no está abierta), y
 * además Editar requiere "Modificar" ([puedeModificar]) y Eliminar requiere "Eliminar"
 * ([puedeEliminar]).
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
    onEliminarClick: (PalletDetalle) -> Unit,
    modifier: Modifier = Modifier
) {
    Column(
        modifier = modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(20.dp))
            .background(PalFondoTarjeta)
            .padding(18.dp)
    ) {
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Text("Líneas de detalle", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)
            if (!bloqueado && palletExiste && puedeCrear) {
                Row(
                    modifier = Modifier
                        .clip(RoundedCornerShape(20.dp))
                        .background(Color.White)
                        .border(1.5.dp, PalTextoTitulo.copy(alpha = 0.2f), RoundedCornerShape(20.dp))
                        .clickable(onClick = onAgregarClick)
                        .padding(horizontal = 14.dp, vertical = 8.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(6.dp)
                ) {
                    Icon(Icons.Filled.Add, contentDescription = null, tint = PalTextoTitulo, modifier = Modifier.size(14.dp))
                    Text("Agregar", fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)
                }
            }
        }

        if (!palletExiste) {
            Text(
                "Guarda el encabezado primero para poder capturar líneas.",
                fontSize = 13.5.sp,
                fontWeight = FontWeight.SemiBold,
                color = PalTextoTerciario,
                modifier = Modifier.padding(top = 14.dp)
            )
        } else if (detalle.isEmpty()) {
            Text(
                "Sin líneas capturadas todavía.",
                fontSize = 13.5.sp,
                fontWeight = FontWeight.SemiBold,
                color = PalTextoTerciario,
                modifier = Modifier.padding(top = 14.dp)
            )
        } else {
            Column(modifier = Modifier.padding(top = 14.dp)) {
                detalle.forEachIndexed { indice, linea ->
                    FilaDetalle(
                        linea = linea,
                        bloqueado = bloqueado,
                        puedeModificar = puedeModificar,
                        puedeEliminar = puedeEliminar,
                        onEditarClick = { onEditarClick(linea) },
                        onEliminarClick = { onEliminarClick(linea) }
                    )
                    if (indice < detalle.lastIndex) {
                        Box(
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(vertical = 10.dp)
                                .height(1.dp)
                                .background(PalBordeInput)
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
    Column {
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.Top
        ) {
            Text(
                "${linea.productoCodigoSap} - ${linea.productoDescripcion}",
                fontSize = 14.sp,
                fontWeight = FontWeight.ExtraBold,
                color = PalTextoTitulo,
                maxLines = 2,
                overflow = TextOverflow.Ellipsis,
                modifier = Modifier.weight(1f).padding(end = 8.dp)
            )
            Row(horizontalArrangement = Arrangement.spacedBy(14.dp)) {
                Icon(
                    Icons.Filled.Edit,
                    contentDescription = "Editar línea",
                    tint = PalTextoTerciario,
                    modifier = Modifier
                        .size(17.dp)
                        .alpha(if (disponible && puedeModificar) 1f else 0.35f)
                        .clickable(enabled = disponible && puedeModificar, onClick = onEditarClick)
                )
                Icon(
                    Icons.Filled.Delete,
                    contentDescription = "Eliminar línea",
                    tint = PalTextoTerciario,
                    modifier = Modifier
                        .size(17.dp)
                        .alpha(if (disponible && puedeEliminar) 1f else 0.35f)
                        .clickable(enabled = disponible && puedeEliminar, onClick = onEliminarClick)
                )
            }
        }
        Text("Lote ${linea.loteFolio}", fontSize = 13.sp, fontWeight = FontWeight.SemiBold, color = PalTextoTerciario, modifier = Modifier.padding(top = 4.dp))
        val cantidad = linea.cajas?.let { "$it cajas" }
        val kgTexto = linea.kilogramos.setScale(2, RoundingMode.HALF_UP)
        val msTexto = linea.porcentajeMateriaSeca.setScale(2, RoundingMode.HALF_UP)
        Text(
            if (cantidad != null) "$cantidad · $kgTexto kg · MS $msTexto%" else "$kgTexto kg · MS $msTexto%",
            fontSize = 13.sp,
            fontWeight = FontWeight.SemiBold,
            color = PalTextoTerciario,
            modifier = Modifier.padding(top = 2.dp)
        )
        if (!linea.loteEnProceso) {
            Text(
                "Corrida ya finalizada",
                fontSize = 13.sp,
                fontWeight = FontWeight.Bold,
                color = PalErrorCorridaFinalizada,
                modifier = Modifier.padding(top = 2.dp)
            )
        }
    }
}
