package com.frontone.android.ui.login

import android.graphics.BitmapFactory
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.asImageBitmap
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.domain.usecase.ObtenerLogoEmpresaUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed interface EstadoLogo {
    data object Cargando : EstadoLogo
    data class Disponible(val imagen: ImageBitmap) : EstadoLogo
    data object NoDisponible : EstadoLogo
}

/**
 * Trae el logo de Configuracion.Empresa al abrir el login. La decodificación de bytes
 * a Bitmap vive aquí (capa :app) porque android.graphics no está disponible en
 * :domain/:data — el caso de uso solo entrega el ByteArray crudo.
 *
 * Si la consulta falla (sin conexión, servidor caído, tabla sin logo cargado todavía),
 * se degrada a NoDisponible en silencio — el login debe seguir siendo usable aunque
 * el logo no cargue, no es una función crítica.
 */
@HiltViewModel
class LoginViewModel @Inject constructor(
    private val obtenerLogoEmpresaUseCase: ObtenerLogoEmpresaUseCase
) : ViewModel() {

    private val _estadoLogo = MutableStateFlow<EstadoLogo>(EstadoLogo.Cargando)
    val estadoLogo: StateFlow<EstadoLogo> = _estadoLogo.asStateFlow()

    init {
        cargarLogo()
    }

    private fun cargarLogo() {
        viewModelScope.launch {
            _estadoLogo.value = runCatching { obtenerLogoEmpresaUseCase() }
                .getOrNull()
                ?.let { bytes -> BitmapFactory.decodeByteArray(bytes, 0, bytes.size) }
                ?.let { bitmap -> EstadoLogo.Disponible(bitmap.asImageBitmap()) }
                ?: EstadoLogo.NoDisponible
        }
    }
}
