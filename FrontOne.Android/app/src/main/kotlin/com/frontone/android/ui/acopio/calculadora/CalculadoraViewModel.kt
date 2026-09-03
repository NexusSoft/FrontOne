package com.frontone.android.ui.acopio.calculadora

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.data.sqlserver.SqlRepositoryException
import com.frontone.android.domain.model.CombinacionMateriaPrima
import com.frontone.android.domain.model.ListaPrecioFrutaTipo
import com.frontone.android.domain.model.VigenciaListaPrecioFruta
import com.frontone.android.domain.usecase.ObtenerCombinacionesMateriaPrimaUseCase
import com.frontone.android.domain.usecase.ObtenerPreciosPorFechaUseCase
import com.frontone.android.domain.usecase.ObtenerVigenciasListaPrecioFrutaUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import java.math.BigDecimal
import java.math.RoundingMode
import java.time.format.DateTimeFormatter
import java.util.Locale
import javax.inject.Inject

/** Una fila del simulador — equivalente a `FilaSimuladorBanda` (C#). */
data class FilaBanda(
    val categoriaId: Int,
    val categoriaNombre: String,
    val calibreApeamId: Int,
    val calibreApeamNombre: String,
    val precio: BigDecimal = BigDecimal.ZERO,
    val porcentajeTexto: String = "0"
) {
    // Porcentaje admite negativos (ej. Merma) y texto en captura libre — se resuelve a
    // BigDecimal solo al calcular, mismo criterio que el resto de la app con campos
    // numéricos de texto libre (ver DialogoLineaDetalle en el módulo Pallets).
    val porcentaje: BigDecimal get() = porcentajeTexto.toBigDecimalOrNull() ?: BigDecimal.ZERO

    // Banda = Precio × Porcentaje ÷ 100, sin redondear — igual que FilaSimuladorBanda.cs.
    // Redondear aquí distorsionaría la suma total si se compone contra otras filas.
    val banda: BigDecimal get() = precio.multiply(porcentaje).divide(BigDecimal(100))
}

/** Info de dónde salieron los precios actualmente cargados, para el banner informativo. */
data class OrigenPrecios(val vigencia: VigenciaListaPrecioFruta, val lista: ListaPrecioFrutaTipo)

data class EstadoCalculadora(
    val cargando: Boolean = true,
    val error: String? = null,
    val filas: List<FilaBanda> = emptyList(),
    val listaSeleccionada: ListaPrecioFrutaTipo = ListaPrecioFrutaTipo.CONVENCIONAL,
    val busqueda: String = "",
    val origenPrecios: OrigenPrecios? = null,
    val selectorAbierto: Boolean = false,
    val cargandoVigencias: Boolean = false,
    val vigencias: List<VigenciaListaPrecioFruta> = emptyList(),
    val vigenciaSeleccionadaEnSelector: VigenciaListaPrecioFruta? = null,
    val busquedaVigencias: String = "",
    val mostrarConfirmarLimpiar: Boolean = false
) {
    val filasFiltradas: List<FilaBanda>
        get() = if (busqueda.isBlank()) filas else filas.filter {
            it.calibreApeamNombre.contains(busqueda, ignoreCase = true) ||
                it.categoriaNombre.contains(busqueda, ignoreCase = true)
        }

    val curvaTotal: BigDecimal get() = filas.fold(BigDecimal.ZERO) { acc, f -> acc + f.porcentaje }
    val bandaTotal: BigDecimal get() = filas.fold(BigDecimal.ZERO) { acc, f -> acc + f.banda }

    // Mismo umbral y criterio que ActualizarAvisoSuma en SimuladorBandasForm.cs: solo
    // aviso visual, nunca bloquea captura ni exportación.
    val avisoSumaVisible: Boolean
        get() = (curvaTotal - BigDecimal(100)).abs() > BigDecimal("0.005")

    val vigenciasFiltradas: List<VigenciaListaPrecioFruta>
        get() = if (busquedaVigencias.isBlank()) vigencias else vigencias.filter {
            val formateada = it.fecha.format(FORMATEADOR_FECHA_VIGENCIA)
            formateada.contains(busquedaVigencias, ignoreCase = true) ||
                (it.productorNombre ?: "General").contains(busquedaVigencias, ignoreCase = true)
        }
}

private val FORMATEADOR_FECHA_VIGENCIA: DateTimeFormatter =
    DateTimeFormatter.ofPattern("dd/MM/yyyy", Locale("es", "MX"))

fun BigDecimal.aMoneda(): String = "$" + this.setScale(2, RoundingMode.HALF_UP).toPlainString()
fun BigDecimal.aPorcentaje(): String = this.setScale(2, RoundingMode.HALF_UP).toPlainString() + "%"
fun VigenciaListaPrecioFruta.fechaFormateada(): String = fecha.format(FORMATEADOR_FECHA_VIGENCIA)

/**
 * Calculadora del módulo Acopio — puerto móvil del "Simulador de Bandas" de escritorio
 * (`SimuladorBandasForm.cs`). Misma lógica de negocio real (no la del mockup, que traía
 * datos falsos en memoria): universo de combinaciones Categoría×Calibre APEAM activas,
 * precarga opcional de precios desde una vigencia ya guardada de
 * `Acopio.ListaPrecioFruta`, captura de %Curva por fila y Banda = Precio×Curva/100.
 * Puramente en memoria — no persiste nada en el servidor (igual que en escritorio, el
 * único "guardado" real allá es exportar a archivo, ver contexto/acopio.md).
 */
@HiltViewModel
class CalculadoraViewModel @Inject constructor(
    private val obtenerCombinacionesUseCase: ObtenerCombinacionesMateriaPrimaUseCase,
    private val obtenerVigenciasUseCase: ObtenerVigenciasListaPrecioFrutaUseCase,
    private val obtenerPreciosPorFechaUseCase: ObtenerPreciosPorFechaUseCase
) : ViewModel() {

    private val _estado = MutableStateFlow(EstadoCalculadora())
    val estado: StateFlow<EstadoCalculadora> = _estado.asStateFlow()

    init {
        cargar()
    }

    fun cargar() {
        viewModelScope.launch {
            _estado.update { it.copy(cargando = true, error = null) }
            try {
                val combinaciones = obtenerCombinacionesUseCase()
                _estado.update {
                    it.copy(cargando = false, filas = combinaciones.map(::filaVacia))
                }
            } catch (ex: SqlRepositoryException) {
                _estado.update { it.copy(cargando = false, error = ex.message) }
            }
        }
    }

    private fun filaVacia(combinacion: CombinacionMateriaPrima): FilaBanda = FilaBanda(
        categoriaId = combinacion.categoriaId,
        categoriaNombre = combinacion.categoriaNombre,
        calibreApeamId = combinacion.calibreApeamId,
        calibreApeamNombre = combinacion.calibreApeamNombre
    )

    fun cambiarLista(tipo: ListaPrecioFrutaTipo) {
        _estado.update { it.copy(listaSeleccionada = tipo) }
    }

    fun cambiarBusqueda(texto: String) {
        _estado.update { it.copy(busqueda = texto) }
    }

    fun cambiarPorcentaje(fila: FilaBanda, texto: String) {
        // Filtro de entrada: dígitos, punto decimal y signo negativo al inicio (Merma).
        // No se recorta a 100 aquí (arruinaría la escritura de decimales, ej. "1." se
        // volvería "1" a media captura) — el tope real se aplica en [finalizarEdicionPorcentaje],
        // al perder el foco.
        val limpio = texto.filterIndexed { indice, c -> c.isDigit() || c == '.' || (c == '-' && indice == 0) }
        actualizarFila(fila) { it.copy(porcentajeTexto = limpio) }
    }

    /**
     * Al salir del campo %Curva de una fila: recorta el valor para que (a) esa fila sola
     * nunca pase de 100% y (b) la suma de TODAS las filas nunca pase de 100% — pedido
     * explícito del usuario, más estricto que el simulador de escritorio (que solo
     * mostraba un aviso visual sin bloquear). El % puede seguir siendo negativo (Merma),
     * solo se topa el máximo.
     */
    fun finalizarEdicionPorcentaje(fila: FilaBanda) {
        _estado.update { estado ->
            val actual = estado.filas.firstOrNull {
                it.categoriaId == fila.categoriaId && it.calibreApeamId == fila.calibreApeamId
            } ?: return@update estado

            val sumaOtras = estado.filas
                .filterNot { it.categoriaId == fila.categoriaId && it.calibreApeamId == fila.calibreApeamId }
                .fold(BigDecimal.ZERO) { acc, f -> acc + f.porcentaje }
            val topeDisponible = BigDecimal(100) - sumaOtras
            val valorFinal = minOf(actual.porcentaje, BigDecimal(100), topeDisponible)
            val texto = if (valorFinal.signum() == 0) "0" else valorFinal.stripTrailingZeros().toPlainString()

            estado.copy(
                filas = estado.filas.map {
                    if (it.categoriaId == fila.categoriaId && it.calibreApeamId == fila.calibreApeamId) it.copy(porcentajeTexto = texto) else it
                }
            )
        }
    }

    private fun actualizarFila(fila: FilaBanda, transformar: (FilaBanda) -> FilaBanda) {
        _estado.update { estado ->
            estado.copy(
                filas = estado.filas.map {
                    if (it.categoriaId == fila.categoriaId && it.calibreApeamId == fila.calibreApeamId) transformar(it) else it
                }
            )
        }
    }

    fun solicitarLimpiar() {
        _estado.update { it.copy(mostrarConfirmarLimpiar = true) }
    }

    fun cancelarLimpiar() {
        _estado.update { it.copy(mostrarConfirmarLimpiar = false) }
    }

    fun confirmarLimpiar() {
        _estado.update { estado ->
            estado.copy(
                mostrarConfirmarLimpiar = false,
                filas = estado.filas.map { it.copy(precio = BigDecimal.ZERO, porcentajeTexto = "0") },
                origenPrecios = null
            )
        }
    }

    fun abrirSelectorPrecios() {
        _estado.update { it.copy(selectorAbierto = true, busquedaVigencias = "", vigenciaSeleccionadaEnSelector = null) }
        viewModelScope.launch {
            _estado.update { it.copy(cargandoVigencias = true) }
            try {
                val vigencias = obtenerVigenciasUseCase()
                _estado.update { it.copy(cargandoVigencias = false, vigencias = vigencias) }
            } catch (ex: SqlRepositoryException) {
                _estado.update { it.copy(cargandoVigencias = false, error = ex.message, selectorAbierto = false) }
            }
        }
    }

    fun cerrarSelectorPrecios() {
        _estado.update { it.copy(selectorAbierto = false) }
    }

    fun cambiarBusquedaVigencias(texto: String) {
        _estado.update { it.copy(busquedaVigencias = texto) }
    }

    fun elegirVigenciaEnSelector(vigencia: VigenciaListaPrecioFruta) {
        _estado.update { it.copy(vigenciaSeleccionadaEnSelector = vigencia) }
    }

    /** "Cargar" del selector — reemplaza SOLO la columna Precio (match por
     * categoriaId+calibreApeamId; sin match queda en 0), nunca toca el %Curva ya
     * capturado. Igual que BtnCargarPrecios_Click en SimuladorBandasForm.cs. */
    fun confirmarCargaPrecios() {
        val vigencia = _estado.value.vigenciaSeleccionadaEnSelector ?: run {
            _estado.update { it.copy(selectorAbierto = false) }
            return
        }
        val lista = _estado.value.listaSeleccionada
        viewModelScope.launch {
            try {
                val precios = obtenerPreciosPorFechaUseCase(vigencia.fecha, vigencia.productorId)
                _estado.update { estado ->
                    estado.copy(
                        selectorAbierto = false,
                        filas = estado.filas.map { fila ->
                            val match = precios.firstOrNull {
                                it.categoriaId == fila.categoriaId && it.calibreApeamId == fila.calibreApeamId
                            }
                            fila.copy(precio = match?.let(lista::precioDe) ?: BigDecimal.ZERO)
                        },
                        origenPrecios = OrigenPrecios(vigencia, lista)
                    )
                }
            } catch (ex: SqlRepositoryException) {
                _estado.update { it.copy(selectorAbierto = false, error = ex.message) }
            }
        }
    }
}
