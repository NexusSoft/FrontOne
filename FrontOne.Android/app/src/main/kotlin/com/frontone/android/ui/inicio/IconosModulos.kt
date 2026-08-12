package com.frontone.android.ui.inicio

import androidx.compose.foundation.Canvas
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.CornerRadius
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.Path
import androidx.compose.ui.graphics.StrokeCap
import androidx.compose.ui.graphics.StrokeJoin
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.graphics.drawscope.scale

/**
 * Íconos de los 8 módulos de la pantalla de Inicio — traducción directa (mismas
 * coordenadas, mismo viewBox 24×24) de los `<svg>` del diseño importado
 * (`Modulos v3.dc.html`). Mismo patrón que `ui/login/PatronAgricola.kt`: se dibuja
 * con `scale(size/24f)` para copiar las coordenadas del SVG tal cual.
 */
enum class TipoIconoModulo { PALLETS, EMBARQUES, ACOPIO, CAJAS_CAMPO, BASCULA, INOCUIDAD, CALIDAD, REPORTES }

private const val VIEWBOX = 24f

@Composable
fun IconoModulo(tipo: TipoIconoModulo, color: Color, modifier: Modifier = Modifier) {
    Canvas(modifier = modifier) {
        scale(scale = size.width / VIEWBOX, pivot = Offset.Zero) {
            when (tipo) {
                TipoIconoModulo.PALLETS -> {
                    // 4 <rect> 6x6 + 5 líneas (base de tarima)
                    val trazo = Stroke(width = 1.6f)
                    drawRect(color, topLeft = Offset(5f, 4f), size = Size(6f, 6f), style = trazo)
                    drawRect(color, topLeft = Offset(12f, 4f), size = Size(6f, 6f), style = trazo)
                    drawRect(color, topLeft = Offset(5f, 11f), size = Size(6f, 6f), style = trazo)
                    drawRect(color, topLeft = Offset(12f, 11f), size = Size(6f, 6f), style = trazo)
                    drawLine(color, Offset(3f, 19f), Offset(21f, 19f), strokeWidth = 1.6f)
                    drawLine(color, Offset(4f, 21f), Offset(6f, 19f), strokeWidth = 1.6f)
                    drawLine(color, Offset(9f, 21f), Offset(9f, 19f), strokeWidth = 1.6f)
                    drawLine(color, Offset(14f, 21f), Offset(14f, 19f), strokeWidth = 1.6f)
                    drawLine(color, Offset(20f, 21f), Offset(18f, 19f), strokeWidth = 1.6f)
                }

                TipoIconoModulo.EMBARQUES -> {
                    // <rect> caja + <path> cabina camión + 2 llantas + línea
                    val trazo = Stroke(width = 1.8f, cap = StrokeCap.Round, join = StrokeJoin.Round)
                    drawRect(color, topLeft = Offset(1f, 9f), size = Size(13f, 8f), style = trazo)
                    val cabina = Path().apply {
                        moveTo(14f, 12f); lineTo(18f, 12f); lineTo(21f, 15f); lineTo(21f, 17f); lineTo(18f, 17f)
                    }
                    drawPath(cabina, color = color, style = trazo)
                    drawCircle(color, radius = 2f, center = Offset(6f, 19f), style = trazo)
                    drawCircle(color, radius = 2f, center = Offset(17f, 19f), style = trazo)
                    drawLine(color, Offset(8f, 19f), Offset(15f, 19f), strokeWidth = 1.8f, cap = StrokeCap.Round)
                }

                TipoIconoModulo.ACOPIO -> {
                    // silueta de montaña/silo + línea base
                    val trazo = Stroke(width = 1.8f, cap = StrokeCap.Round, join = StrokeJoin.Round)
                    val silueta = Path().apply {
                        moveTo(12f, 3f); lineTo(17f, 11f); lineTo(14f, 11f); lineTo(18f, 17f)
                        lineTo(6f, 17f); lineTo(10f, 11f); lineTo(7f, 11f); close()
                    }
                    drawPath(silueta, color = color, style = trazo)
                    drawLine(color, Offset(12f, 17f), Offset(12f, 21f), strokeWidth = 1.8f, cap = StrokeCap.Round)
                }

                TipoIconoModulo.CAJAS_CAMPO -> {
                    // caja con asa/tapa
                    val trazo = Stroke(width = 1.8f, cap = StrokeCap.Round, join = StrokeJoin.Round)
                    drawRoundRect(
                        color, topLeft = Offset(3f, 7f), size = Size(18f, 13f),
                        cornerRadius = CornerRadius(1f, 1f), style = trazo
                    )
                    val asa = Path().apply {
                        moveTo(8f, 7f); lineTo(8f, 5f)
                        cubicTo(8f, 3.9f, 8.9f, 3f, 10f, 3f)
                        lineTo(14f, 3f)
                        cubicTo(15.1f, 3f, 16f, 3.9f, 16f, 5f)
                        lineTo(16f, 7f)
                    }
                    drawPath(asa, color = color, style = trazo)
                }

                TipoIconoModulo.BASCULA -> {
                    // arco + base + aguja + pivote
                    val trazo = Stroke(width = 1.8f, cap = StrokeCap.Round, join = StrokeJoin.Round)
                    val arco = Path().apply {
                        moveTo(4f, 13f)
                        cubicTo(4f, 8.6f, 7.6f, 5f, 12f, 5f)
                        cubicTo(16.4f, 5f, 20f, 8.6f, 20f, 13f)
                    }
                    drawPath(arco, color = color, style = trazo)
                    drawLine(color, Offset(2f, 20f), Offset(22f, 20f), strokeWidth = 1.8f, cap = StrokeCap.Round)
                    drawLine(color, Offset(12f, 13f), Offset(9f, 8f), strokeWidth = 1.8f, cap = StrokeCap.Round)
                    drawCircle(color, radius = 1f, center = Offset(12f, 13f))
                }

                TipoIconoModulo.INOCUIDAD -> {
                    // escudo + check
                    val trazo = Stroke(width = 1.8f, cap = StrokeCap.Round, join = StrokeJoin.Round)
                    val escudo = Path().apply {
                        moveTo(12f, 2f); lineTo(20f, 6f); lineTo(20f, 12f)
                        cubicTo(20f, 17f, 16.5f, 20f, 12f, 22f)
                        cubicTo(7.5f, 20f, 4f, 17f, 4f, 12f)
                        lineTo(4f, 6f); close()
                    }
                    drawPath(escudo, color = color, style = trazo)
                    val check = Path().apply { moveTo(9f, 12f); lineTo(11f, 14f); lineTo(15f, 10f) }
                    drawPath(check, color = color, style = trazo)
                }

                TipoIconoModulo.CALIDAD -> {
                    // círculo + check
                    val trazo = Stroke(width = 1.8f, cap = StrokeCap.Round, join = StrokeJoin.Round)
                    drawCircle(color, radius = 9f, center = Offset(12f, 12f), style = trazo)
                    val check = Path().apply { moveTo(9f, 12f); lineTo(11f, 14f); lineTo(15f, 10f) }
                    drawPath(check, color = color, style = trazo)
                }

                TipoIconoModulo.REPORTES -> {
                    // documento con esquina doblada + líneas de texto
                    val trazo = Stroke(width = 1.8f, cap = StrokeCap.Round, join = StrokeJoin.Round)
                    val documento = Path().apply {
                        moveTo(4f, 19f); lineTo(4f, 5f)
                        cubicTo(4f, 4.45f, 4.45f, 4f, 5f, 4f)
                        lineTo(13f, 4f); lineTo(19f, 10f); lineTo(19f, 19f)
                        cubicTo(19f, 19.55f, 18.55f, 20f, 18f, 20f)
                        lineTo(5f, 20f)
                        cubicTo(4.45f, 20f, 4f, 19.55f, 4f, 19f); close()
                    }
                    drawPath(documento, color = color, style = trazo)
                    val doblez = Path().apply { moveTo(13f, 4f); lineTo(13f, 9f); lineTo(18f, 9f) }
                    drawPath(doblez, color = color, style = trazo)
                    drawLine(color, Offset(8f, 13f), Offset(14f, 13f), strokeWidth = 1.8f, cap = StrokeCap.Round)
                    drawLine(color, Offset(8f, 17f), Offset(14f, 17f), strokeWidth = 1.8f, cap = StrokeCap.Round)
                }
            }
        }
    }
}
