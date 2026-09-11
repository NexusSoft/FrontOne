package com.frontone.android.ui.acopio.huertas

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.domain.model.HuertaMapa
import com.frontone.android.domain.usecase.ActualizarUbicacionHuertaUseCase
import com.frontone.android.domain.usecase.BuscarHuertasMapaUseCase
import com.frontone.android.domain.usecase.ObtenerHuertasMapaUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import java.math.BigDecimal
import javax.inject.Inject

data class EstadoHuertasMapa(
    val cargando: Boolean = true,
    val error: String? = null,
    val mensaje: String? = null,
    val huertas: List<HuertaMapa> = emptyList(),
    val busqueda: String = "",
    val huertaSeleccionada: HuertaMapa? = null,
    val modoEdicionActivo: Boolean = false,
    val guardandoUbicacion: Boolean = false
)

/**
 * Mapa del submódulo Acopio > Huertas — consulta de huertas con coordenadas y
 * corrección de ubicación (Latitud/Longitud) desde el celular. Única excepción a la
 * regla de que los catálogos se quedan en escritorio (ver `CLAUDE.md` de Android,
 * pedido explícito del usuario) — el resto de los datos de la huerta sigue siendo de
 * solo lectura aquí, se editan en `HuertaEditarForm` de escritorio.
 */
@HiltViewModel
class HuertasMapaViewModel @Inject constructor(
    private val obtenerHuertasMapaUseCase: ObtenerHuertasMapaUseCase,
    private val buscarHuertasMapaUseCase: BuscarHuertasMapaUseCase,
    private val actualizarUbicacionHuertaUseCase: ActualizarUbicacionHuertaUseCase
) : ViewModel() {

    private val _estado = MutableStateFlow(EstadoHuertasMapa())
    val estado: StateFlow<EstadoHuertasMapa> = _estado.asStateFlow()

    init {
        cargar()
    }

    fun cargar() {
        viewModelScope.launch {
            _estado.update { it.copy(cargando = true, error = null) }
            try {
                val huertas = obtenerHuertasMapaUseCase()
                _estado.update { it.copy(cargando = false, huertas = huertas) }
            } catch (ex: Exception) {
                _estado.update { it.copy(cargando = false, error = ex.message ?: "No se pudieron cargar las huertas.") }
            }
        }
    }

    /** Búsqueda por Registro SAGARPA, Nombre o Productor — mínimo 2 caracteres, igual
     * que el resto de los buscadores del proyecto; con menos, regresa a la carga
     * inicial (TOP 100). */
    fun cambiarBusqueda(texto: String) {
        _estado.update { it.copy(busqueda = texto) }
        val filtro = texto.trim()
        if (filtro.length < 2) {
            if (filtro.isEmpty()) cargar()
            return
        }
        viewModelScope.launch {
            _estado.update { it.copy(cargando = true, error = null) }
            try {
                val huertas = buscarHuertasMapaUseCase(filtro)
                _estado.update { it.copy(cargando = false, huertas = huertas) }
            } catch (ex: Exception) {
                _estado.update { it.copy(cargando = false, error = ex.message ?: "No se pudo buscar.") }
            }
        }
    }

    fun seleccionarHuerta(huerta: HuertaMapa) {
        _estado.update { it.copy(huertaSeleccionada = huerta) }
    }

    fun cerrarDetalle() {
        _estado.update { it.copy(huertaSeleccionada = null) }
    }

    fun iniciarEdicionUbicacion() {
        _estado.update { it.copy(modoEdicionActivo = true) }
    }

    fun cancelarEdicionUbicacion() {
        _estado.update { estado ->
            // Restaura la huerta seleccionada a su coordenada real (descarta el
            // arrastre en memoria si no se guardó).
            val original = estado.huertas.firstOrNull { it.id == estado.huertaSeleccionada?.id }
            estado.copy(modoEdicionActivo = false, huertaSeleccionada = original)
        }
    }

    /** Mueve el pin en memoria (sin guardar todavía) — se llama al tocar el mapa
     * mientras `modoEdicionActivo` está activo. */
    fun moverPinTemporal(latitud: BigDecimal, longitud: BigDecimal) {
        _estado.update { estado ->
            val actual = estado.huertaSeleccionada ?: return@update estado
            estado.copy(huertaSeleccionada = actual.copy(latitud = latitud, longitud = longitud))
        }
    }

    fun guardarUbicacion() {
        val huerta = _estado.value.huertaSeleccionada ?: return
        viewModelScope.launch {
            _estado.update { it.copy(guardandoUbicacion = true, error = null) }
            try {
                actualizarUbicacionHuertaUseCase(huerta.id, huerta.latitud, huerta.longitud)
                _estado.update { estado ->
                    estado.copy(
                        guardandoUbicacion = false,
                        modoEdicionActivo = false,
                        huertaSeleccionada = null,
                        mensaje = "Ubicación de \"${huerta.nombre}\" actualizada.",
                        huertas = estado.huertas.map { if (it.id == huerta.id) huerta else it }
                    )
                }
            } catch (ex: Exception) {
                _estado.update { it.copy(guardandoUbicacion = false, error = ex.message ?: "No se pudo guardar la ubicación.") }
            }
        }
    }

    fun limpiarMensaje() {
        _estado.update { it.copy(mensaje = null) }
    }

    fun limpiarError() {
        _estado.update { it.copy(error = null) }
    }
}
