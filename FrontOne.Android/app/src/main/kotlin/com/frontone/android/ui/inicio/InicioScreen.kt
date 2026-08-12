package com.frontone.android.ui.inicio

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Business
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.Usuario
import com.frontone.android.ui.login.EstadoLogo
import kotlinx.coroutines.delay
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.Locale

/**
 * Pantalla de Inicio (post-login) — implementada sobre el diseño real
 * (`Modulos v3.dc.html`, mismo proyecto de Design que el login, traído vía
 * DesignSync). Header con saludo/fecha dinámicos, buscador que filtra la cuadrícula
 * localmente, y la cuadrícula de módulos con sus íconos exactos (ver
 * `IconosModulos.kt`).
 *
 * **Cada tarjeta se muestra u oculta según permiso real**, no es solo decorativo:
 * [permisos] viene de `ResultadoLogin.Exitoso` (ya se trajo durante el login para
 * validar `AccesoMovil`, no se vuelve a consultar la base aquí) y se cruza contra
 * las 8 Pantallas del módulo `Seguridad.AplicacionMovil`
 * (`Database/Seguridad/036_Seed_Modulo_AplicacionMovil.sql`) — si el usuario no
 * tiene "Consultar" en la pantalla con el mismo nombre que la tarjeta, esa tarjeta
 * ni siquiera aparece en la lista filtrable por búsqueda. Se administra desde la
 * pantalla Permisos de escritorio, igual que cualquier otro permiso del proyecto.
 *
 * [onModuloClick] sigue sin navegar a ningún lado real todavía — de los 8 módulos,
 * algunos ya son pantallas reales en escritorio (Pallets, Acopio, Cajas de
 * Campo/Almacenes, Reportes) y otros no (Embarques, Báscula como módulo propio,
 * Inocuidad, Calidad) — el callback queda listo para cuando se construya cada uno,
 * mismo criterio de "visual primero" del resto de esta app. El **permiso** de cada
 * tarjeta ya es real aunque la pantalla detrás todavía no exista.
 */
private data class ModuloInfo(val nombre: String, val color: Color, val tipo: TipoIconoModulo)

private val MODULOS = listOf(
    ModuloInfo("Pallets", Color(0xFF4E6D9C), TipoIconoModulo.PALLETS),
    ModuloInfo("Embarques", Color(0xFF8CC63F), TipoIconoModulo.EMBARQUES),
    ModuloInfo("Acopio", Color(0xFFC98A3F), TipoIconoModulo.ACOPIO),
    ModuloInfo("Cajas de campo", Color(0xFFD1495B), TipoIconoModulo.CAJAS_CAMPO),
    ModuloInfo("Báscula", Color(0xFF3D5A82), TipoIconoModulo.BASCULA),
    ModuloInfo("Inocuidad", Color(0xFF2F9E6E), TipoIconoModulo.INOCUIDAD),
    ModuloInfo("Calidad", Color(0xFFA45FBF), TipoIconoModulo.CALIDAD),
    ModuloInfo("Reportes", Color(0xFFE0A72E), TipoIconoModulo.REPORTES)
)

private val TextoTitulo = Color(0xFF14162A)
private val TextoSecundario = Color(0xFF9A9EB0)
private val FondoPantalla = Color(0xFFF6F6FA)
private val AvatarFondo = Color(0xFFE9EBF2)
private val AvatarTexto = Color(0xFF4E6D9C)
private val BuscadorBorde = Color(0xFFE6E8F0)

private const val MODULO_APLICACION_MOVIL = "AplicacionMovil"
private const val ACCION_CONSULTAR = "Consultar"

@Composable
fun InicioScreen(
    usuario: Usuario,
    estadoLogo: EstadoLogo,
    permisos: List<PermisoUsuario>,
    onModuloClick: (String) -> Unit = {},
    onAvatarClick: () -> Unit = {}
) {
    var busqueda by remember { mutableStateOf("") }

    // Cada tarjeta = una Pantalla del módulo Seguridad.AplicacionMovil (mismo nombre
    // exacto que ModuloInfo.nombre, ver Database/Seguridad/036_Seed_Modulo_
    // AplicacionMovil.sql) — si el usuario no tiene "Consultar" ahí, la tarjeta ni
    // siquiera entra a la lista filtrable por búsqueda.
    val modulosPermitidos = remember(permisos) {
        MODULOS.filter { modulo ->
            permisos.any { it.modulo == MODULO_APLICACION_MOVIL && it.pantalla == modulo.nombre && it.accion == ACCION_CONSULTAR }
        }
    }
    val modulosFiltrados = remember(busqueda, modulosPermitidos) {
        if (busqueda.isBlank()) modulosPermitidos else modulosPermitidos.filter { it.nombre.contains(busqueda, ignoreCase = true) }
    }

    val primerNombre = remember(usuario) { usuario.nombreCompleto.trim().substringBefore(" ") }
    val inicialAvatar = remember(usuario) { usuario.nombreCompleto.trim().firstOrNull()?.uppercaseChar()?.toString() ?: "?" }

    // Antes se calculaba una sola vez con `remember` (sin dependencias) y se quedaba
    // congelada en la hora en la que se abrió la pantalla, sin importar cuánto
    // tiempo pasara — no era un problema de sincronización con el reloj del sistema,
    // era que nada disparaba una nueva composición. Este LaunchedEffect refresca el
    // texto cada minuto mientras la pantalla esté visible.
    val formateador = remember { DateTimeFormatter.ofPattern("EEEE, d 'de' MMMM · HH:mm", Locale("es", "MX")) }
    var fechaTexto by remember { mutableStateOf(LocalDateTime.now().format(formateador).replaceFirstChar { it.uppercase() }) }
    LaunchedEffect(Unit) {
        while (true) {
            delay(60_000)
            fechaTexto = LocalDateTime.now().format(formateador).replaceFirstChar { it.uppercase() }
        }
    }

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
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                LogoEncabezado(estadoLogo)
                Surface(
                    modifier = Modifier.size(38.dp),
                    shape = CircleShape,
                    color = AvatarFondo,
                    onClick = onAvatarClick
                ) {
                    Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
                        Text(inicialAvatar, fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = AvatarTexto)
                    }
                }
            }

            Column(modifier = Modifier.padding(horizontal = 24.dp)) {
                Text("Hola, $primerNombre", fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = TextoTitulo)
                Text(
                    fechaTexto,
                    fontSize = 13.sp,
                    fontWeight = FontWeight.SemiBold,
                    color = TextoSecundario,
                    modifier = Modifier.padding(top = 4.dp)
                )
            }

            // ---------- Buscador ----------
            Row(
                modifier = Modifier
                    .padding(start = 24.dp, end = 24.dp, top = 16.dp)
                    .fillMaxWidth()
                    .height(46.dp)
                    .clip(RoundedCornerShape(14.dp))
                    .background(Color.White)
                    .border(1.5.dp, BuscadorBorde, RoundedCornerShape(14.dp))
                    .padding(horizontal = 14.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(10.dp)
            ) {
                Icon(Icons.Filled.Search, contentDescription = null, tint = TextoSecundario, modifier = Modifier.size(18.dp))
                Box(modifier = Modifier.fillMaxWidth()) {
                    if (busqueda.isEmpty()) {
                        Text("Buscar módulo", fontSize = 14.sp, fontWeight = FontWeight.SemiBold, color = TextoSecundario)
                    }
                    BasicTextField(
                        value = busqueda,
                        onValueChange = { busqueda = it },
                        singleLine = true,
                        textStyle = TextStyle(fontSize = 14.sp, fontWeight = FontWeight.SemiBold, color = TextoTitulo),
                        modifier = Modifier.fillMaxWidth()
                    )
                }
            }

            // ---------- Cuadrícula de módulos ----------
            Text(
                "MÓDULOS",
                fontSize = 12.sp,
                fontWeight = FontWeight.Bold,
                letterSpacing = 0.7.sp,
                color = TextoSecundario,
                modifier = Modifier.padding(start = 24.dp, top = 22.dp, bottom = 14.dp)
            )

            Column(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(start = 24.dp, end = 24.dp, bottom = 24.dp),
                verticalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                modulosFiltrados.chunked(2).forEach { fila ->
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.spacedBy(14.dp)
                    ) {
                        fila.forEach { modulo ->
                            TarjetaModulo(
                                modulo = modulo,
                                onClick = { onModuloClick(modulo.nombre) },
                                modifier = Modifier.weight(1f)
                            )
                        }
                        // Fila impar (número de módulos filtrados no múltiplo de 2):
                        // espacio vacío del mismo ancho para que la última tarjeta no
                        // se estire a todo el ancho.
                        if (fila.size == 1) {
                            Box(modifier = Modifier.weight(1f))
                        }
                    }
                }
            }
        }
    }
}

/** Donde el diseño decía "Fronterra" (texto) va el logo real de Configuracion.Empresa. */
@Composable
private fun LogoEncabezado(estado: EstadoLogo) {
    Box(
        modifier = Modifier
            .height(34.dp)
            .width(150.dp),
        contentAlignment = Alignment.CenterStart
    ) {
        when (estado) {
            is EstadoLogo.Cargando -> CircularProgressIndicator(modifier = Modifier.size(22.dp), strokeWidth = 2.dp, color = AvatarTexto)

            is EstadoLogo.Disponible -> Image(
                bitmap = estado.imagen,
                contentDescription = "Logo de la empresa",
                modifier = Modifier.fillMaxHeight()
            )

            is EstadoLogo.NoDisponible -> Icon(
                imageVector = Icons.Filled.Business,
                contentDescription = null,
                modifier = Modifier.size(28.dp),
                tint = AvatarTexto
            )
        }
    }
}

@Composable
private fun TarjetaModulo(modulo: ModuloInfo, onClick: () -> Unit, modifier: Modifier = Modifier) {
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
                    .background(modulo.color.copy(alpha = 0.1f)),
                contentAlignment = Alignment.Center
            ) {
                IconoModulo(tipo = modulo.tipo, color = modulo.color, modifier = Modifier.size(24.dp))
            }
            Text(modulo.nombre, fontSize = 14.5.sp, fontWeight = FontWeight.ExtraBold, color = TextoTitulo)
        }
    }
}
