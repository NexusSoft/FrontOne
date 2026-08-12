package com.frontone.android.ui.login

import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.graphics.drawscope.scale
import androidx.compose.ui.unit.dp

/**
 * Íconos decorativos del patrón agrícola de la cabecera del login — traducción
 * directa (mismas coordenadas, mismo viewBox 24×24, mismo stroke-width 1.4) de los
 * `<svg>` originales del diseño importado (`Login FrontOne.dc.html`). Se dibujan con
 * `scale(size/24f)` para poder copiar las coordenadas del SVG tal cual, sin
 * recalcular manualmente cada punto.
 */
private enum class TipoIconoAgricola { AGUACATE, ARBOL, GRANERO, TRACTOR }

private const val VIEWBOX = 24f
private const val GROSOR_TRAZO = 1.4f

@Composable
private fun IconoAgricola(tipo: TipoIconoAgricola, color: Color, modifier: Modifier = Modifier) {
    Canvas(modifier = modifier) {
        scale(scale = size.width / VIEWBOX, pivot = Offset.Zero) {
            when (tipo) {
                TipoIconoAgricola.AGUACATE -> {
                    // <ellipse cx="12" cy="14" rx="6.2" ry="8"> + <circle cx="12" cy="15" r="3.2" fill>
                    drawOval(
                        color = color,
                        topLeft = Offset(12f - 6.2f, 14f - 8f),
                        size = Size(12.4f, 16f),
                        style = Stroke(width = GROSOR_TRAZO)
                    )
                    drawCircle(color = color, radius = 3.2f, center = Offset(12f, 15f))
                }

                TipoIconoAgricola.ARBOL -> {
                    // <path d="M12 3l5 8H7z"/> + <path d="M12 8l5 8H7z"/> + <line x1=12 y1=16 x2=12 y2=21/>
                    val trazo = Stroke(width = GROSOR_TRAZO)
                    drawPath(triangulo(12f, 3f, 17f, 11f, 7f, 11f), color = color, style = trazo)
                    drawPath(triangulo(12f, 8f, 17f, 16f, 7f, 16f), color = color, style = trazo)
                    drawLine(color, Offset(12f, 16f), Offset(12f, 21f), strokeWidth = GROSOR_TRAZO)
                }

                TipoIconoAgricola.GRANERO -> {
                    // <path d="M3 21V10l5 3.5V10l5 3.5V10l5 3.5V8h3v13z"/> + 3 líneas verticales
                    val trazo = Stroke(width = GROSOR_TRAZO)
                    val silueta = androidx.compose.ui.graphics.Path().apply {
                        moveTo(3f, 21f)
                        lineTo(3f, 10f)
                        lineTo(8f, 13.5f)
                        lineTo(8f, 10f)
                        lineTo(13f, 13.5f)
                        lineTo(13f, 10f)
                        lineTo(18f, 13.5f)
                        lineTo(18f, 8f)
                        lineTo(21f, 8f)
                        lineTo(21f, 21f)
                        close()
                    }
                    drawPath(silueta, color = color, style = trazo)
                    drawLine(color, Offset(7f, 17f), Offset(7f, 21f), strokeWidth = GROSOR_TRAZO)
                    drawLine(color, Offset(12f, 17f), Offset(12f, 21f), strokeWidth = GROSOR_TRAZO)
                    drawLine(color, Offset(17f, 17f), Offset(17f, 21f), strokeWidth = GROSOR_TRAZO)
                }

                TipoIconoAgricola.TRACTOR -> {
                    // 2 llantas (circle) + <line> + <path d="M9 17V9h4l3 4h2v4"/>
                    val trazo = Stroke(width = GROSOR_TRAZO)
                    drawCircle(color = color, radius = 2.4f, center = Offset(7f, 17f), style = trazo)
                    drawCircle(color = color, radius = 3.4f, center = Offset(18f, 17f), style = trazo)
                    drawLine(color, Offset(9f, 17f), Offset(15f, 17f), strokeWidth = GROSOR_TRAZO)
                    val cuerpo = androidx.compose.ui.graphics.Path().apply {
                        moveTo(9f, 17f)
                        lineTo(9f, 9f)
                        lineTo(13f, 9f)
                        lineTo(16f, 13f)
                        lineTo(18f, 13f)
                        lineTo(18f, 17f)
                    }
                    drawPath(cuerpo, color = color, style = trazo)
                }
            }
        }
    }
}

private fun triangulo(x1: Float, y1: Float, x2: Float, y2: Float, x3: Float, y3: Float) =
    androidx.compose.ui.graphics.Path().apply {
        moveTo(x1, y1)
        lineTo(x2, y2)
        lineTo(x3, y3)
        close()
    }

/**
 * Círculo relleno con el patrón de íconos agrícolas tileados — equivalente al
 * `sc-for`/`sc-if` del diseño original (49 celdas en un grid 7×7 para el círculo
 * grande, 25 en 5×5 para el chico). El ciclo de íconos es siempre
 * aguacate→árbol→granero→tractor, repetido; [offsetCiclo] corre el punto de partida
 * del ciclo (el círculo chico del diseño original arranca 2 posiciones adelantado).
 */
@Composable
fun PatronAgricolaCircular(
    modifier: Modifier = Modifier,
    columnas: Int,
    filas: Int,
    colorFondo: Color,
    colorIcono: Color,
    tamanoCelda: androidx.compose.ui.unit.Dp = 30.dp,
    offsetCiclo: Int = 0
) {
    Column(
        modifier = modifier
            .clip(CircleShape)
            .background(colorFondo)
    ) {
        repeat(filas) { fila ->
            Row {
                repeat(columnas) { columna ->
                    val indice = (fila * columnas + columna + offsetCiclo) % 4
                    IconoAgricola(
                        tipo = TipoIconoAgricola.entries[indice],
                        color = colorIcono,
                        modifier = Modifier.size(tamanoCelda)
                    )
                }
            }
        }
    }
}
