package com.frontone.android.ui.pallets

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.PalletDetalle
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.tienePermiso

/**
 * Orquestador de la captura de un Pallet — equivalente a PalletEditarForm.cs, reconstruido sobre
 * el diseño real (`Pallets.dc.html`, mismo proyecto DesignSync que la Lista): header con botón
 * redondo de regreso, chip resumen con separadores "·", tarjetas Encabezado/Líneas de detalle.
 * Eliminar (no cubierto por el mockup, que no modela esa acción) se mantiene como botón redondo
 * junto al de regreso — mismo patrón de confirmación fuerte que el resto de la app (ver CLAUDE.md
 * "todo evento de eliminar registro pregunta antes de eliminar"). Bloquear existe en escritorio
 * (`PalletsForm`/`PalletEditarForm`) pero se quitó de la app móvil a pedido explícito del usuario
 * — ver `contexto/arquitectura.md`.
 */
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
    var textoProducto by remember { mutableStateOf("") }
    var expandidoProducto by remember { mutableStateOf(false) }
    var pesoReal by remember { mutableStateOf("") }
    var mostrarDialogoLinea by remember { mutableStateOf(false) }
    var lineaEnEdicion by remember { mutableStateOf<PalletDetalle?>(null) }
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
                textoProducto = if (pallet.productoTerminadoId != null) "${pallet.productoCodigoSap} - ${pallet.productoDescripcion}" else ""
                pesoReal = pallet.pesoReal?.toPlainString() ?: ""
            }
            precargado = true
        }
    }

    Surface(modifier = Modifier.fillMaxSize(), color = PalFondoPantalla) {
        Box(modifier = Modifier.fillMaxSize()) {
            when (val estadoActual = estado) {
                is EstadoPalletCaptura.Cargando -> Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    CircularProgressIndicator(color = PalAcentoAzul)
                }

                is EstadoPalletCaptura.ErrorCarga -> Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    Text(estadoActual.mensaje, color = MaterialTheme.colorScheme.error, modifier = Modifier.padding(24.dp))
                }

                is EstadoPalletCaptura.Formulario -> {
                    val bloqueado = estadoActual.pallet?.bloqueado == true
                    Column(modifier = Modifier.fillMaxSize().verticalScroll(rememberScrollState())) {
                        // ---------- Encabezado de pantalla ----------
                        Row(
                            modifier = Modifier.fillMaxWidth().padding(start = 24.dp, end = 24.dp, top = 44.dp),
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.spacedBy(12.dp)
                        ) {
                            BotonRedondoPequeno(onClick = onVolverClick) {
                                Icon(Icons.Filled.ArrowBack, contentDescription = "Volver", tint = PalTextoTitulo, modifier = Modifier.size(18.dp))
                            }
                            Text(
                                if (palletIdInicial == null) "Nuevo pallet" else "Pallet",
                                fontSize = 20.sp,
                                fontWeight = FontWeight.ExtraBold,
                                color = PalTextoTitulo,
                                modifier = Modifier.weight(1f)
                            )
                            if (estadoActual.palletId != null && !bloqueado) {
                                if (puedeEliminar) {
                                    BotonRedondoPequeno(onClick = { mostrarConfirmarEliminar = true }) {
                                        Icon(Icons.Filled.Delete, contentDescription = "Eliminar pallet", tint = PalErrorCorridaFinalizada, modifier = Modifier.size(16.dp))
                                    }
                                }
                            }
                        }

                        // ---------- Chip resumen ----------
                        estadoActual.pallet?.let { pallet ->
                            Row(
                                modifier = Modifier
                                    .padding(start = 24.dp, top = 16.dp)
                                    .clip(RoundedCornerShape(20.dp))
                                    .background(Color.White)
                                    .border(1.5.dp, PalBordeChip, RoundedCornerShape(20.dp))
                                    .padding(horizontal = 14.dp, vertical = 9.dp),
                                verticalAlignment = Alignment.CenterVertically,
                                horizontalArrangement = Arrangement.spacedBy(6.dp)
                            ) {
                                Text(pallet.folio, fontSize = 12.5.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoProducto)
                                Text("·", color = PalBordeChip)
                                Text(pallet.estatus.etiqueta(), fontSize = 12.5.sp, fontWeight = FontWeight.ExtraBold, color = pallet.estatus.color())
                                Text("·", color = PalBordeChip)
                                Text("${pallet.totalCajas} cajas", fontSize = 12.5.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoProducto)
                                Text("·", color = PalBordeChip)
                                Text("${pallet.totalKilogramos} kg", fontSize = 12.5.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoProducto)
                            }
                        }

                        // ---------- Encabezado (tarjeta) ----------
                        PalletEncabezadoSeccion(
                            lineasProduccion = estadoActual.lineasProduccion,
                            lineaProduccionId = lineaProduccionId,
                            onLineaProduccionChange = { lineaProduccionId = it },
                            esMixto = esMixto,
                            onEsMixtoChange = { esMixto = it },
                            textoProducto = textoProducto,
                            onTextoProductoChange = {
                                textoProducto = it
                                viewModel.buscarProductos(it)
                            },
                            expandidoProducto = expandidoProducto,
                            onExpandidoProductoChange = { expandidoProducto = it },
                            productosBusqueda = productosBusqueda,
                            onProductoSeleccionado = { producto ->
                                productoTerminadoId = producto.id
                                textoProducto = "${producto.codigoSap} - ${producto.descripcionSap}"
                                expandidoProducto = false
                            },
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
                            },
                            modifier = Modifier.padding(start = 24.dp, end = 24.dp, top = 16.dp)
                        )

                        estadoActual.mensajeError?.let {
                            Text(it, color = MaterialTheme.colorScheme.error, modifier = Modifier.padding(horizontal = 24.dp, vertical = 8.dp))
                        }

                        // ---------- Líneas de detalle (tarjeta) ----------
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
                            onEliminarClick = { linea -> viewModel.eliminarLinea(linea.id) },
                            modifier = Modifier.padding(start = 24.dp, end = 24.dp, top = 16.dp, bottom = 32.dp)
                        )
                    }

                    if (mostrarDialogoLinea) {
                        val productoEncabezado = if (!esMixto && productoTerminadoId != null) {
                            estadoActual.productosEncabezado.firstOrNull { it.id == productoTerminadoId }
                                ?: productosBusqueda.firstOrNull { it.id == productoTerminadoId }
                        } else {
                            null
                        }
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
}

@Composable
private fun BotonRedondoPequeno(onClick: () -> Unit, contenido: @Composable () -> Unit) {
    Box(
        modifier = Modifier
            .size(38.dp)
            .clip(RoundedCornerShape(12.dp))
            .background(Color.White)
            .clickable(onClick = onClick),
        contentAlignment = Alignment.Center
    ) {
        contenido()
    }
}

private fun String.aBigDecimalCapturado(): java.math.BigDecimal? = try {
    if (isBlank()) null else java.math.BigDecimal(this)
} catch (ex: NumberFormatException) {
    null
}
