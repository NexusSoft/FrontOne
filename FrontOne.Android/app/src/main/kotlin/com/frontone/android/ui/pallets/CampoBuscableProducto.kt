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
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.frontone.android.domain.model.ProductoTerminado

/**
 * Campo "Producto" buscable — mismo look del diseño real (`Pallets.dc.html`): input de texto con
 * lupa que dispara búsqueda en vivo (`Catalogos.sp_ProductoTerminado_Buscar`, ya expuesta por
 * `PalletCapturaViewModel.buscarProductos`) y una lista de resultados justo debajo. A diferencia
 * del mockup (que la superpone con `position:absolute`), aquí empuja el contenido de abajo — más
 * simple y robusto en Compose que un overlay flotante, sin cambiar el comportamiento funcional.
 * Se usa tanto en el encabezado (pallet no mixto) como en el diálogo de línea (pallet mixto).
 */
@Composable
fun CampoBuscableProducto(
    etiqueta: String,
    texto: String,
    onTextoChange: (String) -> Unit,
    expandido: Boolean,
    onExpandidoChange: (Boolean) -> Unit,
    opciones: List<ProductoTerminado>,
    onSeleccionar: (ProductoTerminado) -> Unit,
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
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(top = 6.dp)
                .height(48.dp)
                .clip(RoundedCornerShape(12.dp))
                .background(if (habilitado) Color.White else PalFondoTarjeta)
                .border(1.5.dp, PalBordeInput, RoundedCornerShape(12.dp))
                .padding(horizontal = 14.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(8.dp)
        ) {
            Icon(Icons.Filled.Search, contentDescription = null, tint = PalTextoSecundario, modifier = Modifier.size(15.dp))
            Box(modifier = Modifier.fillMaxWidth()) {
                if (texto.isEmpty()) {
                    Text("Buscar producto por nombre o SAP", fontSize = 13.5.sp, fontWeight = FontWeight.Bold, color = PalTextoSecundario)
                }
                BasicTextField(
                    value = texto,
                    onValueChange = {
                        onTextoChange(it)
                        onExpandidoChange(true)
                    },
                    enabled = habilitado,
                    singleLine = true,
                    textStyle = TextStyle(fontSize = 13.5.sp, fontWeight = FontWeight.Bold, color = PalTextoTitulo),
                    modifier = Modifier.fillMaxWidth()
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
                if (opciones.isEmpty()) {
                    Text(
                        "Sin resultados",
                        fontSize = 13.sp,
                        fontWeight = FontWeight.SemiBold,
                        color = PalTextoSecundario,
                        modifier = Modifier.padding(horizontal = 14.dp, vertical = 11.dp)
                    )
                } else {
                    LazyColumn(modifier = Modifier.heightIn(max = 190.dp)) {
                        items(opciones, key = { it.id }) { producto ->
                            Text(
                                "${producto.codigoSap} - ${producto.descripcionSap}",
                                fontSize = 13.sp,
                                fontWeight = FontWeight.Bold,
                                color = PalTextoTitulo,
                                modifier = Modifier
                                    .fillMaxWidth()
                                    .clickable {
                                        onSeleccionar(producto)
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
}
