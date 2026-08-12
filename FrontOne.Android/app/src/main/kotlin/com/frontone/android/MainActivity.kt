package com.frontone.android

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.animation.Crossfade
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.Usuario
import com.frontone.android.ui.configuracion.ConfiguracionConexionScreen
import com.frontone.android.ui.inicio.InicioScreen
import com.frontone.android.ui.login.EstadoLogo
import com.frontone.android.ui.login.LoginScreen
import com.frontone.android.ui.login.LoginSplashScreen
import com.frontone.android.ui.login.LoginViewModel
import com.frontone.android.ui.theme.FrontOneTheme
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.first
import kotlinx.coroutines.withTimeoutOrNull

private enum class Pantalla { SPLASH, LOGIN, CONFIGURACION, INICIO }

/**
 * Punto de entrada de la app — equivalente a MainForm.cs de FrontOne.WinForms,
 * salvo que aquí no hay Ribbon: la navegación entre pantallas se agrega cuando
 * exista más de un módulo real (hoy es un switch local simple, no NavController).
 *
 * Flujo: Splash → Login → Inicio (cuadrícula de módulos, post-login) — o
 * Configuración de Conexión desde el ícono de ajustes del Login. LoginViewModel se
 * obtiene una sola vez aquí arriba y se comparte entre Splash y Login para no
 * disparar dos veces la consulta del logo.
 */
@AndroidEntryPoint
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            FrontOneTheme {
                var pantalla by remember { mutableStateOf(Pantalla.SPLASH) }
                var usuarioAutenticado by remember { mutableStateOf<Usuario?>(null) }
                var permisosUsuario by remember { mutableStateOf<List<PermisoUsuario>>(emptyList()) }
                val loginViewModel: LoginViewModel = hiltViewModel()
                val estadoLogo by loginViewModel.estadoLogo.collectAsState()

                // Espera a que el logo termine de resolverse (Disponible o
                // NoDisponible) antes de pasar a Login, con un tope de 4s por si la
                // conexión está lenta — nunca deja el splash trabado indefinidamente.
                // El +400ms es solo para que la transición no se sienta como un
                // parpadeo instantáneo cuando el logo carga muy rápido.
                LaunchedEffect(Unit) {
                    withTimeoutOrNull(4000) {
                        loginViewModel.estadoLogo.first { it !is EstadoLogo.Cargando }
                    }
                    delay(400)
                    pantalla = Pantalla.LOGIN
                }

                Crossfade(targetState = pantalla, label = "navegacionPrincipal") { pantallaActual ->
                    when (pantallaActual) {
                        Pantalla.SPLASH -> LoginSplashScreen(estadoLogo = estadoLogo)

                        Pantalla.CONFIGURACION -> ConfiguracionConexionScreen(
                            onVolverClick = { pantalla = Pantalla.LOGIN }
                        )

                        Pantalla.LOGIN -> LoginScreen(
                            viewModel = loginViewModel,
                            onConfiguracionClick = { pantalla = Pantalla.CONFIGURACION },
                            onLoginExitoso = { usuario, permisos ->
                                usuarioAutenticado = usuario
                                permisosUsuario = permisos
                                pantalla = Pantalla.INICIO
                            }
                        )

                        Pantalla.INICIO -> usuarioAutenticado?.let { usuario ->
                            InicioScreen(usuario = usuario, estadoLogo = estadoLogo, permisos = permisosUsuario)
                        }
                    }
                }
            }
        }
    }
}
