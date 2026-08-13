package com.frontone.android.ui.pallets

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyRow
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.EstatusPallet
import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.tienePermiso
import java.math.RoundingMode
import java.time.format.DateTimeFormatter

/**
 * Equivalente móvil del listado de PalletsForm.cs — implementado sobre el diseño real
 * (`Pallets.dc.html`, mismo proyecto de Design que Login/Inicio, traído vía DesignSync).
 * El mockup incluye una pantalla "Nuevo pallet" placeholder ("Próximamente") que NO se usa
 * aquí — el "+" y cada tarjeta siguen enrutando a la captura real ya construida
 * (PalletCapturaScreen), la del mockup solo cubría el diseño del listado.
 */
private val TextoTitulo = Color(0xFF14162A)
private val TextoSecundario = Color(0xFF9A9EB0)
private val TextoTerciario = Color(0xFF6B6F82)
private val TextoProducto = Color(0xFF3B3E52)
private val FondoPantalla = Color(0xFFF6F6FA)
private val BuscadorBorde = Color(0xFFE6E8F0)
private val FiltroInactivoTexto = Color(0xFF3B3E52)
private val AcentoAzul = Color(0xFF4E6D9C)

@Composable
fun PalletsListaScreen(
    permisos: List<PermisoUsuario>,
    onVolverClick: () -> Unit,
    onPalletClick: (Int) -> Unit,
    onNuevoClick: () -> Unit,
    viewModel: PalletsListaViewModel = hiltViewModel()
) {
    val estado by viewModel.estado.collectAsState()
    val filtroEstatus by viewModel.filtroEstatus.collectAsState()
    val puedeCrear = remember(permisos) { permisos.tienePermiso("Pallets", "Crear") }
    var busqueda by remember { mutableStateOf("") }

    Surface(modifier = Modifier.fillMaxSize(), color = FondoPantalla) {
        Box(modifier = Modifier.fillMaxSize()) {
            Column(modifier = Modifier.fillMaxSize()) {
                // ---------- Encabezado ----------
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(start = 24.dp, end = 24.dp, top = 44.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(12.dp)
                ) {
                    BotonRedondo(onClick = onVolverClick) {
                        Icon(Icons.Filled.ArrowBack, contentDescription = "Volver", tint = TextoTitulo, modifier = Modifier.size(18.dp))
                    }
                    Text("Pallets", fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = TextoTitulo)
                }

                // ---------- Buscador ----------
                Row(
                    modifier = Modifier
                        .padding(start = 24.dp, end = 24.dp, top = 16.dp)
                        .fillMaxWidth()
                        .height(46.dp)
                        .clip(RoundedCornerShape(14.dp))
                        .background(Color.White)
                        .border(1.5.dp, BuscadorBorde, RoundedCornerShape(14.dp))
                        .padding(horizontal = 14.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Icon(Icons.Filled.Search, contentDescription = null, tint = TextoSecundario, modifier = Modifier.size(16.dp))
                    Box(modifier = Modifier.fillMaxWidth()) {
                        if (busqueda.isEmpty()) {
                            Text("Buscar folio o producto", fontSize = 14.sp, fontWeight = FontWeight.SemiBold, color = TextoSecundario)
                        }
                        BasicTextField(
                            value = busqueda,
                            onValueChange = { busqueda = it },
                            singleLine = true,
                            textStyle = TextStyle(fontSize = 14.sp, fontWeight = FontWeight.SemiBold, color = TextoTitulo),
                            modifier = Modifier.fillMaxWidth()
                        )
                    }
                }

                // ---------- Chips de filtro por Estatus ----------
                LazyRow(
                    modifier = Modifier.padding(top = 14.dp, bottom = 4.dp),
                    contentPadding = PaddingValues(horizontal = 24.dp),
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    item {
                        ChipFiltro(texto = "Todos", activo = filtroEstatus == null, onClick = { viewModel.cambiarFiltro(null) })
                    }
                    items(EstatusPallet.entries) { estatus ->
                        ChipFiltro(
                            texto = estatus.etiqueta(),
                            activo = filtroEstatus == estatus,
                            onClick = { viewModel.cambiarFiltro(estatus) }
                        )
                    }
                }

                when (val estadoActual = estado) {
                    is EstadoPalletsLista.Cargando -> Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                        CircularProgressIndicator(color = AcentoAzul)
                    }

                    is EstadoPalletsLista.Error -> Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                        Text(estadoActual.mensaje, color = MaterialTheme.colorScheme.error, modifier = Modifier.padding(24.dp))
                    }

                    is EstadoPalletsLista.Cargado -> {
                        val q = busqueda.trim()
                        val pallets = remember(estadoActual.pallets, filtroEstatus, q) {
                            estadoActual.pallets
                                .filter { filtroEstatus == null || it.estatus == filtroEstatus }
                                .filter {
                                    q.isBlank() ||
                                        it.folio.contains(q, ignoreCase = true) ||
                                        it.productoDescripcion.contains(q, ignoreCase = true)
                                }
                        }
                        if (pallets.isEmpty()) {
                            Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                                Text("No hay pallets para mostrar.", color = TextoSecundario)
                            }
                        } else {
                            LazyColumn(
                                modifier = Modifier.fillMaxSize(),
                                contentPadding = PaddingValues(start = 24.dp, end = 24.dp, top = 20.dp, bottom = 110.dp),
                                verticalArrangement = Arrangement.spacedBy(12.dp)
                            ) {
                                items(pallets, key = { it.id }) { pallet ->
                                    TarjetaPallet(pallet = pallet, onClick = { onPalletClick(pallet.id) })
                                }
                            }
                        }
                    }
                }
            }

            if (puedeCrear) {
                Box(
                    modifier = Modifier
                        .align(Alignment.BottomEnd)
                        .padding(end = 24.dp, bottom = 26.dp)
                        .size(56.dp)
                        .shadow(elevation = 14.dp, shape = RoundedCornerShape(18.dp), ambientColor = AcentoAzul, spotColor = AcentoAzul)
                        .clip(RoundedCornerShape(18.dp))
                        .background(AcentoAzul),
                    contentAlignment = Alignment.Center
                ) {
                    IconButton(onClick = onNuevoClick) {
                        Icon(Icons.Filled.Add, contentDescription = "Nuevo pallet", tint = Color.White, modifier = Modifier.size(24.dp))
                    }
                }
            }
        }
    }
}

@Composable
private fun BotonRedondo(onClick: () -> Unit, contenido: @Composable () -> Unit) {
    Box(
        modifier = Modifier
            .size(38.dp)
            .shadow(elevation = 6.dp, shape = RoundedCornerShape(12.dp), ambientColor = TextoTitulo, spotColor = TextoTitulo)
            .clip(RoundedCornerShape(12.dp))
            .background(Color.White),
        contentAlignment = Alignment.Center
    ) {
        IconButton(onClick = onClick, modifier = Modifier.size(38.dp)) { contenido() }
    }
}

@Composable
private fun ChipFiltro(texto: String, activo: Boolean, onClick: () -> Unit) {
    Box(
        modifier = Modifier
            .shadow(
                elevation = if (activo) 6.dp else 2.dp,
                shape = RoundedCornerShape(20.dp),
                ambientColor = if (activo) AcentoAzul else TextoTitulo,
                spotColor = if (activo) AcentoAzul else TextoTitulo
            )
            .clip(RoundedCornerShape(20.dp))
            .background(if (activo) AcentoAzul else Color.White)
            .clickable(onClick = onClick)
            .padding(horizontal = 16.dp, vertical = 9.dp)
    ) {
        Text(
            texto,
            fontSize = 13.sp,
            fontWeight = FontWeight.ExtraBold,
            color = if (activo) Color.White else FiltroInactivoTexto
        )
    }
}

private val FormateadorFecha = DateTimeFormatter.ofPattern("dd/MM/yyyy")

@Composable
private fun TarjetaPallet(pallet: Pallet, onClick: () -> Unit) {
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .shadow(elevation = 10.dp, shape = RoundedCornerShape(18.dp), ambientColor = TextoTitulo, spotColor = TextoTitulo)
            .clip(RoundedCornerShape(18.dp))
            .background(Color.White)
            .clickable(onClick = onClick)
    ) {
        // ---------- Banda de estatus ----------
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .background(pallet.estatus.color())
                .padding(horizontal = 16.dp, vertical = 9.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Text(pallet.folio, fontSize = 13.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
            Text(
                pallet.fechaCreacion.format(FormateadorFecha),
                fontSize = 12.5.sp,
                fontWeight = FontWeight.Bold,
                color = Color.White.copy(alpha = 0.8f)
            )
        }

        // ---------- Cuerpo ----------
        Column(modifier = Modifier.padding(start = 16.dp, end = 16.dp, top = 14.dp, bottom = 16.dp)) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.Top
            ) {
                Text(
                    pallet.lineaProduccionNombre.uppercase(),
                    fontSize = 12.sp,
                    fontWeight = FontWeight.Bold,
                    letterSpacing = 0.3.sp,
                    color = TextoSecundario,
                    maxLines = 1,
                    overflow = TextOverflow.Ellipsis,
                    modifier = Modifier.weight(1f)
                )
                Box(
                    modifier = Modifier
                        .clip(RoundedCornerShape(20.dp))
                        .background(pallet.estatus.color().copy(alpha = 0.1f))
                        .padding(horizontal = 10.dp, vertical = 5.dp)
                ) {
                    Text(pallet.estatus.etiqueta(), fontSize = 11.5.sp, fontWeight = FontWeight.Bold, color = pallet.estatus.color())
                }
            }

            Text(
                pallet.productoDescripcion.ifBlank { "Sin asignar" },
                fontSize = 13.5.sp,
                fontWeight = FontWeight.Bold,
                color = TextoProducto,
                modifier = Modifier.padding(top = 6.dp)
            )

            Row(
                modifier = Modifier.padding(top = 4.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                val kgTexto = pallet.totalKilogramos.setScale(2, RoundingMode.HALF_UP)
                Text("${pallet.totalCajas} cajas · $kgTexto kg", fontSize = 12.5.sp, fontWeight = FontWeight.Bold, color = TextoTerciario)
                if (pallet.productoCodigoSap.isNotBlank()) {
                    Box(modifier = Modifier.size(3.dp).clip(CircleShape).background(TextoSecundario.copy(alpha = 0.6f)))
                    Text("SAP ${pallet.productoCodigoSap}", fontSize = 11.5.sp, fontWeight = FontWeight.Bold, color = TextoSecundario)
                }
            }
        }
    }
}
