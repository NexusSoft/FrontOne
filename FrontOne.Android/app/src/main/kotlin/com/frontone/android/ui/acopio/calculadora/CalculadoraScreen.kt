package com.frontone.android.ui.acopio.calculadora

import androidx.activity.compose.BackHandler
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Check
import androidx.compose.material.icons.filled.Error
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
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
import androidx.compose.ui.draw.drawBehind
import androidx.compose.ui.focus.onFocusChanged
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.ListaPrecioFrutaTipo
import com.frontone.android.domain.model.VigenciaListaPrecioFruta
import com.frontone.android.ui.pallets.CampoSelector
import com.frontone.android.ui.pallets.PalAcentoAzul
import com.frontone.android.ui.pallets.PalBordeChip
import com.frontone.android.ui.pallets.PalBordeInput
import com.frontone.android.ui.pallets.PalFondoPantalla
import com.frontone.android.ui.pallets.PalFondoTarjeta
import com.frontone.android.ui.pallets.PalTextoSecundario
import com.frontone.android.ui.pallets.PalTextoTerciario
import com.frontone.android.ui.pallets.PalTextoTitulo

/**
 * Calculadora del módulo Acopio — traducción pixel-exacta de `Calculadora.dc.html`
 * (DesignSync), con la lógica real del "Simulador de Bandas" de escritorio
 * (`SimuladorBandasForm.cs`/`FilaSimuladorBanda.cs`) por debajo — nunca los datos falsos
 * en memoria del mockup. Ver `CalculadoraViewModel` para la fórmula/reglas exactas.
 */
@Composable
fun CalculadoraScreen(onVolverClick: () -> Unit, viewModel: CalculadoraViewModel = hiltViewModel()) {
    val estado by viewModel.estado.collectAsState()
    var expandidoLista by remember { mutableStateOf(false) }

    // El gesto de retroceso cierra primero el selector de precios si está abierto;
    // si no, se propaga al BackHandler de AcopioHostScreen (regresa al Dashboard).
    BackHandler(enabled = estado.selectorAbierto) { viewModel.cerrarSelectorPrecios() }

    Surface(modifier = Modifier.fillMaxSize(), color = PalFondoPantalla) {
        Column(modifier = Modifier.fillMaxSize()) {
            Encabezado(onVolverClick)

            Column(
                modifier = Modifier.padding(horizontal = 24.dp, vertical = 0.dp),
                verticalArrangement = Arrangement.spacedBy(10.dp)
            ) {
                Row(
                    modifier = Modifier.padding(top = 16.dp),
                    horizontalArrangement = Arrangement.spacedBy(10.dp),
                    verticalAlignment = Alignment.Bottom
                ) {
                    CampoSelector(
                        etiqueta = "Lista a cargar",
                        textoSeleccionado = estado.listaSeleccionada.etiqueta,
                        placeholder = "Seleccionar",
                        expandido = expandidoLista,
                        onExpandidoChange = { expandidoLista = it },
                        opciones = ListaPrecioFrutaTipo.entries.toList(),
                        etiquetaOpcion = { it.etiqueta },
                        onSeleccionar = { viewModel.cambiarLista(it) },
                        habilitado = true,
                        modifier = Modifier.weight(1f)
                    )
                    BotonPrecios(onClick = viewModel::abrirSelectorPrecios)
                }

                if (!expandidoLista) {
                    estado.origenPrecios?.let { origen ->
                        BannerInfo(
                            "Precios cargados de ${origen.vigencia.fechaFormateada()} — " +
                                "${origen.vigencia.productorNombre ?: "General"} (${origen.lista.etiqueta})"
                        )
                    } ?: BannerInfo("Aún no se han cargado precios — toca \"Precios\" para elegir una vigencia.")

                    CampoBusqueda(
                        valor = estado.busqueda,
                        onValorChange = viewModel::cambiarBusqueda,
                        placeholder = "Buscar calibre o categoría"
                    )
                }
            }

            if (estado.cargando) {
                Box(modifier = Modifier.fillMaxWidth().weight(1f), contentAlignment = Alignment.Center) {
                    CircularProgressIndicator(color = PalAcentoAzul)
                }
            } else if (estado.error != null) {
                Box(modifier = Modifier.fillMaxWidth().weight(1f).padding(24.dp), contentAlignment = Alignment.Center) {
                    Text(estado.error ?: "", fontSize = 13.sp, fontWeight = FontWeight.SemiBold, color = PalTextoTerciario)
                }
            } else {
                EncabezadoTabla()
                TablaFilas(
                    filas = estado.filasFiltradas,
                    preciosCargados = estado.origenPrecios != null,
                    onCambiarPorcentaje = viewModel::cambiarPorcentaje,
                    onFinalizarEdicionPorcentaje = viewModel::finalizarEdicionPorcentaje,
                    modifier = Modifier.weight(1f)
                )
                PieTotales(estado)
                BotonesAccion(onLimpiarClick = viewModel::solicitarLimpiar)
            }
        }
    }

    if (estado.selectorAbierto) {
        SelectorVigenciaModal(estado = estado, viewModel = viewModel)
    }

    if (estado.mostrarConfirmarLimpiar) {
        AlertDialog(
            onDismissRequest = viewModel::cancelarLimpiar,
            title = { Text("Limpiar calculadora") },
            text = { Text("¿Seguro que quieres reiniciar todos los precios y porcentajes capturados?") },
            confirmButton = {
                Text(
                    "Limpiar",
                    color = PalAcentoAzul,
                    fontWeight = FontWeight.Bold,
                    modifier = Modifier.padding(12.dp).clickable { viewModel.confirmarLimpiar() }
                )
            },
            dismissButton = {
                Text(
                    "Cancelar",
                    color = PalTextoTerciario,
                    fontWeight = FontWeight.Bold,
                    modifier = Modifier.padding(12.dp).clickable { viewModel.cancelarLimpiar() }
                )
            }
        )
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
            Text("Calculadora", fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)
            Text("Simulador de bandas · Acopio", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = PalTextoSecundario)
        }
    }
}

@Composable
private fun BotonPrecios(onClick: () -> Unit) {
    Surface(
        modifier = Modifier.height(48.dp),
        shape = RoundedCornerShape(12.dp),
        color = PalAcentoAzul,
        onClick = onClick
    ) {
        Row(
            modifier = Modifier.fillMaxHeight().padding(horizontal = 16.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(7.dp)
        ) {
            Icon(Icons.Filled.Check, contentDescription = null, tint = Color.White, modifier = Modifier.size(15.dp))
            Text("Precios", fontSize = 13.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
        }
    }
}

@Composable
private fun BannerInfo(texto: String) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(12.dp))
            .background(PalAcentoAzul.copy(alpha = 0.08f))
            .padding(horizontal = 13.dp, vertical = 10.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        Icon(Icons.Filled.Error, contentDescription = null, tint = PalAcentoAzul, modifier = Modifier.size(14.dp))
        Text(texto, fontSize = 12.sp, fontWeight = FontWeight.Bold, color = CalcInfoTexto, lineHeight = 16.sp)
    }
}

@Composable
private fun CampoBusqueda(valor: String, onValorChange: (String) -> Unit, placeholder: String) {
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
                singleLine = true,
                textStyle = TextStyle(fontSize = 13.5.sp, fontWeight = FontWeight.SemiBold, color = PalTextoTitulo),
                modifier = Modifier.fillMaxWidth()
            )
        }
    }
}

@Composable
private fun EncabezadoTabla() {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(start = 24.dp, end = 24.dp, top = 16.dp)
            .clip(RoundedCornerShape(topStart = 10.dp, topEnd = 10.dp))
            .background(CalcEncabezadoTabla)
            .padding(vertical = 9.dp)
    ) {
        Text(
            "CALIBRE", fontSize = 10.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White, letterSpacing = 0.3.sp,
            modifier = Modifier.weight(1f).padding(start = 12.dp)
        )
        Text(
            "$ / KG", fontSize = 10.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White.copy(alpha = 0.7f), letterSpacing = 0.3.sp,
            textAlign = androidx.compose.ui.text.style.TextAlign.End, modifier = Modifier.weight(1f)
        )
        Text(
            "% CURVA", fontSize = 10.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White, letterSpacing = 0.3.sp,
            textAlign = androidx.compose.ui.text.style.TextAlign.End, modifier = Modifier.weight(1f)
        )
        Text(
            "BANDA", fontSize = 10.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White, letterSpacing = 0.3.sp,
            textAlign = androidx.compose.ui.text.style.TextAlign.End, modifier = Modifier.weight(1f).padding(end = 12.dp)
        )
    }
}

@Composable
private fun TablaFilas(
    filas: List<FilaBanda>,
    preciosCargados: Boolean,
    onCambiarPorcentaje: (FilaBanda, String) -> Unit,
    onFinalizarEdicionPorcentaje: (FilaBanda) -> Unit,
    modifier: Modifier = Modifier
) {
    Surface(
        modifier = modifier
            .fillMaxWidth()
            .padding(start = 24.dp, end = 24.dp)
            .clip(RoundedCornerShape(bottomStart = 14.dp, bottomEnd = 14.dp)),
        color = Color.White,
        shadowElevation = 3.dp
    ) {
        LazyColumn {
            items(filas, key = { "${it.categoriaId}-${it.calibreApeamId}" }) { fila ->
                FilaCalculadora(
                    fila = fila,
                    habilitado = preciosCargados,
                    onCambiarPorcentaje = { texto -> onCambiarPorcentaje(fila, texto) },
                    onFinalizarEdicion = { onFinalizarEdicionPorcentaje(fila) }
                )
            }
        }
    }
}

@Composable
private fun FilaCalculadora(
    fila: FilaBanda,
    habilitado: Boolean,
    onCambiarPorcentaje: (String) -> Unit,
    onFinalizarEdicion: () -> Unit
) {
    val (colorCategoria, fondoCategoria) = categoriaColores(fila.categoriaNombre)
    val activa = fila.porcentaje.signum() != 0
    val colorBanda = if (activa) CalcBandaActiva else CalcBandaInactiva

    Row(
        modifier = Modifier
            .fillMaxWidth()
            .background(if (activa) fondoCategoria else Color.White)
            .drawBehind {
                drawLine(
                    color = Color(0xFFF0F0F5),
                    start = androidx.compose.ui.geometry.Offset(0f, size.height),
                    end = androidx.compose.ui.geometry.Offset(size.width, size.height),
                    strokeWidth = 1.5f
                )
            }
            .padding(vertical = 9.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Column(modifier = Modifier.weight(1f).padding(start = 12.dp)) {
            Text(
                fila.calibreApeamNombre, fontSize = 13.5.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo,
                maxLines = 1, overflow = TextOverflow.Ellipsis
            )
            Text(
                fila.categoriaNombre.uppercase(), fontSize = 10.5.sp, fontWeight = FontWeight.Bold, color = colorCategoria,
                letterSpacing = 0.3.sp
            )
        }
        Text(
            fila.precio.aMoneda(), fontSize = 13.sp, fontWeight = FontWeight.Bold, color = PalTextoTerciario,
            textAlign = androidx.compose.ui.text.style.TextAlign.End, modifier = Modifier.weight(1f)
        )
        Row(modifier = Modifier.weight(1f), horizontalArrangement = Arrangement.End, verticalAlignment = Alignment.CenterVertically) {
            Box(
                modifier = Modifier
                    .width(54.dp)
                    .height(32.dp)
                    .clip(RoundedCornerShape(8.dp))
                    .background(if (!habilitado) PalFondoTarjeta else if (activa) PalAcentoAzul.copy(alpha = 0.06f) else Color.White)
                    .border(1.5.dp, if (habilitado && activa) PalAcentoAzul else PalBordeInput, RoundedCornerShape(8.dp))
                    .padding(horizontal = 6.dp),
                contentAlignment = Alignment.CenterEnd
            ) {
                BasicTextField(
                    value = fila.porcentajeTexto,
                    onValueChange = onCambiarPorcentaje,
                    readOnly = !habilitado,
                    singleLine = true,
                    textStyle = TextStyle(
                        fontSize = 13.sp, fontWeight = FontWeight.ExtraBold,
                        color = if (habilitado) PalTextoTitulo else PalTextoSecundario,
                        textAlign = androidx.compose.ui.text.style.TextAlign.End
                    ),
                    keyboardOptions = androidx.compose.foundation.text.KeyboardOptions(keyboardType = androidx.compose.ui.text.input.KeyboardType.Number),
                    modifier = Modifier
                        .fillMaxWidth()
                        .onFocusChanged { estadoFoco -> if (!estadoFoco.isFocused) onFinalizarEdicion() }
                )
            }
            Text("%", fontSize = 12.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoSecundario, modifier = Modifier.padding(start = 2.dp))
        }
        Text(
            fila.banda.aMoneda(), fontSize = 13.5.sp, fontWeight = FontWeight.ExtraBold, color = colorBanda,
            textAlign = androidx.compose.ui.text.style.TextAlign.End, modifier = Modifier.weight(1f).padding(end = 12.dp)
        )
    }
}

private fun categoriaColores(nombre: String): Pair<Color, Color> = when (nombre) {
    "Cat 1" -> CalcCat1Texto to CalcCat1Fondo
    "Cat 2" -> CalcCat2Texto to CalcCat2Fondo
    "Nal" -> CalcNacionalTexto to CalcNacionalFondo
    else -> PalTextoTerciario to Color.White
}

@Composable
private fun PieTotales(estado: EstadoCalculadora) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(start = 24.dp, end = 24.dp, top = 14.dp)
            .clip(RoundedCornerShape(16.dp))
            .background(CalcEncabezadoTabla)
            .padding(horizontal = 16.dp, vertical = 14.dp),
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        Column {
            Text("CURVA TOTAL", fontSize = 10.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White.copy(alpha = 0.5f), letterSpacing = 0.4.sp)
            Text(
                estado.curvaTotal.aPorcentaje(),
                fontSize = 17.sp, fontWeight = FontWeight.ExtraBold,
                color = if (estado.avisoSumaVisible) CalcCurvaAviso else CalcCurvaOk
            )
        }
        Column(horizontalAlignment = Alignment.End) {
            Text("BANDA FINAL", fontSize = 10.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White.copy(alpha = 0.5f), letterSpacing = 0.4.sp)
            Text(estado.bandaTotal.aMoneda(), fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
        }
    }
}

@Composable
private fun BotonesAccion(onLimpiarClick: () -> Unit) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(start = 24.dp, end = 24.dp, top = 14.dp, bottom = 10.dp),
        horizontalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Surface(
            modifier = Modifier.weight(1f).height(46.dp),
            shape = RoundedCornerShape(14.dp),
            color = Color.White,
            border = androidx.compose.foundation.BorderStroke(1.5.dp, PalBordeInput),
            onClick = onLimpiarClick
        ) {
            Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                Text("Limpiar", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTerciario)
            }
        }
        Surface(
            modifier = Modifier.weight(1f).height(46.dp),
            shape = RoundedCornerShape(14.dp),
            color = PalAcentoAzul,
            onClick = { /* Exportar: pendiente — en escritorio genera imagen/PDF/Excel local, ver contexto/acopio.md */ }
        ) {
            Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                Text("Exportar", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
            }
        }
    }
}

@Composable
private fun SelectorVigenciaModal(estado: EstadoCalculadora, viewModel: CalculadoraViewModel) {
    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(Color.Black.copy(alpha = 0.4f))
            .clickable(indication = null, interactionSource = remember { androidx.compose.foundation.interaction.MutableInteractionSource() }) {
                viewModel.cerrarSelectorPrecios()
            },
        contentAlignment = Alignment.BottomCenter
    ) {
        Surface(
            modifier = Modifier
                .fillMaxWidth()
                .fillMaxHeight(0.78f)
                .clickable(indication = null, interactionSource = remember { androidx.compose.foundation.interaction.MutableInteractionSource() }) {},
            shape = RoundedCornerShape(topStart = 24.dp, topEnd = 24.dp),
            color = PalFondoPantalla
        ) {
            Column(modifier = Modifier.fillMaxSize()) {
                Column(modifier = Modifier.padding(start = 22.dp, end = 22.dp, top = 22.dp)) {
                    Text("Cargar precios", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)
                    Text(
                        "Lista de precio fruta · ${estado.listaSeleccionada.etiqueta}",
                        fontSize = 12.5.sp, fontWeight = FontWeight.Bold, color = PalTextoSecundario,
                        modifier = Modifier.padding(top = 4.dp)
                    )
                }

                Box(modifier = Modifier.padding(start = 22.dp, end = 22.dp, top = 14.dp)) {
                    CampoBusqueda(
                        valor = estado.busquedaVigencias,
                        onValorChange = viewModel::cambiarBusquedaVigencias,
                        placeholder = "Buscar fecha o productor"
                    )
                }

                if (estado.cargandoVigencias) {
                    Box(modifier = Modifier.fillMaxWidth().weight(1f), contentAlignment = Alignment.Center) {
                        CircularProgressIndicator(color = PalAcentoAzul)
                    }
                } else {
                    LazyColumn(
                        modifier = Modifier.weight(1f).padding(start = 22.dp, end = 22.dp, top = 14.dp, bottom = 20.dp),
                        verticalArrangement = Arrangement.spacedBy(8.dp)
                    ) {
                        items(estado.vigenciasFiltradas, key = { "${it.fecha}-${it.productorId}" }) { vigencia ->
                            FilaVigencia(
                                vigencia = vigencia,
                                seleccionada = estado.vigenciaSeleccionadaEnSelector == vigencia,
                                onClick = { viewModel.elegirVigenciaEnSelector(vigencia) }
                            )
                        }
                    }
                }

                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .background(PalFondoPantalla)
                        .border(border = androidx.compose.foundation.BorderStroke(1.dp, PalBordeChip))
                        .padding(start = 22.dp, end = 22.dp, top = 14.dp, bottom = 26.dp),
                    horizontalArrangement = Arrangement.spacedBy(12.dp)
                ) {
                    Surface(
                        modifier = Modifier.weight(1f).height(48.dp),
                        shape = RoundedCornerShape(14.dp),
                        color = Color.White,
                        border = androidx.compose.foundation.BorderStroke(1.5.dp, PalBordeInput),
                        onClick = viewModel::cerrarSelectorPrecios
                    ) {
                        Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                            Text("Cancelar", fontSize = 14.5.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTerciario)
                        }
                    }
                    Surface(
                        modifier = Modifier.weight(1f).height(48.dp),
                        shape = RoundedCornerShape(14.dp),
                        color = if (estado.vigenciaSeleccionadaEnSelector != null) PalAcentoAzul else PalBordeInput,
                        onClick = { if (estado.vigenciaSeleccionadaEnSelector != null) viewModel.confirmarCargaPrecios() }
                    ) {
                        Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                            Text("Cargar", fontSize = 14.5.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                        }
                    }
                }
            }
        }
    }
}

@Composable
private fun FilaVigencia(vigencia: VigenciaListaPrecioFruta, seleccionada: Boolean, onClick: () -> Unit) {
    Surface(
        modifier = Modifier.fillMaxWidth().clickable(onClick = onClick),
        shape = RoundedCornerShape(14.dp),
        color = Color.White,
        border = androidx.compose.foundation.BorderStroke(1.5.dp, if (seleccionada) PalAcentoAzul else PalBordeChip),
        shadowElevation = if (seleccionada) 4.dp else 0.dp
    ) {
        Row(
            modifier = Modifier.fillMaxWidth().padding(horizontal = 15.dp, vertical = 13.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Column {
                Text(vigencia.fechaFormateada(), fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = PalTextoTitulo)
                Text(vigencia.productorNombre ?: "General", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = PalTextoSecundario)
            }
            if (seleccionada) {
                Icon(Icons.Filled.Check, contentDescription = null, tint = PalAcentoAzul, modifier = Modifier.size(18.dp))
            }
        }
    }
}
