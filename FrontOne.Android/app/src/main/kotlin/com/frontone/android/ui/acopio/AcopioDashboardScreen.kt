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
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Agriculture
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Calculate
import androidx.compose.material.icons.filled.Handshake
import androidx.compose.material.icons.filled.ReportProblem
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

/**
 * Dashboard del módulo Acopio — mismo patrón visual y de cuadrícula que
 * `InicioScreen.kt` (cuadrícula de tarjetas en filas de 2, `TarjetaSubmodulo`
 * análoga a `TarjetaModulo`), pero un nivel más adentro: se llega aquí desde la
 * tarjeta "Acopio" de Inicio (`AcopioHostScreen`).
 *
 * Los 4 submódulos (Huertas, Calculadora, Acuerdos de Corte, Incidencias) todavía
 * no tienen pantalla real — se muestran todos sin filtro de permiso individual
 * (el permiso de nivel tarjeta `AplicacionMovil/Acopio/Consultar` ya se validó en
 * Inicio para poder entrar aquí; ver `PantallasMovilDisponibles.cs` en el repo
 * raíz — todavía no hay códigos de permiso finos por submódulo, se agregan cuando
 * cada uno tenga su propia pantalla real).
 */
private data class SubmoduloAcopioInfo(val nombre: String, val color: Color, val icono: ImageVector)

private val SUBMODULOS_ACOPIO = listOf(
    SubmoduloAcopioInfo("Huertas", Color(0xFF2F9E6E), Icons.Filled.Agriculture),
    SubmoduloAcopioInfo("Calculadora", Color(0xFF4E6D9C), Icons.Filled.Calculate),
    SubmoduloAcopioInfo("Acuerdos de Corte", Color(0xFFC98A3F), Icons.Filled.Handshake),
    SubmoduloAcopioInfo("Incidencias", Color(0xFFD1495B), Icons.Filled.ReportProblem)
)

private val TextoTitulo = Color(0xFF14162A)
private val TextoSecundario = Color(0xFF9A9EB0)
private val FondoPantalla = Color(0xFFF6F6FA)
private val AvatarFondo = Color(0xFFE9EBF2)

@Composable
fun AcopioDashboardScreen(onVolverClick: () -> Unit, onSubmoduloClick: (String) -> Unit) {
    Surface(modifier = Modifier.fillMaxSize(), color = FondoPantalla) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .verticalScroll(rememberScrollState())
        ) {
            // ---------- Encabezado ----------
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
                Text("Acopio", fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = TextoTitulo)
            }

            Text(
                "SUBMÓDULOS",
                fontSize = 12.sp,
                fontWeight = FontWeight.Bold,
                letterSpacing = 0.7.sp,
                color = TextoSecundario,
                modifier = Modifier.padding(start = 24.dp, bottom = 14.dp)
            )

            Column(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(start = 24.dp, end = 24.dp, bottom = 24.dp),
                verticalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                SUBMODULOS_ACOPIO.chunked(2).forEach { fila ->
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.spacedBy(14.dp)
                    ) {
                        fila.forEach { submodulo ->
                            TarjetaSubmodulo(
                                submodulo = submodulo,
                                onClick = { onSubmoduloClick(submodulo.nombre) },
                                modifier = Modifier.weight(1f)
                            )
                        }
                        if (fila.size == 1) {
                            Box(modifier = Modifier.weight(1f))
                        }
                    }
                }
            }
        }
    }
}

@Composable
private fun TarjetaSubmodulo(submodulo: SubmoduloAcopioInfo, onClick: () -> Unit, modifier: Modifier = Modifier) {
    Surface(
        modifier = modifier,
        shape = RoundedCornerShape(20.dp),
        color = Color.White,
        shadowElevation = 4.dp,
        onClick = onClick
    ) {
        Column(
            modifier = Modifier.padding(horizontal = 16.dp, vertical = 18.dp),
            verticalArrangement = Arrangement.spacedBy(20.dp)
        ) {
            Box(
                modifier = Modifier
                    .size(44.dp)
                    .clip(RoundedCornerShape(12.dp))
                    .background(submodulo.color.copy(alpha = 0.1f)),
                contentAlignment = Alignment.Center
            ) {
                Icon(submodulo.icono, contentDescription = null, tint = submodulo.color, modifier = Modifier.size(24.dp))
            }
            Text(submodulo.nombre, fontSize = 14.5.sp, fontWeight = FontWeight.ExtraBold, color = TextoTitulo)
        }
    }
}
