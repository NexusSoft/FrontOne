package com.frontone.android.ui.pallets

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.KeyboardArrowDown
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

/**
 * Selector estilo `<select>` del diseño real — igual look que [CampoBuscableProducto] pero sin
 * búsqueda (listas cortas: Línea de Producción, Lote en proceso). Toca el campo para abrir/cerrar
 * la lista, que empuja el contenido de abajo en vez de superponerse (mismo criterio que el campo
 * buscable, más simple y robusto en Compose que un overlay flotante).
 */
@Composable
fun <T> CampoSelector(
    etiqueta: String,
    textoSeleccionado: String,
    placeholder: String,
    expandido: Boolean,
    onExpandidoChange: (Boolean) -> Unit,
    opciones: List<T>,
    etiquetaOpcion: (T) -> String,
    onSeleccionar: (T) -> Unit,
    habilitado: Boolean,
    modifier: Modifier = Modifier
) {
    Column(modifier = modifier) {
        Text(
            etiqueta.uppercase(),
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
                .background(if (habilitado) Color.White else PalFondoTarjeta)
                .border(1.5.dp, PalBordeInput, RoundedCornerShape(12.dp))
                .let { if (habilitado) it.clickable { onExpandidoChange(!expandido) } else it }
                .padding(horizontal = 14.dp),
            contentAlignment = Alignment.CenterStart
        ) {
            Text(
                textoSeleccionado.ifBlank { placeholder },
                fontSize = 14.5.sp,
                fontWeight = FontWeight.Bold,
                color = if (textoSeleccionado.isBlank()) PalTextoSecundario else PalTextoTitulo,
                maxLines = 1,
                overflow = TextOverflow.Ellipsis,
                modifier = Modifier.padding(end = 24.dp)
            )
            if (habilitado) {
                Icon(
                    Icons.Filled.KeyboardArrowDown,
                    contentDescription = null,
                    tint = PalTextoSecundario,
                    modifier = Modifier.size(18.dp).align(Alignment.CenterEnd)
                )
            }
        }

        if (expandido && habilitado) {
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(top = 6.dp)
                    .shadow(elevation = 10.dp, shape = RoundedCornerShape(14.dp), ambientColor = PalTextoTitulo, spotColor = PalTextoTitulo)
                    .clip(RoundedCornerShape(14.dp))
                    .background(Color.White)
                    .border(1.5.dp, PalBordeChip, RoundedCornerShape(14.dp))
            ) {
                LazyColumn(modifier = Modifier.heightIn(max = 190.dp)) {
                    items(opciones) { opcion ->
                        Text(
                            etiquetaOpcion(opcion),
                            fontSize = 13.5.sp,
                            fontWeight = FontWeight.Bold,
                            color = PalTextoTitulo,
                            modifier = Modifier
                                .fillMaxWidth()
                                .clickable {
                                    onSeleccionar(opcion)
                                    onExpandidoChange(false)
                                }
                                .padding(horizontal = 14.dp, vertical = 11.dp)
                        )
                    }
                }
            }
        }
    }
}
