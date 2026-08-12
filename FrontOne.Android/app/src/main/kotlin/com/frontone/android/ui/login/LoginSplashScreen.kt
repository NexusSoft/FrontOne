package com.frontone.android.ui.login

import androidx.compose.animation.core.animateFloatAsState
import androidx.compose.animation.core.tween
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Surface
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.alpha
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.unit.dp

/**
 * Pantalla de bienvenida a pantalla completa. Fondo: mismo degradado azul que la
 * cabecera/botón "Iniciar sesión" de LoginScreen (AzulDiseno1→2→3, mismo paquete) —
 * antes usaba una paleta distinta (AzulSplashOscuro/AzulFrontOne, de una iteración
 * anterior a la importación del diseño real) y quedaban dos azules distintos
 * conviviendo en la app; unificado a pedido del usuario. Ver contexto/arquitectura.md
 * para el historial completo del splash (extracción dinámica de color vía Palette,
 * después logo "flotando" directo sobre el degradado — ambas descartadas por
 * problemas de contraste con el texto del logo real de Fronterra).
 *
 * Solución vigente: el logo vive dentro de una TARJETA BLANCA propia, nunca directo
 * sobre el degradado — el logo de Fronterra (texto gris oscuro semitransparente) se
 * diseñó para fondo claro, así que ponerlo sobre blanco es la única forma de
 * garantizar que nunca se confunda con el fondo, sin importar qué logo se cargue en
 * el futuro desde Configuracion.Empresa.
 */
private val DegradadoFondoSplash = Brush.linearGradient(
    colors = listOf(AzulDiseno1, AzulDiseno2, AzulDiseno3),
    start = Offset(0f, 0f),
    end = Offset(1000f, 1400f)
)

@Composable
fun LoginSplashScreen(estadoLogo: EstadoLogo) {
    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(DegradadoFondoSplash),
        contentAlignment = Alignment.Center
    ) {
        when (estadoLogo) {
            is EstadoLogo.Disponible -> {
                val opacidad by animateFloatAsState(
                    targetValue = 1f,
                    animationSpec = tween(durationMillis = 500),
                    label = "opacidadLogoSplash"
                )
                Surface(
                    modifier = Modifier
                        .alpha(opacidad)
                        .width(300.dp),
                    shape = RoundedCornerShape(24.dp),
                    color = Color.White,
                    shadowElevation = 12.dp
                ) {
                    Image(
                        bitmap = estadoLogo.imagen,
                        contentDescription = "Logo de la empresa",
                        contentScale = ContentScale.Fit,
                        modifier = Modifier
                            .fillMaxWidth()
                            .height(100.dp)
                            .padding(horizontal = 32.dp, vertical = 20.dp)
                    )
                }
            }

            else -> CircularProgressIndicator(color = Color.White)
        }
    }
}
