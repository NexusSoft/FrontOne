package com.frontone.android.ui.pallets

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.data.sqlserver.SqlRepositoryException
import com.frontone.android.domain.model.LineaProduccion
import com.frontone.android.domain.model.LoteEnProceso
import com.frontone.android.domain.model.Pallet
import com.frontone.android.domain.model.PalletDetalle
import com.frontone.android.domain.model.ProductoTerminado
import com.frontone.android.domain.usecase.ActualizarEncabezadoPalletUseCase
import com.frontone.android.domain.usecase.ActualizarLineaPalletUseCase
import com.frontone.android.domain.usecase.AgregarLineaPalletUseCase
import com.frontone.android.domain.usecase.BloquearPalletUseCase
import com.frontone.android.domain.usecase.CrearPalletUseCase
import com.frontone.android.domain.usecase.EliminarLineaPalletUseCase
import com.frontone.android.domain.usecase.EliminarPalletUseCase
import com.frontone.android.domain.usecase.ObtenerLineasProduccionUseCase
import com.frontone.android.domain.usecase.ObtenerLotesEnProcesoUseCase
import com.frontone.android.domain.usecase.ObtenerPalletDetalleUseCase
import com.frontone.android.domain.usecase.ObtenerPalletsUseCase
import com.frontone.android.domain.usecase.ObtenerProductosTerminadosUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import java.math.BigDecimal
import javax.inject.Inject

private const val INTERVALO_ACTUALIZACION_MS = 5_000L

sealed interface EstadoPalletCaptura {
    data object Cargando : EstadoPalletCaptura
    data class Formulario(
        val palletId: Int?,
        val pallet: Pallet?,
        val detalle: List<PalletDetalle>,
        val lineasProduccion: List<LineaProduccion>,
        val productosEncabezado: List<ProductoTerminado>,
        val guardandoEncabezado: Boolean = false,
        val mensajeError: String? = null,
        val eliminado: Boolean = false
    ) : EstadoPalletCaptura
    data class ErrorCarga(val mensaje: String) : EstadoPalletCaptura
}

/**
 * Orquesta la captura completa de un Pallet — equivalente a PalletEditarForm.cs +
 * PalletDetalleCapturaForm.cs (FrontOne WinForms), aplanado en un solo ViewModel porque acá
 * no hay Designer.cs/DataNavigator, solo Compose.
 *
 * Restricción real que se replica tal cual: mientras [EstadoPalletCaptura.Formulario.palletId]
 * sea null (alta nueva sin guardar), la sección de líneas de detalle no se puede mostrar — el
 * encabezado debe existir en la BD primero (mismo criterio que escritorio).
 */
@HiltViewModel
class PalletCapturaViewModel @Inject constructor(
    private val obtenerPalletsUseCase: ObtenerPalletsUseCase,
    private val obtenerPalletDetalleUseCase: ObtenerPalletDetalleUseCase,
    private val obtenerLotesEnProcesoUseCase: ObtenerLotesEnProcesoUseCase,
    private val crearPalletUseCase: CrearPalletUseCase,
    private val actualizarEncabezadoPalletUseCase: ActualizarEncabezadoPalletUseCase,
    private val bloquearPalletUseCase: BloquearPalletUseCase,
    private val eliminarPalletUseCase: EliminarPalletUseCase,
    private val agregarLineaPalletUseCase: AgregarLineaPalletUseCase,
    private val actualizarLineaPalletUseCase: ActualizarLineaPalletUseCase,
    private val eliminarLineaPalletUseCase: EliminarLineaPalletUseCase,
    private val obtenerLineasProduccionUseCase: ObtenerLineasProduccionUseCase,
    private val obtenerProductosTerminadosUseCase: ObtenerProductosTerminadosUseCase
) : ViewModel() {

    private val _estado = MutableStateFlow<EstadoPalletCaptura>(EstadoPalletCaptura.Cargando)
    val estado: StateFlow<EstadoPalletCaptura> = _estado.asStateFlow()

    private val _lotesEnProceso = MutableStateFlow<List<LoteEnProceso>>(emptyList())
    val lotesEnProceso: StateFlow<List<LoteEnProceso>> = _lotesEnProceso.asStateFlow()

    private val _productosBusqueda = MutableStateFlow<List<ProductoTerminado>>(emptyList())
    val productosBusqueda: StateFlow<List<ProductoTerminado>> = _productosBusqueda.asStateFlow()

    private var inicializado = false

    fun inicializar(palletId: Int?) {
        if (inicializado) return
        inicializado = true
        iniciarActualizacionPeriodica()
        viewModelScope.launch {
            try {
                val lineasProduccion = obtenerLineasProduccionUseCase()
                val productos = obtenerProductosTerminadosUseCase()
                if (palletId == null) {
                    _estado.value = EstadoPalletCaptura.Formulario(
                        palletId = null,
                        pallet = null,
                        detalle = emptyList(),
                        lineasProduccion = lineasProduccion,
                        productosEncabezado = productos
                    )
                } else {
                    val pallet = obtenerPalletsUseCase(palletId).firstOrNull()
                        ?: throw SqlRepositoryException("Este pallet ya no existe.")
                    val detalle = obtenerPalletDetalleUseCase(palletId)
                    _estado.value = EstadoPalletCaptura.Formulario(
                        palletId = palletId,
                        pallet = pallet,
                        detalle = detalle,
                        lineasProduccion = lineasProduccion,
                        productosEncabezado = productos
                    )
                }
            } catch (ex: SqlRepositoryException) {
                _estado.value = EstadoPalletCaptura.ErrorCarga(ex.message ?: "Ocurrió un error al comunicarse con el servidor.")
            }
        }
    }

    /**
     * Igual que en la Lista (`PalletsListaViewModel`): sin caché offline ni notificaciones
     * push, un pallet modificado desde otro dispositivo (u otra Corrida que se finaliza y deja
     * una línea de solo lectura) solo se refleja acá con un refresco periódico. Silencioso —
     * nunca interrumpe con un error de fondo ni pisa lo que el usuario esté escribiendo en el
     * formulario (los campos del encabezado y el diálogo de línea son estado local del
     * Composable, `recargar` solo toca `pallet`/`detalle`, no esos campos).
     */
    private fun iniciarActualizacionPeriodica() {
        viewModelScope.launch {
            while (true) {
                delay(INTERVALO_ACTUALIZACION_MS)
                val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: continue
                val palletId = formularioActual.palletId ?: continue
                try {
                    recargar(palletId)
                } catch (ex: SqlRepositoryException) {
                    // Silencioso a propósito — ver KDoc de la función.
                }
            }
        }
    }

    private suspend fun recargar(palletId: Int) {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        val pallet = obtenerPalletsUseCase(palletId).firstOrNull()
        val detalle = obtenerPalletDetalleUseCase(palletId)
        _estado.value = formularioActual.copy(palletId = palletId, pallet = pallet, detalle = detalle)
    }

    fun guardarEncabezado(
        lineaProduccionId: Int,
        esMixto: Boolean,
        productoTerminadoId: Int?,
        pesoReal: BigDecimal?
    ) {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        viewModelScope.launch {
            _estado.value = formularioActual.copy(guardandoEncabezado = true, mensajeError = null)
            try {
                val idFinal = if (formularioActual.palletId == null) {
                    crearPalletUseCase(lineaProduccionId, esMixto, productoTerminadoId, pesoReal)
                } else {
                    actualizarEncabezadoPalletUseCase(
                        formularioActual.palletId, lineaProduccionId, esMixto, productoTerminadoId, pesoReal
                    )
                    formularioActual.palletId
                }
                recargar(idFinal)
                _estado.value = (_estado.value as EstadoPalletCaptura.Formulario).copy(guardandoEncabezado = false)
            } catch (ex: SqlRepositoryException) {
                _estado.value = formularioActual.copy(
                    guardandoEncabezado = false,
                    mensajeError = ex.message ?: "Ocurrió un error al comunicarse con el servidor."
                )
            }
        }
    }

    fun cargarLotesEnProceso(lineaProduccionId: Int?) {
        viewModelScope.launch {
            _lotesEnProceso.value = try {
                obtenerLotesEnProcesoUseCase(lineaProduccionId)
            } catch (ex: SqlRepositoryException) {
                emptyList()
            }
        }
    }

    fun buscarProductos(filtro: String) {
        viewModelScope.launch {
            _productosBusqueda.value = try {
                obtenerProductosTerminadosUseCase(filtro)
            } catch (ex: SqlRepositoryException) {
                emptyList()
            }
        }
    }

    fun agregarLinea(
        corridaId: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal,
        alTerminar: (String?) -> Unit
    ) {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        val palletId = formularioActual.palletId ?: return
        viewModelScope.launch {
            try {
                agregarLineaPalletUseCase(palletId, corridaId, productoTerminadoId, cajas, kilogramos, porcentajeMateriaSeca)
                recargar(palletId)
                alTerminar(null)
            } catch (ex: SqlRepositoryException) {
                alTerminar(ex.message ?: "Ocurrió un error al comunicarse con el servidor.")
            }
        }
    }

    fun editarLinea(
        id: Int,
        productoTerminadoId: Int,
        cajas: Int?,
        kilogramos: BigDecimal?,
        porcentajeMateriaSeca: BigDecimal,
        alTerminar: (String?) -> Unit
    ) {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        val palletId = formularioActual.palletId ?: return
        viewModelScope.launch {
            try {
                actualizarLineaPalletUseCase(id, productoTerminadoId, cajas, kilogramos, porcentajeMateriaSeca)
                recargar(palletId)
                alTerminar(null)
            } catch (ex: SqlRepositoryException) {
                alTerminar(ex.message ?: "Ocurrió un error al comunicarse con el servidor.")
            }
        }
    }

    fun eliminarLinea(id: Int) {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        val palletId = formularioActual.palletId ?: return
        viewModelScope.launch {
            try {
                eliminarLineaPalletUseCase(id)
                recargar(palletId)
            } catch (ex: SqlRepositoryException) {
                _estado.value = formularioActual.copy(mensajeError = ex.message ?: "Ocurrió un error al comunicarse con el servidor.")
            }
        }
    }

    fun bloquear() {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        val palletId = formularioActual.palletId ?: return
        viewModelScope.launch {
            try {
                bloquearPalletUseCase(palletId)
                recargar(palletId)
            } catch (ex: SqlRepositoryException) {
                _estado.value = formularioActual.copy(mensajeError = ex.message ?: "Ocurrió un error al comunicarse con el servidor.")
            }
        }
    }

    fun eliminarPallet() {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        val palletId = formularioActual.palletId ?: return
        viewModelScope.launch {
            try {
                eliminarPalletUseCase(palletId)
                _estado.value = formularioActual.copy(eliminado = true)
            } catch (ex: SqlRepositoryException) {
                _estado.value = formularioActual.copy(mensajeError = ex.message ?: "Ocurrió un error al comunicarse con el servidor.")
            }
        }
    }

    fun limpiarMensajeError() {
        val formularioActual = _estado.value as? EstadoPalletCaptura.Formulario ?: return
        _estado.value = formularioActual.copy(mensajeError = null)
    }
}
