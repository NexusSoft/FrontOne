package com.frontone.android.ui.pallets

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material3.Card
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.FilterChip
import androidx.compose.material3.FloatingActionButton
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.EstatusPallet
import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.tienePermiso

/** Equivalente móvil del listado de PalletsForm.cs — sin edición de grid, solo consulta + acceso a captura. */
@OptIn(ExperimentalMaterial3Api::class)
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

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Pallets") },
                navigationIcon = {
                    IconButton(onClick = onVolverClick) {
                        Icon(Icons.Filled.ArrowBack, contentDescription = "Volver")
                    }
                }
            )
        },
        floatingActionButton = {
            if (puedeCrear) {
                FloatingActionButton(onClick = onNuevoClick) {
                    Icon(Icons.Filled.Add, contentDescription = "Nuevo pallet")
                }
            }
        }
    ) { paddingInterno ->
        Column(modifier = Modifier.fillMaxSize().padding(paddingInterno)) {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 16.dp, vertical = 10.dp),
                horizontalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                FilterChip(
                    selected = filtroEstatus == null,
                    onClick = { viewModel.cambiarFiltro(null) },
                    label = { Text("Todos") }
                )
                EstatusPallet.entries.forEach { estatus ->
                    FilterChip(
                        selected = filtroEstatus == estatus,
                        onClick = { viewModel.cambiarFiltro(estatus) },
                        label = { Text(estatus.etiqueta()) }
                    )
                }
            }

            when (val estadoActual = estado) {
                is EstadoPalletsLista.Cargando -> Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    CircularProgressIndicator()
                }

                is EstadoPalletsLista.Error -> Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    Text(estadoActual.mensaje, color = MaterialTheme.colorScheme.error, modifier = Modifier.padding(24.dp))
                }

                is EstadoPalletsLista.Cargado -> {
                    val pallets = remember(estadoActual.pallets, filtroEstatus) {
                        if (filtroEstatus == null) estadoActual.pallets else estadoActual.pallets.filter { it.estatus == filtroEstatus }
                    }
                    if (pallets.isEmpty()) {
                        Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                            Text("No hay pallets para mostrar.", color = MaterialTheme.colorScheme.onSurfaceVariant)
                        }
                    } else {
                        LazyColumn(
                            modifier = Modifier.fillMaxSize(),
                            contentPadding = androidx.compose.foundation.layout.PaddingValues(16.dp),
                            verticalArrangement = Arrangement.spacedBy(10.dp)
                        ) {
                            items(pallets, key = { it.id }) { pallet ->
                                TarjetaPallet(pallet = pallet, onClick = { onPalletClick(pallet.id) })
                            }
                        }
                    }
                }
            }
        }
    }
}

@Composable
private fun TarjetaPallet(pallet: Pallet, onClick: () -> Unit) {
    Card(modifier = Modifier.fillMaxWidth(), onClick = onClick) {
        Row(
            modifier = Modifier.padding(14.dp),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Column(modifier = Modifier.weight(1f)) {
                Text("Folio ${pallet.folio}", fontWeight = FontWeight.Bold)
                Text(pallet.lineaProduccionNombre, color = MaterialTheme.colorScheme.onSurfaceVariant)
                val descripcion = pallet.productoDescripcion.ifBlank { "Sin producto" }
                Text(descripcion, color = MaterialTheme.colorScheme.onSurfaceVariant)
                Text(
                    if (pallet.totalCajas > 0) "${pallet.totalCajas} cajas · ${pallet.totalKilogramos} kg" else "${pallet.totalKilogramos} kg",
                    color = MaterialTheme.colorScheme.onSurfaceVariant
                )
            }
            Box(
                modifier = Modifier
                    .clip(RoundedCornerShape(20.dp))
                    .background(pallet.estatus.color().copy(alpha = 0.15f))
                    .padding(horizontal = 10.dp, vertical = 6.dp),
                contentAlignment = Alignment.Center
            ) {
                Text(pallet.estatus.etiqueta(), color = pallet.estatus.color(), fontWeight = FontWeight.SemiBold)
            }
        }
    }
}
