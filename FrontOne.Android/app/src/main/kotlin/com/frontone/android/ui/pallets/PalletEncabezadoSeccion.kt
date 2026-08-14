package com.frontone.android.ui.pallets

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Check
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
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
import com.frontone.android.domain.model.LineaProduccion
import com.frontone.android.domain.model.ProductoTerminado

/**
 * Encabezado del Pallet — equivalente al panel superior de PalletEditarForm.cs (Línea de
 * Producción, Es Mixto, Producto de referencia, Peso Real, Guardar), reconstruido sobre el
 * diseño real (`Pallets.dc.html`): tarjeta gris-lavanda, checkbox propio y campo de Producto
 * buscable. Todo deshabilitado si [bloqueado] (mismo criterio de escritorio: un pallet bloqueado
 * no se puede modificar) — el campo de Producto además solo aparece si el pallet NO es mixto
 * (regla real: un pallet mixto no tiene un producto único de encabezado).
 */
@Composable
fun PalletEncabezadoSeccion(
    lineasProduccion: List<LineaProduccion>,
    lineaProduccionId: Int?,
    onLineaProduccionChange: (Int) -> Unit,
    esMixto: Boolean,
    onEsMixtoChange: (Boolean) -> Unit,
    textoProducto: String,
    onTextoProductoChange: (String) -> Unit,
    expandidoProducto: Boolean,
    onExpandidoProductoChange: (Boolean) -> Unit,
    productosBusqueda: List<ProductoTerminado>,
    onProductoSeleccionado: (ProductoTerminado) -> Unit,
    pesoReal: String,
    onPesoRealChange: (String) -> Unit,
    bloqueado: Boolean,
    puedeGuardar: Boolean,
    guardando: Boolean,
    onGuardarClick: () -> Unit,
    modifier: Modifier = Modifier
) {
    var expandidoLinea by remember { mutableStateOf(false) }
    val lineaSeleccionada = lineasProduccion.firstOrNull { it.id == lineaProduccionId }

    Column(
        modifier = modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(20.dp))
            .background(PalFondoTarjeta)
            .padding(18.dp)
    ) {
        Text("Encabezado", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)

        CampoSelector(
            etiqueta = "Línea de Producción",
            textoSeleccionado = lineaSeleccionada?.nombre ?: "",
            placeholder = "Selecciona una línea",
            expandido = expandidoLinea,
            onExpandidoChange = { expandidoLinea = it },
            opciones = lineasProduccion,
            etiquetaOpcion = { it.nombre },
            onSeleccionar = { onLineaProduccionChange(it.id) },
            habilitado = !bloqueado,
            modifier = Modifier.fillMaxWidth().padding(top = 14.dp)
        )

        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(top = 14.dp)
                .clickable(enabled = !bloqueado) { onEsMixtoChange(!esMixto) },
            verticalAlignment = Alignment.CenterVertically
        ) {
            Box(
                modifier = Modifier
                    .size(22.dp)
                    .clip(RoundedCornerShape(5.dp))
                    .background(if (esMixto) PalAcentoAzul else Color.White)
                    .border(1.5.dp, PalTextoSecundario, RoundedCornerShape(5.dp)),
                contentAlignment = Alignment.Center
            ) {
                if (esMixto) {
                    Icon(Icons.Filled.Check, contentDescription = null, tint = Color.White, modifier = Modifier.size(13.dp))
                }
            }
            Text(
                "Pallet mixto (varios productos por línea)",
                fontSize = 14.sp,
                fontWeight = FontWeight.SemiBold,
                color = PalTextoProducto,
                modifier = Modifier.padding(start = 10.dp)
            )
        }

        if (!esMixto) {
            CampoBuscableProducto(
                etiqueta = "Producto",
                texto = textoProducto,
                onTextoChange = onTextoProductoChange,
                expandido = expandidoProducto,
                onExpandidoChange = onExpandidoProductoChange,
                opciones = productosBusqueda,
                onSeleccionar = onProductoSeleccionado,
                habilitado = !bloqueado,
                modifier = Modifier.fillMaxWidth().padding(top = 14.dp)
            )
        }

        Column(modifier = Modifier.fillMaxWidth().padding(top = 14.dp)) {
            Text(
                "PESO REAL (OPCIONAL)",
                fontSize = 12.sp,
                fontWeight = FontWeight.Bold,
                letterSpacing = 0.4.sp,
                color = PalTextoSecundario
            )
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(top = 6.dp)
                    .height(48.dp)
                    .clip(RoundedCornerShape(12.dp))
                    .background(if (bloqueado) PalFondoTarjeta else Color.White)
                    .border(1.5.dp, PalBordeInput, RoundedCornerShape(12.dp))
                    .padding(horizontal = 14.dp),
                contentAlignment = Alignment.CenterStart
            ) {
                if (pesoReal.isEmpty()) {
                    Text("Peso Real (opcional)", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = PalTextoSecundario)
                }
                BasicTextField(
                    value = pesoReal,
                    onValueChange = { if (!bloqueado) onPesoRealChange(it) },
                    enabled = !bloqueado,
                    singleLine = true,
                    keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Decimal),
                    textStyle = TextStyle(fontSize = 14.sp, fontWeight = FontWeight.Bold, color = PalTextoTitulo),
                    modifier = Modifier.fillMaxWidth()
                )
            }
        }

        if (!bloqueado && puedeGuardar) {
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(top = 16.dp)
                    .height(50.dp)
                    .clip(RoundedCornerShape(16.dp))
                    .background(if (guardando) PalAcentoAzul.copy(alpha = 0.6f) else PalAcentoAzul)
                    .clickable(enabled = !guardando, onClick = onGuardarClick),
                contentAlignment = Alignment.Center
            ) {
                Row(verticalAlignment = Alignment.CenterVertically) {
                    if (guardando) {
                        CircularProgressIndicator(modifier = Modifier.size(20.dp).padding(end = 8.dp), color = Color.White, strokeWidth = 2.dp)
                    }
                    Text("Guardar", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                }
            }
        }
    }
}
