package com.frontone.android.ui.login

import android.graphics.BitmapFactory
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.asImageBitmap
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.config.CredencialesGuardadas
import com.frontone.android.config.CredencialesRecordadasStore
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.ResultadoLogin
import com.frontone.android.domain.model.Usuario
import com.frontone.android.domain.usecase.LoginUseCase
import com.frontone.android.domain.usecase.ObtenerLogoEmpresaUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import javax.inject.Inject

sealed interface EstadoLogo {
    data object Cargando : EstadoLogo
    data class Disponible(val imagen: ImageBitmap) : EstadoLogo
    data object NoDisponible : EstadoLogo
}

sealed interface EstadoLogin {
    data object Inicial : EstadoLogin
    data object Autenticando : EstadoLogin
    data class Exitoso(val usuario: Usuario, val permisos: List<PermisoUsuario>) : EstadoLogin
    data class Error(val mensaje: String) : EstadoLogin
}

/**
 * Trae el logo de Configuracion.Empresa al abrir el login (decodificación de bytes a
 * Bitmap vive aquí, capa :app, porque android.graphics no está disponible en
 * :domain/:data), las credenciales recordadas si el usuario marcó "Recordarme" antes,
 * y maneja la autenticación real contra Seguridad.Usuario vía LoginUseCase — mismo
 * flujo que AuthService.LoginAsync (C#).
 *
 * El fondo del splash (ui/login/LoginSplashScreen.kt) NO se calcula a partir del
 * logo — se probó extracción de color con Palette y se descartó, ver
 * contexto/arquitectura.md. Este ViewModel solo entrega la imagen.
 */
@HiltViewModel
class LoginViewModel @Inject constructor(
    private val obtenerLogoEmpresaUseCase: ObtenerLogoEmpresaUseCase,
    private val loginUseCase: LoginUseCase,
    private val credencialesRecordadasStore: CredencialesRecordadasStore
) : ViewModel() {

    private val _estadoLogo = MutableStateFlow<EstadoLogo>(EstadoLogo.Cargando)
    val estadoLogo: StateFlow<EstadoLogo> = _estadoLogo.asStateFlow()

    private val _estadoLogin = MutableStateFlow<EstadoLogin>(EstadoLogin.Inicial)
    val estadoLogin: StateFlow<EstadoLogin> = _estadoLogin.asStateFlow()

    private val _credencialesGuardadas = MutableStateFlow<CredencialesGuardadas?>(null)
    val credencialesGuardadas: StateFlow<CredencialesGuardadas?> = _credencialesGuardadas.asStateFlow()

    init {
        cargarLogo()
        cargarCredencialesGuardadas()
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

    private fun cargarCredencialesGuardadas() {
        viewModelScope.launch {
            _credencialesGuardadas.value = withContext(Dispatchers.IO) { credencialesRecordadasStore.obtener() }
        }
    }

    fun iniciarSesion(usuario: String, contrasena: String, recordarme: Boolean) {
        if (usuario.isBlank() || contrasena.isBlank()) {
            _estadoLogin.value = EstadoLogin.Error("Captura usuario y contraseña.")
            return
        }
        _estadoLogin.value = EstadoLogin.Autenticando
        viewModelScope.launch {
            val resultado = loginUseCase(usuario, contrasena)
            _estadoLogin.value = when (resultado) {
                is ResultadoLogin.Exitoso -> {
                    withContext(Dispatchers.IO) {
                        if (recordarme) {
                            credencialesRecordadasStore.guardar(usuario, contrasena)
                        } else {
                            credencialesRecordadasStore.limpiar()
                        }
                    }
                    EstadoLogin.Exitoso(resultado.usuario, resultado.permisos)
                }
                is ResultadoLogin.Fallido -> EstadoLogin.Error(resultado.mensaje)
            }
        }
    }
}
