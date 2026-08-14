package com.frontone.android.ui.pallets

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material.icons.filled.Lock
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.AssistChip
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.PalletDetalle
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.tienePermiso

/**
 * Orquestador de la captura de un Pallet — equivalente a PalletEditarForm.cs. Combina
 * [PalletEncabezadoSeccion] + [PalletDetalleLista] + [DialogoLineaDetalle], con Bloquear/Eliminar
 * en la barra superior (mismo patrón de confirmación fuerte que el resto de la app,
 * ver CLAUDE.md "todo evento de eliminar registro pregunta antes de eliminar").
 */
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun PalletCapturaScreen(
    palletIdInicial: Int?,
    claveViewModel: String,
    permisos: List<PermisoUsuario>,
    onVolverClick: () -> Unit,
    onPalletEliminado: () -> Unit,
    viewModel: PalletCapturaViewModel = hiltViewModel(key = claveViewModel)
) {
    LaunchedEffect(Unit) { viewModel.inicializar(palletIdInicial) }

    val estado by viewModel.estado.collectAsState()
    val lotesEnProceso by viewModel.lotesEnProceso.collectAsState()
    val productosBusqueda by viewModel.productosBusqueda.collectAsState()

    val puedeCrear = remember(permisos) { permisos.tienePermiso("Pallets", "Crear") }
    val puedeModificar = remember(permisos) { permisos.tienePermiso("Pallets", "Modificar") }
    val puedeEliminar = remember(permisos) { permisos.tienePermiso("Pallets", "Eliminar") }

    var lineaProduccionId by remember { mutableStateOf<Int?>(null) }
    var esMixto by remember { mutableStateOf(false) }
    var productoTerminadoId by remember { mutableStateOf<Int?>(null) }
    var pesoReal by remember { mutableStateOf("") }
    var mostrarDialogoLinea by remember { mutableStateOf(false) }
    var lineaEnEdicion by remember { mutableStateOf<PalletDetalle?>(null) }
    var mostrarConfirmarBloquear by remember { mutableStateOf(false) }
    var mostrarConfirmarEliminar by remember { mutableStateOf(false) }
    var mensajeErrorLinea by remember { mutableStateOf<String?>(null) }
    var precargado by remember { mutableStateOf(false) }

    LaunchedEffect(estado) {
        val formularioActual = estado as? EstadoPalletCaptura.Formulario ?: return@LaunchedEffect
        if (formularioActual.eliminado) {
            onPalletEliminado()
            return@LaunchedEffect
        }
        if (!precargado) {
            formularioActual.pallet?.let { pallet ->
                lineaProduccionId = pallet.lineaProduccionId
                esMixto = pallet.esMixto
                productoTerminadoId = pallet.productoTerminadoId
                pesoReal = pallet.pesoReal?.toPlainString() ?: ""
            }
            precargado = true
        }
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text(if (palletIdInicial == null) "Nuevo pallet" else "Pallet") },
                navigationIcon = {
                    IconButton(onClick = onVolverClick) {
                        Icon(Icons.Filled.ArrowBack, contentDescription = "Volver")
                    }
                },
                actions = {
                    val formularioActual = estado as? EstadoPalletCaptura.Formulario
                    if (formularioActual?.palletId != null) {
                        val bloqueado = formularioActual.pallet?.bloqueado == true
                        if (!bloqueado) {
                            if (puedeModificar) {
                                IconButton(onClick = { mostrarConfirmarBloquear = true }) {
                                    Icon(Icons.Filled.Lock, contentDescription = "Bloquear pallet")
                                }
                            }
                            if (puedeEliminar) {
                                IconButton(onClick = { mostrarConfirmarEliminar = true }) {
                                    Icon(Icons.Filled.Delete, contentDescription = "Eliminar pallet")
                                }
                            }
                        }
                    }
                }
            )
        }
    ) { paddingInterno ->
        when (val estadoActual = estado) {
            is EstadoPalletCaptura.Cargando -> Box(
                modifier = Modifier.fillMaxSize().padding(paddingInterno),
                contentAlignment = Alignment.Center
            ) { CircularProgressIndicator() }

            is EstadoPalletCaptura.ErrorCarga -> Box(
                modifier = Modifier.fillMaxSize().padding(paddingInterno),
                contentAlignment = Alignment.Center
            ) { Text(estadoActual.mensaje, color = MaterialTheme.colorScheme.error, modifier = Modifier.padding(24.dp)) }

            is EstadoPalletCaptura.Formulario -> {
                val bloqueado = estadoActual.pallet?.bloqueado == true
                Column(
                    modifier = Modifier
                        .fillMaxSize()
                        .padding(paddingInterno)
                        .verticalScroll(rememberScrollState())
                ) {
                    estadoActual.pallet?.let { pallet ->
                        Box(modifier = Modifier.padding(start = 16.dp, top = 12.dp)) {
                            AssistChip(
                                onClick = {},
                                label = {
                                    Text("${pallet.folio} · ${pallet.estatus.etiqueta()} · ${pallet.totalCajas} cajas · ${pallet.totalKilogramos} kg")
                                }
                            )
                        }
                    }

                    PalletEncabezadoSeccion(
                        lineasProduccion = estadoActual.lineasProduccion,
                        productos = estadoActual.productosEncabezado,
                        lineaProduccionId = lineaProduccionId,
                        onLineaProduccionChange = { lineaProduccionId = it },
                        esMixto = esMixto,
                        onEsMixtoChange = { esMixto = it },
                        productoTerminadoId = productoTerminadoId,
                        onProductoChange = { productoTerminadoId = it },
                        pesoReal = pesoReal,
                        onPesoRealChange = { pesoReal = it },
                        bloqueado = bloqueado,
                        puedeGuardar = if (estadoActual.palletId == null) puedeCrear else puedeModificar,
                        guardando = estadoActual.guardandoEncabezado,
                        onGuardarClick = {
                            val lineaId = lineaProduccionId ?: return@PalletEncabezadoSeccion
                            viewModel.guardarEncabezado(
                                lineaProduccionId = lineaId,
                                esMixto = esMixto,
                                productoTerminadoId = if (esMixto) null else productoTerminadoId,
                                pesoReal = pesoReal.aBigDecimalCapturado()
                            )
                        }
                    )

                    estadoActual.mensajeError?.let {
                        Text(it, color = MaterialTheme.colorScheme.error, modifier = Modifier.padding(horizontal = 16.dp))
                    }

                    PalletDetalleLista(
                        detalle = estadoActual.detalle,
                        palletExiste = estadoActual.palletId != null,
                        bloqueado = bloqueado,
                        puedeCrear = puedeCrear,
                        puedeModificar = puedeModificar,
                        puedeEliminar = puedeEliminar,
                        onAgregarClick = {
                            lineaEnEdicion = null
                            mensajeErrorLinea = null
                            viewModel.cargarLotesEnProceso(lineaProduccionId)
                            mostrarDialogoLinea = true
                        },
                        onEditarClick = { linea ->
                            lineaEnEdicion = linea
                            mensajeErrorLinea = null
                            mostrarDialogoLinea = true
                        },
                        onEliminarClick = { linea -> viewModel.eliminarLinea(linea.id) }
                    )
                }

                if (mostrarDialogoLinea) {
                    val productoEncabezado = estadoActual.productosEncabezado.firstOrNull { it.id == productoTerminadoId }
                    DialogoLineaDetalle(
                        lineaExistente = lineaEnEdicion,
                        esMixto = esMixto,
                        productoEncabezado = productoEncabezado,
                        lotesEnProceso = lotesEnProceso,
                        productosBusqueda = productosBusqueda,
                        onBuscarProducto = { viewModel.buscarProductos(it) },
                        guardando = false,
                        mensajeError = mensajeErrorLinea,
                        onDismiss = { mostrarDialogoLinea = false },
                        onGuardar = { corridaId, productoId, cajas, kilogramos, porcentajeMateriaSeca ->
                            val existente = lineaEnEdicion
                            if (existente == null) {
                                val corrida = corridaId ?: return@DialogoLineaDetalle
                                viewModel.agregarLinea(corrida, productoId, cajas, kilogramos, porcentajeMateriaSeca) { error ->
                                    if (error == null) mostrarDialogoLinea = false else mensajeErrorLinea = error
                                }
                            } else {
                                viewModel.editarLinea(existente.id, productoId, cajas, kilogramos, porcentajeMateriaSeca) { error ->
                                    if (error == null) mostrarDialogoLinea = false else mensajeErrorLinea = error
                                }
                            }
                        }
                    )
                }

                if (mostrarConfirmarBloquear) {
                    AlertDialog(
                        onDismissRequest = { mostrarConfirmarBloquear = false },
                        title = { Text("Bloquear pallet") },
                        text = { Text("Esta acción es irreversible: el pallet ya no se podrá modificar ni agregar/quitar líneas. ¿Continuar?") },
                        confirmButton = {
                            TextButton(onClick = {
                                viewModel.bloquear()
                                mostrarConfirmarBloquear = false
                            }) { Text("Bloquear") }
                        },
                        dismissButton = {
                            TextButton(onClick = { mostrarConfirmarBloquear = false }) { Text("Cancelar") }
                        }
                    )
                }

                if (mostrarConfirmarEliminar) {
                    AlertDialog(
                        onDismissRequest = { mostrarConfirmarEliminar = false },
                        title = { Text("Eliminar pallet") },
                        text = { Text("¿Eliminar este pallet? Se liberarán los kilogramos consumidos de sus lotes. Esta acción no se puede deshacer.") },
                        confirmButton = {
                            TextButton(onClick = {
                                viewModel.eliminarPallet()
                                mostrarConfirmarEliminar = false
                            }) { Text("Eliminar") }
                        },
                        dismissButton = {
                            TextButton(onClick = { mostrarConfirmarEliminar = false }) { Text("Cancelar") }
                        }
                    )
                }
            }
        }
    }
}

private fun String.aBigDecimalCapturado(): java.math.BigDecimal? = try {
    if (isBlank()) null else java.math.BigDecimal(this)
} catch (ex: NumberFormatException) {
    null
}
