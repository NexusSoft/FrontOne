package com.frontone.android.ui.conexion

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.domain.model.ResultadoConexion
import com.frontone.android.domain.usecase.ProbarConexionUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed interface EstadoProbarConexion {
    data object SinProbar : EstadoProbarConexion
    data object Probando : EstadoProbarConexion
    data class Exito(val versionServidor: String) : EstadoProbarConexion
    data class Error(val mensaje: String) : EstadoProbarConexion
}

/**
 * ViewModel del piloto — consume el caso de uso de :domain, nunca conoce JDBC
 * ni el adaptador concreto. Mismo rol que el constructor con servicios inyectados
 * de un {Entidad}EditarForm.cs en FrontOne.WinForms.
 */
@HiltViewModel
class ProbarConexionViewModel @Inject constructor(
    private val probarConexionUseCase: ProbarConexionUseCase
) : ViewModel() {

    private val _estado = MutableStateFlow<EstadoProbarConexion>(EstadoProbarConexion.SinProbar)
    val estado: StateFlow<EstadoProbarConexion> = _estado.asStateFlow()

    fun probarConexion() {
        _estado.value = EstadoProbarConexion.Probando
        viewModelScope.launch {
            _estado.value = when (val resultado = probarConexionUseCase()) {
                is ResultadoConexion.Exitosa -> EstadoProbarConexion.Exito(resultado.versionServidor)
                is ResultadoConexion.Fallida -> EstadoProbarConexion.Error(resultado.mensaje)
            }
        }
    }
}
