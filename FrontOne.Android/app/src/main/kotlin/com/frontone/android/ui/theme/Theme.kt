package com.frontone.android.ui.theme

import androidx.compose.foundation.isSystemInDarkTheme
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color

// Paleta base — morado/índigo profesional (mismo tono que ya se ve en el botón por
// defecto de Material3). Reemplazar por los colores de marca reales de Fronterra en
// cuanto se defina identidad visual (logo/paleta oficial) — hoy es un placeholder
// deliberado, no una decisión de marca final.
val AzulFrontOne = Color(0xFF3949AB)
private val AzulFrontOneOscuro = Color(0xFF7986CB)
private val AzulFrontOneContenedor = Color(0xFFDEE1FF)
private val FondoClaro = Color(0xFFF7F7FB)
private val FondoOscuro = Color(0xFF121218)

private val EsquemaClaro = lightColorScheme(
    primary = AzulFrontOne,
    onPrimary = Color.White,
    primaryContainer = AzulFrontOneContenedor,
    onPrimaryContainer = Color(0xFF1A1F5C),
    background = FondoClaro,
    surface = Color.White
)

private val EsquemaOscuro = darkColorScheme(
    primary = AzulFrontOneOscuro,
    onPrimary = Color(0xFF1A1F5C),
    background = FondoOscuro,
    surface = Color(0xFF1C1C24)
)

@Composable
fun FrontOneTheme(
    darkTheme: Boolean = isSystemInDarkTheme(),
    content: @Composable () -> Unit
) {
    val colorScheme = if (darkTheme) EsquemaOscuro else EsquemaClaro
    MaterialTheme(colorScheme = colorScheme, content = content)
}
