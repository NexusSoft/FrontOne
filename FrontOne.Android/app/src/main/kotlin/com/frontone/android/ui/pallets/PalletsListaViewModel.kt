package com.frontone.android.ui.pallets

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.data.sqlserver.SqlRepositoryException
import com.frontone.android.domain.model.EstatusPallet
import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.usecase.ObtenerPalletsUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed interface EstadoPalletsLista {
    data object Cargando : EstadoPalletsLista
    data class Cargado(val pallets: List<Pallet>) : EstadoPalletsLista
    data class Error(val mensaje: String) : EstadoPalletsLista
}

private const val INTERVALO_ACTUALIZACION_MS = 5_000L

/**
 * No hay SP de filtro por Estatus (sp_Pallet_Obtener solo filtra por @Id) — el filtro por
 * Estatus del listado se hace en memoria, aquí, sobre la lista completa ya traída.
 *
 * Sin caché offline ni WebSockets/SignalR (la app es *online-only* por regla dura del
 * proyecto) — para que un pallet creado/modificado desde otro dispositivo se vea "en tiempo
 * real" sin que el usuario tenga que salir y volver a entrar, se refresca en segundo plano cada
 * [INTERVALO_ACTUALIZACION_MS] mientras la pantalla esté visible (el bucle vive en
 * `viewModelScope`, se cancela solo cuando el ViewModel se destruye). El refresco es silencioso:
 * nunca vuelve a `Cargando` ni interrumpe el filtro/búsqueda que el usuario ya tenga puesto, solo
 * reemplaza la lista — y si una actualización de fondo falla (ej. una desconexión momentánea), se
 * ignora en vez de tirar la pantalla a un estado de Error por algo que no fue una acción del
 * usuario; el próximo tick lo vuelve a intentar.
 */
@HiltViewModel
class PalletsListaViewModel @Inject constructor(
    private val obtenerPalletsUseCase: ObtenerPalletsUseCase
) : ViewModel() {

    private val _estado = MutableStateFlow<EstadoPalletsLista>(EstadoPalletsLista.Cargando)
    val estado: StateFlow<EstadoPalletsLista> = _estado.asStateFlow()

    private val _filtroEstatus = MutableStateFlow<EstatusPallet?>(null)
    val filtroEstatus: StateFlow<EstatusPallet?> = _filtroEstatus.asStateFlow()

    init {
        cargar()
        iniciarActualizacionPeriodica()
    }

    fun cargar() {
        _estado.value = EstadoPalletsLista.Cargando
        viewModelScope.launch {
            _estado.value = try {
                EstadoPalletsLista.Cargado(obtenerPalletsUseCase())
            } catch (ex: SqlRepositoryException) {
                EstadoPalletsLista.Error(ex.message ?: "Ocurrió un error al comunicarse con el servidor.")
            }
        }
    }

    private fun iniciarActualizacionPeriodica() {
        viewModelScope.launch {
            while (true) {
                delay(INTERVALO_ACTUALIZACION_MS)
                try {
                    _estado.value = EstadoPalletsLista.Cargado(obtenerPalletsUseCase())
                } catch (ex: SqlRepositoryException) {
                    // Silencioso a propósito — ver KDoc de la clase.
                }
            }
        }
    }

    fun cambiarFiltro(estatus: EstatusPallet?) {
        _filtroEstatus.value = estatus
    }
}
