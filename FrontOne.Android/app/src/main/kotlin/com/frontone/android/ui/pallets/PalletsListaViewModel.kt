package com.frontone.android.ui.pallets

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.data.sqlserver.SqlRepositoryException
import com.frontone.android.domain.model.EstatusPallet
import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.usecase.ObtenerPalletsUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
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

/**
 * No hay SP de filtro por Estatus (sp_Pallet_Obtener solo filtra por @Id) — el filtro por
 * Estatus del listado se hace en memoria, aquí, sobre la lista completa ya traída.
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

    fun cambiarFiltro(estatus: EstatusPallet?) {
        _filtroEstatus.value = estatus
    }
}
