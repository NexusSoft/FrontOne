package com.frontone.android.ui.acopio

import androidx.activity.compose.BackHandler
import androidx.compose.animation.Crossfade
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Agriculture
import androidx.compose.material.icons.filled.Handshake
import androidx.compose.material.icons.filled.ReportProblem
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.graphics.Color
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.ui.acopio.calculadora.CalculadoraScreen

private sealed interface RutaAcopio {
    data object Dashboard : RutaAcopio
    data object Huertas : RutaAcopio
    data object Calculadora : RutaAcopio
    data object AcuerdosCorte : RutaAcopio
    data object Incidencias : RutaAcopio
}

/**
 * Sub-navegación Dashboard→Submódulo del módulo Acopio, encapsulada aquí — mismo
 * criterio que `PalletsHostScreen.kt` (MainActivity solo conoce un único destino
 * "Acopio"). A diferencia de Pallets (Lista↔Captura de una sola entidad), acá el
 * primer nivel es un dashboard propio con 4 tarjetas de submódulo — ninguna tiene
 * captura real todavía, así que las 4 rutas caen en `SubmoduloProximamenteScreen`.
 * Cuando un submódulo se construya de verdad, se le agrega su propio Screen+
 * ViewModel (ver `ui/pallets/`) y aquí solo cambia a qué composable apunta su rama
 * del `when` — la forma de navegación no cambia.
 */
@Composable
fun AcopioHostScreen(permisos: List<PermisoUsuario>, onVolverAInicioClick: () -> Unit) {
    var ruta by remember { mutableStateOf<RutaAcopio>(RutaAcopio.Dashboard) }

    BackHandler(enabled = ruta is RutaAcopio.Dashboard) { onVolverAInicioClick() }
    BackHandler(enabled = ruta !is RutaAcopio.Dashboard) { ruta = RutaAcopio.Dashboard }

    Crossfade(targetState = ruta, label = "navegacionAcopio") { rutaActual ->
        when (rutaActual) {
            is RutaAcopio.Dashboard -> AcopioDashboardScreen(
                onVolverClick = onVolverAInicioClick,
                onSubmoduloClick = { nombre ->
                    ruta = when (nombre) {
                        "Huertas" -> RutaAcopio.Huertas
                        "Calculadora" -> RutaAcopio.Calculadora
                        "Acuerdos de Corte" -> RutaAcopio.AcuerdosCorte
                        "Incidencias" -> RutaAcopio.Incidencias
                        else -> RutaAcopio.Dashboard
                    }
                }
            )

            is RutaAcopio.Huertas -> SubmoduloProximamenteScreen(
                nombre = "Huertas",
                color = Color(0xFF2F9E6E),
                icono = Icons.Filled.Agriculture,
                onVolverClick = { ruta = RutaAcopio.Dashboard }
            )

            is RutaAcopio.Calculadora -> CalculadoraScreen(
                onVolverClick = { ruta = RutaAcopio.Dashboard }
            )

            is RutaAcopio.AcuerdosCorte -> SubmoduloProximamenteScreen(
                nombre = "Acuerdos de Corte",
                color = Color(0xFFC98A3F),
                icono = Icons.Filled.Handshake,
                onVolverClick = { ruta = RutaAcopio.Dashboard }
            )

            is RutaAcopio.Incidencias -> SubmoduloProximamenteScreen(
                nombre = "Incidencias",
                color = Color(0xFFD1495B),
                icono = Icons.Filled.ReportProblem,
                onVolverClick = { ruta = RutaAcopio.Dashboard }
            )
        }
    }
}
