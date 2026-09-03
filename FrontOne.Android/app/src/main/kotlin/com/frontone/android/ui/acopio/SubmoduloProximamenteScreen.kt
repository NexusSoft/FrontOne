package com.frontone.android.ui.acopio

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material3.Icon
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

private val TextoTitulo = Color(0xFF14162A)
private val TextoSecundario = Color(0xFF9A9EB0)
private val FondoPantalla = Color(0xFFF6F6FA)
private val AvatarFondo = Color(0xFFE9EBF2)

/**
 * Pantalla de relleno para un submódulo de Acopio que todavía no tiene captura
 * real (Huertas/Calculadora/Acuerdos de Corte/Incidencias) — mismo criterio de
 * "visual primero" ya usado en el resto de la app para módulos sin pantalla
 * detrás. Cuando cada uno se construya de verdad, se reemplaza por su propio
 * Screen+ViewModel (ver patrón de `ui/pallets/`) y este composable deja de
 * usarse para ese submódulo.
 */
@Composable
fun SubmoduloProximamenteScreen(nombre: String, color: Color, icono: ImageVector, onVolverClick: () -> Unit) {
    Surface(modifier = Modifier.fillMaxSize(), color = FondoPantalla) {
        Column(modifier = Modifier.fillMaxSize()) {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(start = 24.dp, end = 24.dp, top = 48.dp, bottom = 24.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Surface(
                    modifier = Modifier.size(38.dp),
                    shape = CircleShape,
                    color = AvatarFondo,
                    onClick = onVolverClick
                ) {
                    Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                        Icon(Icons.Filled.ArrowBack, contentDescription = "Volver", tint = TextoTitulo, modifier = Modifier.size(18.dp))
                    }
                }
                Text(nombre, fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = TextoTitulo)
            }

            Column(
                modifier = Modifier.fillMaxSize().padding(24.dp),
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                Box(
                    modifier = Modifier
                        .size(72.dp)
                        .clip(RoundedCornerShape(20.dp))
                        .background(color.copy(alpha = 0.1f)),
                    contentAlignment = Alignment.Center
                ) {
                    Icon(icono, contentDescription = null, tint = color, modifier = Modifier.size(36.dp))
                }
                Text(
                    "Próximamente",
                    fontSize = 18.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color = TextoTitulo,
                    modifier = Modifier.padding(top = 20.dp)
                )
                Text(
                    "El módulo \"$nombre\" todavía no está disponible en la app.",
                    fontSize = 13.sp,
                    fontWeight = FontWeight.SemiBold,
                    color = TextoSecundario,
                    modifier = Modifier.padding(top = 6.dp)
                )
            }
        }
    }
}
