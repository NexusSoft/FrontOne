package com.frontone.android

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Settings
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.frontone.android.ui.configuracion.ConfiguracionConexionScreen
import com.frontone.android.ui.login.LoginScreen
import com.frontone.android.ui.theme.FrontOneTheme
import dagger.hilt.android.AndroidEntryPoint

/**
 * Punto de entrada de la app — equivalente a MainForm.cs de FrontOne.WinForms,
 * salvo que aquí no hay Ribbon: la navegación entre pantallas se agrega cuando
 * exista más de un módulo real (hoy es un switch local simple, no NavController).
 *
 * LoginScreen es la pantalla de entrada. El ícono de ajustes suelto (abajo-derecha,
 * sin contenedor/FAB de fondo — solo el ícono) abre ConfiguracionConexionScreen,
 * equivalente móvil de ConfiguracionConexionesForm.
 */
@AndroidEntryPoint
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            FrontOneTheme {
                var mostrarConfiguracion by remember { mutableStateOf(false) }

                if (mostrarConfiguracion) {
                    ConfiguracionConexionScreen(onVolverClick = { mostrarConfiguracion = false })
                } else {
                    Box {
                        LoginScreen()
                        IconButton(
                            onClick = { mostrarConfiguracion = true },
                            modifier = Modifier
                                .align(Alignment.BottomEnd)
                                .padding(24.dp)
                                .size(40.dp)
                        ) {
                            Icon(
                                Icons.Filled.Settings,
                                contentDescription = "Configuración de Conexión",
                                modifier = Modifier.size(32.dp)
                            )
                        }
                    }
                }
            }
        }
    }
}
