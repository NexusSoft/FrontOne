package com.frontone.android.ui.acopio.huertas

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.EditLocationAlt
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.DisposableEffect
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberUpdatedState
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.ui.viewinterop.AndroidView
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.HuertaMapa
import com.frontone.android.ui.pallets.PalAcentoAzul
import com.frontone.android.ui.pallets.PalBordeInput
import com.frontone.android.ui.pallets.PalErrorCorridaFinalizada
import com.frontone.android.ui.pallets.PalFondoPantalla
import com.frontone.android.ui.pallets.PalTextoSecundario
import com.frontone.android.ui.pallets.PalTextoTerciario
import com.frontone.android.ui.pallets.PalTextoTitulo
import kotlinx.coroutines.delay
import org.osmdroid.config.Configuration
import org.osmdroid.events.MapEventsReceiver
import org.osmdroid.tileprovider.tilesource.TileSourceFactory
import org.osmdroid.util.GeoPoint
import org.osmdroid.views.MapView
import org.osmdroid.views.overlay.Marker
import org.osmdroid.views.overlay.MapEventsOverlay

/** Centro entre Michoacán y Jalisco — solo referencia visual inicial, sin significado
 * de negocio (mismo criterio que el centro de México usado en el mapa de escritorio). */
private val CENTRO_INICIAL = GeoPoint(19.9, -102.5)
private const val ZOOM_INICIAL = 7.0

@Composable
fun HuertasMapaScreen(onVolverClick: () -> Unit, viewModel: HuertasMapaViewModel = hiltViewModel()) {
    val estado by viewModel.estado.collectAsState()

    // Auto-oculta el banner de confirmación/error después de un momento — mismo
    // criterio ligero que el resto de la app (sin Scaffold/Snackbar todavía en el
    // proyecto).
    LaunchedEffect(estado.mensaje) {
        if (estado.mensaje != null) {
            delay(2500)
            viewModel.limpiarMensaje()
        }
    }

    Surface(modifier = Modifier.fillMaxSize(), color = PalFondoPantalla) {
        Column(modifier = Modifier.fillMaxSize()) {
            Encabezado(onVolverClick)

            Box(modifier = Modifier.padding(horizontal = 24.dp, vertical = 16.dp)) {
                CampoBusqueda(
                    valor = estado.busqueda,
                    onValorChange = viewModel::cambiarBusqueda,
                    placeholder = "Buscar por HUE, nombre o productor",
                    habilitado = !estado.modoEdicionActivo
                )
            }

            Box(modifier = Modifier.weight(1f).fillMaxWidth()) {
                MapaHuertas(
                    huertas = estado.huertas,
                    huertaSeleccionada = estado.huertaSeleccionada,
                    modoEdicionActivo = estado.modoEdicionActivo,
                    onMarcarClick = viewModel::seleccionarHuerta,
                    onMoverPin = viewModel::moverPinTemporal
                )

                if (estado.cargando) {
                    Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                        CircularProgressIndicator(color = PalAcentoAzul)
                    }
                }

                (estado.mensaje ?: estado.error)?.let { texto ->
                    BannerFlotante(texto = texto, esError = estado.error != null)
                }

                if (estado.modoEdicionActivo) {
                    BarraEdicionUbicacion(
                        onGuardarClick = viewModel::guardarUbicacion,
                        onCancelarClick = viewModel::cancelarEdicionUbicacion,
                        guardando = estado.guardandoUbicacion
                    )
                } else {
                    estado.huertaSeleccionada?.let { huerta ->
                        TarjetaDetalleHuerta(
                            huerta = huerta,
                            onCorregirUbicacionClick = viewModel::iniciarEdicionUbicacion,
                            onCerrarClick = viewModel::cerrarDetalle
                        )
                    }
                }
            }
        }
    }
}

@Composable
private fun Encabezado(onVolverClick: () -> Unit) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(start = 24.dp, end = 24.dp, top = 20.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        Surface(
            modifier = Modifier.size(38.dp),
            shape = RoundedCornerShape(12.dp),
            color = Color.White,
            shadowElevation = 4.dp,
            onClick = onVolverClick
        ) {
            Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                Icon(Icons.Filled.ArrowBack, contentDescription = "Volver", tint = PalTextoTitulo, modifier = Modifier.size(18.dp))
            }
        }
        Column {
            Text("Huertas", fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)
            Text("Mapa · Acopio", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = PalTextoSecundario)
        }
    }
}

@Composable
private fun CampoBusqueda(valor: String, onValorChange: (String) -> Unit, placeholder: String, habilitado: Boolean) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .height(44.dp)
            .clip(RoundedCornerShape(12.dp))
            .background(Color.White)
            .border(1.5.dp, PalBordeInput, RoundedCornerShape(12.dp))
            .padding(horizontal = 14.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Icon(Icons.Filled.Search, contentDescription = null, tint = PalTextoSecundario, modifier = Modifier.size(15.dp))
        Box(modifier = Modifier.fillMaxWidth()) {
            if (valor.isEmpty()) {
                Text(placeholder, fontSize = 13.5.sp, fontWeight = FontWeight.SemiBold, color = PalTextoSecundario)
            }
            BasicTextField(
                value = valor,
                onValueChange = onValorChange,
                enabled = habilitado,
                singleLine = true,
                textStyle = TextStyle(fontSize = 13.5.sp, fontWeight = FontWeight.SemiBold, color = PalTextoTitulo),
                modifier = Modifier.fillMaxWidth()
            )
        }
    }
}

@Composable
private fun MapaHuertas(
    huertas: List<HuertaMapa>,
    huertaSeleccionada: HuertaMapa?,
    modoEdicionActivo: Boolean,
    onMarcarClick: (HuertaMapa) -> Unit,
    onMoverPin: (java.math.BigDecimal, java.math.BigDecimal) -> Unit
) {
    val contexto = LocalContext.current
    val mapView = remember {
        Configuration.getInstance().load(contexto, contexto.getSharedPreferences("osmdroid", 0))
        MapView(contexto).apply {
            setTileSource(TileSourceFactory.MAPNIK)
            setMultiTouchControls(true)
            controller.setZoom(ZOOM_INICIAL)
            controller.setCenter(CENTRO_INICIAL)
        }
    }

    // Callbacks siempre "frescos" dentro del listener del mapa (que se crea una sola
    // vez) — evita capturar una versión vieja de la lambda entre recomposiciones.
    val onMoverPinActual by rememberUpdatedState(onMoverPin)
    val onMarcarClickActual by rememberUpdatedState(onMarcarClick)

    DisposableEffect(Unit) {
        onDispose { mapView.onDetach() }
    }

    LaunchedEffect(huertas, huertaSeleccionada, modoEdicionActivo) {
        mapView.overlays.clear()

        if (modoEdicionActivo) {
            val receptor = object : MapEventsReceiver {
                override fun singleTapConfirmedHelper(punto: GeoPoint): Boolean {
                    onMoverPinActual(
                        java.math.BigDecimal(punto.latitude).setScale(6, java.math.RoundingMode.HALF_UP),
                        java.math.BigDecimal(punto.longitude).setScale(6, java.math.RoundingMode.HALF_UP)
                    )
                    return true
                }

                override fun longPressHelper(punto: GeoPoint): Boolean = false
            }
            mapView.overlays.add(MapEventsOverlay(receptor))
        }

        huertas.forEach { huerta ->
            val enEdicion = modoEdicionActivo && huertaSeleccionada?.id == huerta.id
            val posicion = if (enEdicion) huertaSeleccionada else huerta
            val marcador = Marker(mapView).apply {
                position = GeoPoint(posicion!!.latitud.toDouble(), posicion.longitud.toDouble())
                title = posicion.nombre
                setAnchor(Marker.ANCHOR_CENTER, Marker.ANCHOR_BOTTOM)
                if (!modoEdicionActivo) {
                    setOnMarkerClickListener { _, _ -> onMarcarClickActual(huerta); true }
                }
            }
            mapView.overlays.add(marcador)
        }

        mapView.invalidate()
    }

    AndroidView(factory = { mapView }, modifier = Modifier.fillMaxSize())
}

@Composable
private fun BannerFlotante(texto: String, esError: Boolean) {
    Box(modifier = Modifier.fillMaxWidth().padding(16.dp), contentAlignment = Alignment.TopCenter) {
        Surface(
            shape = RoundedCornerShape(12.dp),
            color = if (esError) PalErrorCorridaFinalizada else PalTextoTitulo,
            shadowElevation = 6.dp
        ) {
            Text(
                texto,
                fontSize = 13.sp,
                fontWeight = FontWeight.Bold,
                color = Color.White,
                modifier = Modifier.padding(horizontal = 16.dp, vertical = 10.dp)
            )
        }
    }
}

@Composable
private fun TarjetaDetalleHuerta(huerta: HuertaMapa, onCorregirUbicacionClick: () -> Unit, onCerrarClick: () -> Unit) {
    Box(modifier = Modifier.fillMaxSize().padding(16.dp), contentAlignment = Alignment.BottomCenter) {
        Surface(
            modifier = Modifier.fillMaxWidth(),
            shape = RoundedCornerShape(18.dp),
            color = Color.White,
            shadowElevation = 8.dp
        ) {
            Column(modifier = Modifier.padding(18.dp), verticalArrangement = Arrangement.spacedBy(10.dp)) {
                Text(huerta.nombre, fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)
                huerta.registroSagarpa?.let {
                    Text("Registro SAGARPA: $it", fontSize = 12.5.sp, fontWeight = FontWeight.SemiBold, color = PalTextoSecundario)
                }
                Text("Productor: ${huerta.productorNombre}", fontSize = 12.5.sp, fontWeight = FontWeight.SemiBold, color = PalTextoSecundario)

                Row(horizontalArrangement = Arrangement.spacedBy(10.dp), modifier = Modifier.fillMaxWidth().padding(top = 4.dp)) {
                    Surface(
                        modifier = Modifier.weight(1f).height(44.dp),
                        shape = RoundedCornerShape(12.dp),
                        color = Color.White,
                        border = androidx.compose.foundation.BorderStroke(1.5.dp, PalBordeInput),
                        onClick = onCerrarClick
                    ) {
                        Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                            Text("Cerrar", fontSize = 13.5.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTerciario)
                        }
                    }
                    Surface(
                        modifier = Modifier.weight(1f).height(44.dp),
                        shape = RoundedCornerShape(12.dp),
                        color = PalAcentoAzul,
                        onClick = onCorregirUbicacionClick
                    ) {
                        Row(
                            modifier = Modifier.fillMaxSize(),
                            horizontalArrangement = Arrangement.Center,
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            Icon(Icons.Filled.EditLocationAlt, contentDescription = null, tint = Color.White, modifier = Modifier.size(16.dp))
                            Text(
                                "Corregir ubicación",
                                fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = Color.White,
                                modifier = Modifier.padding(start = 6.dp)
                            )
                        }
                    }
                }
            }
        }
    }
}

@Composable
private fun BarraEdicionUbicacion(onGuardarClick: () -> Unit, onCancelarClick: () -> Unit, guardando: Boolean) {
    Box(modifier = Modifier.fillMaxSize().padding(16.dp), contentAlignment = Alignment.BottomCenter) {
        Surface(
            modifier = Modifier.fillMaxWidth(),
            shape = RoundedCornerShape(18.dp),
            color = PalTextoTitulo,
            shadowElevation = 8.dp
        ) {
            Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                Text(
                    "Toca el mapa donde va el pin correcto de la huerta.",
                    fontSize = 12.5.sp, fontWeight = FontWeight.SemiBold, color = Color.White.copy(alpha = 0.85f)
                )
                Row(horizontalArrangement = Arrangement.spacedBy(10.dp), modifier = Modifier.fillMaxWidth()) {
                    Surface(
                        modifier = Modifier.weight(1f).height(44.dp),
                        shape = RoundedCornerShape(12.dp),
                        color = Color.White.copy(alpha = 0.12f),
                        onClick = onCancelarClick
                    ) {
                        Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                            Text("Cancelar", fontSize = 13.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                        }
                    }
                    Surface(
                        modifier = Modifier.weight(1f).height(44.dp),
                        shape = RoundedCornerShape(12.dp),
                        color = PalAcentoAzul,
                        onClick = { if (!guardando) onGuardarClick() }
                    ) {
                        Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                            if (guardando) {
                                CircularProgressIndicator(color = Color.White, modifier = Modifier.size(18.dp), strokeWidth = 2.dp)
                            } else {
                                Text("Guardar ubicación", fontSize = 13.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                            }
                        }
                    }
                }
            }
        }
    }
}
