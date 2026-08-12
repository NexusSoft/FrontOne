package com.frontone.android.ui.login

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxScope
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.offset
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Business
import androidx.compose.material.icons.filled.Settings
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.Usuario

/**
 * Pantalla de login — implementada directamente sobre el archivo de diseño real del
 * usuario (`Login FrontOne.dc.html`, proyecto de claude.ai/design, traído vía
 * DesignSync). Reemplaza todos los intentos previos por aproximación visual.
 *
 * Fiel al diseño: gradiente de cabecera exacto, patrón de íconos agrícolas tileados
 * dentro de los dos círculos decorativos (ver PatronAgricola.kt), chip de logo
 * pegado al borde izquierdo, campos con relleno+borde exactos, checkbox custom
 * (cuadrado redondeado con check, no el Checkbox de Material), botón y banner con
 * los colores exactos del diseño.
 *
 * Desviaciones deliberadas frente al diseño (documentadas, no son descuido):
 * 1. Se omite la barra de estado falsa ("12:09" + batería) y la barra inferior tipo
 *    home-indicator — son parte del marco de teléfono del mockup (chrome del SO),
 *    no de la pantalla real de la app; Android ya dibuja su propia barra de estado.
 * 2. Donde el diseño dice "Fronterra" (texto), va el logo real de
 *    Configuracion.Empresa — instrucción explícita del usuario en una iteración
 *    anterior, sigue vigente.
 * 3. Tipografía: el diseño usa la fuente "Manrope" (Google Fonts) — no está
 *    empotrada en el proyecto todavía (requeriría agregar los .ttf como recurso),
 *    así que por ahora se usa la tipografía default del sistema. Si hace falta
 *    exacta, se puede agregar como iteración aparte.
 * 4. Colores de esta pantalla (`#4E6D9C` y derivados) son los del archivo de diseño,
 *    NO el `AzulFrontOne` (`#3949AB`) que ya usan el splash/tema — son azules
 *    distintos. Se mantuvieron separados a propósito para no alterar el diseño que
 *    el usuario aprobó explícitamente; unificar paletas queda pendiente si se pide.
 */
@Composable
fun LoginScreen(
    onLoginExitoso: (Usuario, List<PermisoUsuario>) -> Unit = { _, _ -> },
    onConfiguracionClick: () -> Unit = {},
    viewModel: LoginViewModel = hiltViewModel()
) {
    var usuario by remember { mutableStateOf("") }
    var contrasena by remember { mutableStateOf("") }
    var mostrarPassword by remember { mutableStateOf(false) }
    var recordarme by remember { mutableStateOf(false) }
    val estadoLogo by viewModel.estadoLogo.collectAsState()
    val estadoLogin by viewModel.estadoLogin.collectAsState()
    val credencialesGuardadas by viewModel.credencialesGuardadas.collectAsState()

    // Precarga los campos una sola vez, cuando el store termina de leer (asíncrono,
    // por eso no puede ir en remember{} directo) — solo si el usuario no había
    // empezado a escribir ya, para no pisarle lo que esté tecleando.
    LaunchedEffect(credencialesGuardadas) {
        credencialesGuardadas?.let { credenciales ->
            if (usuario.isEmpty() && contrasena.isEmpty()) {
                usuario = credenciales.usuario
                contrasena = credenciales.password
                recordarme = true
            }
        }
    }

    LaunchedEffect(estadoLogin) {
        (estadoLogin as? EstadoLogin.Exitoso)?.let { onLoginExitoso(it.usuario, it.permisos) }
    }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(FondoFrame)
    ) {
        Column(modifier = Modifier.fillMaxSize()) {

            // ---------- Cabecera ----------
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(300.dp)
                    .background(
                        Brush.linearGradient(
                            colors = listOf(AzulDiseno1, AzulDiseno2, AzulDiseno3)
                        )
                    )
            ) {
                // Círculo grande (verde aguacate), esquina superior derecha
                PatronAgricolaCircular(
                    modifier = Modifier
                        .align(Alignment.TopEnd)
                        .offset(x = 70.dp, y = (-40).dp)
                        .size(210.dp),
                    columnas = 7,
                    filas = 7,
                    colorFondo = VerdeAguacate.copy(alpha = 0.4f),
                    colorIcono = Color.White.copy(alpha = 0.55f),
                    offsetCiclo = 0
                )

                // Círculo chico (translúcido blanco), lado izquierdo
                PatronAgricolaCircular(
                    modifier = Modifier
                        .align(Alignment.TopStart)
                        .offset(x = (-100).dp, y = 165.dp)
                        .size(150.dp),
                    columnas = 5,
                    filas = 5,
                    colorFondo = Color.White.copy(alpha = 0.1f),
                    colorIcono = Color.White.copy(alpha = 0.45f),
                    tamanoCelda = 30.dp,
                    offsetCiclo = 2
                )

                // Puntos decorativos sueltos
                PuntoDecorativo(Alignment.TopEnd, x = (-170).dp, y = 18.dp, tamano = 10.dp, alpha = 0.75f)
                PuntoDecorativo(Alignment.TopEnd, x = (-290).dp, y = 70.dp, tamano = 6.dp, alpha = 0.6f)
                PuntoDecorativo(Alignment.TopEnd, x = (-250).dp, y = 130.dp, tamano = 14.dp, alpha = 0.45f)
                PuntoDecorativo(Alignment.TopEnd, x = (-190).dp, y = 270.dp, tamano = 8.dp, alpha = 0.65f)
                PuntoDecorativo(Alignment.TopStart, x = 60.dp, y = 200.dp, tamano = 7.dp, alpha = 0.55f)
                PuntoDecorativo(Alignment.TopStart, x = 120.dp, y = 100.dp, tamano = 5.dp, alpha = 0.65f)

                // Chip del logo — pegado al borde izquierdo, redondeado solo a la derecha
                Surface(
                    modifier = Modifier
                        .align(Alignment.TopStart)
                        .offset(y = 60.dp),
                    shape = RoundedCornerShape(topStart = 0.dp, bottomStart = 0.dp, topEnd = 14.dp, bottomEnd = 14.dp),
                    color = Color.White
                ) {
                    Box(modifier = Modifier.padding(horizontal = 22.dp, vertical = 14.dp)) {
                        LogoEmpresaChico(estadoLogo)
                    }
                }

                // Título + subtítulo
                Column(
                    modifier = Modifier.padding(start = 30.dp, end = 30.dp, top = 34.dp)
                ) {
                    Spacer(modifier = Modifier.height(96.dp))
                    Text(
                        "Bienvenido a\nFrontOne",
                        fontSize = 32.sp,
                        lineHeight = 35.sp,
                        fontWeight = FontWeight.ExtraBold,
                        color = Color.White
                    )
                    Spacer(modifier = Modifier.height(8.dp))
                    Text(
                        "Inicia sesión con tu usuario para continuar",
                        fontSize = 14.5.sp,
                        lineHeight = 21.sp,
                        color = Color.White.copy(alpha = 0.72f)
                    )
                }
            }

            // ---------- Tarjeta de captura ----------
            Surface(
                modifier = Modifier
                    .fillMaxWidth()
                    .offset(y = (-40).dp)
                    .padding(horizontal = 18.dp),
                shape = RoundedCornerShape(26.dp),
                color = Color.White,
                shadowElevation = 10.dp
            ) {
                Column(
                    modifier = Modifier.padding(start = 22.dp, end = 22.dp, top = 26.dp, bottom = 22.dp),
                    verticalArrangement = Arrangement.spacedBy(16.dp)
                ) {
                    val autenticando = estadoLogin is EstadoLogin.Autenticando

                    CampoFrontOne(
                        etiqueta = "Usuario",
                        valor = usuario,
                        onValorChange = { usuario = it },
                        placeholder = "Usuario",
                        habilitado = !autenticando
                    )

                    CampoFrontOne(
                        etiqueta = "Contraseña",
                        valor = contrasena,
                        onValorChange = { contrasena = it },
                        placeholder = "••••••••",
                        esPassword = true,
                        mostrarPassword = mostrarPassword,
                        onToggleMostrar = { mostrarPassword = !mostrarPassword },
                        habilitado = !autenticando
                    )

                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.SpaceBetween
                    ) {
                        Row(
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.spacedBy(9.dp),
                            modifier = Modifier.clickable { recordarme = !recordarme }
                        ) {
                            CasillaFrontOne(marcado = recordarme)
                            Text("Recordarme", fontSize = 13.5.sp, fontWeight = FontWeight.SemiBold, color = TextoRecordarme)
                        }
                        TextButton(onClick = { /* TODO: recuperación de contraseña, fuera de alcance por ahora */ }) {
                            Text("¿Olvidaste tu clave?", fontSize = 13.5.sp, fontWeight = FontWeight.Bold, color = AzulDiseno1)
                        }
                    }

                    Button(
                        onClick = { viewModel.iniciarSesion(usuario, contrasena, recordarme) },
                        enabled = !autenticando,
                        shape = RoundedCornerShape(15.dp),
                        colors = ButtonDefaults.buttonColors(containerColor = AzulDiseno1),
                        modifier = Modifier
                            .fillMaxWidth()
                            .height(54.dp)
                    ) {
                        if (autenticando) {
                            CircularProgressIndicator(
                                modifier = Modifier.size(22.dp),
                                color = Color.White,
                                strokeWidth = 2.dp
                            )
                        } else {
                            Text("Iniciar sesión", fontSize = 16.sp, fontWeight = FontWeight.Bold, color = Color.White)
                        }
                    }

                    if (estadoLogin is EstadoLogin.Error) {
                        BannerFeedback(
                            texto = (estadoLogin as EstadoLogin.Error).mensaje,
                            colorFondo = BannerErrorFondo,
                            colorPunto = BannerErrorPunto,
                            colorTexto = BannerErrorTexto
                        )
                    }

                    if (estadoLogin is EstadoLogin.Exitoso) {
                        val usuarioAutenticado = (estadoLogin as EstadoLogin.Exitoso).usuario
                        BannerFeedback(
                            texto = "¡Bienvenido de nuevo, ${usuarioAutenticado.nombreCompleto}!",
                            colorFondo = BannerExitoFondo,
                            colorPunto = BannerExitoPunto,
                            colorTexto = BannerExitoTexto
                        )
                    }
                }
            }

            Spacer(modifier = Modifier.weight(1f))

            // ---------- Pie: ícono de ajustes ----------
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 26.dp, vertical = 20.dp),
                horizontalArrangement = Arrangement.End
            ) {
                IconButton(onClick = onConfiguracionClick) {
                    Icon(
                        Icons.Filled.Settings,
                        contentDescription = "Configuración de Conexión",
                        tint = IconoAjustesColor
                    )
                }
            }
        }
    }
}

// ---------- Colores exactos del diseño (Login FrontOne.dc.html) ----------
// Sin "private": LoginSplashScreen.kt (mismo paquete) las reutiliza para que el
// fondo del splash use exactamente el mismo azul que la cabecera/botón del login,
// no una paleta separada.
val AzulDiseno1 = Color(0xFF4E6D9C)
val AzulDiseno2 = Color(0xFF3D5A82)
val AzulDiseno3 = Color(0xFF2F4766)
private val VerdeAguacate = Color(0xFF8CC63F)
private val FondoFrame = Color(0xFFF6F6FA)
private val FondoCampo = Color(0xFFF4F5F9)
private val BordeCampo = Color(0xFFE4E6EF)
private val TextoEtiqueta = Color(0xFF6B6F85)
private val TextoCampo = Color(0xFF1A1C2B)
private val PlaceholderCampo = Color(0xFFA9ADBE)
private val TextoRecordarme = Color(0xFF4C5064)
private val BordeCasilla = Color(0xFFCFD2E0)
private val BannerExitoFondo = Color(0xFFF2F8E9)
private val BannerExitoPunto = Color(0xFF7AB52E)
private val BannerExitoTexto = Color(0xFF43602A)
private val BannerErrorFondo = Color(0xFFFCEBEA)
private val BannerErrorPunto = Color(0xFFD32F2F)
private val BannerErrorTexto = Color(0xFFB3261E)
private val IconoAjustesColor = Color(0xFF7B7F92)

@Composable
private fun BoxScope.PuntoDecorativo(alineacion: Alignment, x: androidx.compose.ui.unit.Dp, y: androidx.compose.ui.unit.Dp, tamano: androidx.compose.ui.unit.Dp, alpha: Float) {
    Box(
        modifier = Modifier
            .align(alineacion)
            .offset(x = x, y = y)
            .size(tamano)
            .clip(CircleShape)
            .background(Color.White.copy(alpha = alpha))
    )
}

/** Campo de captura custom — replica exacta del diseño (relleno + borde, etiqueta afuera, sin Material TextField). */
@Composable
private fun CampoFrontOne(
    etiqueta: String,
    valor: String,
    onValorChange: (String) -> Unit,
    placeholder: String,
    conPunto: Boolean = false,
    esPassword: Boolean = false,
    mostrarPassword: Boolean = false,
    onToggleMostrar: (() -> Unit)? = null,
    habilitado: Boolean = true
) {
    Column(verticalArrangement = Arrangement.spacedBy(7.dp)) {
        Text(
            etiqueta.uppercase(),
            fontSize = 12.sp,
            fontWeight = FontWeight.Bold,
            letterSpacing = 0.6.sp,
            color = TextoEtiqueta
        )
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .height(52.dp)
                .clip(RoundedCornerShape(14.dp))
                .background(FondoCampo)
                .border(1.5.dp, BordeCampo, RoundedCornerShape(14.dp))
                .padding(horizontal = 14.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            if (conPunto) {
                Box(
                    modifier = Modifier
                        .size(8.dp)
                        .clip(CircleShape)
                        .background(VerdeAguacate)
                )
            }

            Box(modifier = Modifier.weight(1f)) {
                if (valor.isEmpty()) {
                    Text(placeholder, fontSize = 16.sp, fontWeight = FontWeight.SemiBold, color = PlaceholderCampo)
                }
                BasicTextField(
                    value = valor,
                    onValueChange = onValorChange,
                    singleLine = true,
                    enabled = habilitado,
                    textStyle = TextStyle(
                        fontSize = 16.sp,
                        fontWeight = FontWeight.SemiBold,
                        color = TextoCampo,
                        letterSpacing = if (esPassword) 1.sp else 0.sp
                    ),
                    visualTransformation = if (esPassword && !mostrarPassword) PasswordVisualTransformation() else VisualTransformation.None,
                    keyboardOptions = if (esPassword) KeyboardOptions(keyboardType = KeyboardType.Password) else KeyboardOptions.Default,
                    modifier = Modifier.fillMaxWidth()
                )
            }

            if (esPassword && onToggleMostrar != null) {
                TextButton(onClick = onToggleMostrar) {
                    Text(
                        if (mostrarPassword) "Ocultar" else "Ver",
                        fontSize = 12.5.sp,
                        fontWeight = FontWeight.Bold,
                        color = AzulDiseno1
                    )
                }
            }
        }
    }
}

/** Checkbox custom (cuadrado redondeado con check) — el diseño no usa el Checkbox nativo de Material. */
@Composable
private fun CasillaFrontOne(marcado: Boolean) {
    Box(
        modifier = Modifier
            .size(20.dp)
            .clip(RoundedCornerShape(6.dp))
            .background(if (marcado) AzulDiseno1 else Color.White)
            .border(1.5.dp, BordeCasilla, RoundedCornerShape(6.dp)),
        contentAlignment = Alignment.Center
    ) {
        if (marcado) {
            Text("✓", fontSize = 12.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
        }
    }
}

@Composable
private fun BannerFeedback(texto: String, colorFondo: Color, colorPunto: Color, colorTexto: Color) {
    Surface(shape = RoundedCornerShape(13.dp), color = colorFondo) {
        Row(
            modifier = Modifier.padding(horizontal = 13.dp, vertical = 11.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(9.dp)
        ) {
            Box(
                modifier = Modifier
                    .size(7.dp)
                    .clip(CircleShape)
                    .background(colorPunto)
            )
            Text(texto, fontSize = 13.sp, fontWeight = FontWeight.SemiBold, color = colorTexto)
        }
    }
}

@Composable
private fun LogoEmpresaChico(estado: EstadoLogo) {
    // Rectangular, no cuadrado: el logo (ícono + texto) es más ancho que alto — una
    // caja cuadrada lo dejaría minúsculo al ajustarse por el lado corto.
    Box(
        modifier = Modifier
            .height(30.dp)
            .width(135.dp),
        contentAlignment = Alignment.CenterStart
    ) {
        when (estado) {
            is EstadoLogo.Cargando -> CircularProgressIndicator(modifier = Modifier.size(20.dp), strokeWidth = 2.dp, color = AzulDiseno1)

            is EstadoLogo.Disponible -> Image(
                bitmap = estado.imagen,
                contentDescription = "Logo de la empresa",
                modifier = Modifier.fillMaxHeight()
            )

            is EstadoLogo.NoDisponible -> Icon(
                imageVector = Icons.Filled.Business,
                contentDescription = null,
                modifier = Modifier.size(26.dp),
                tint = AzulDiseno1
            )
        }
    }
}
