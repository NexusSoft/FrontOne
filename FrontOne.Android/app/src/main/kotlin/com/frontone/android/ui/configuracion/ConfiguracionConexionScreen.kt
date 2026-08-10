package com.frontone.android.ui.configuracion

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.frontone.android.data.sqlserver.ConfiguracionConexion

/**
 * Equivalente móvil de ConfiguracionConexionesForm.cs — captura Servidor/Puerto/Base de
 * datos/Usuario/Contraseña, prueba la conexión y guarda. Se abre desde el ícono de
 * ajustes en el login (ver MainActivity.kt).
 */
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ConfiguracionConexionScreen(
    onVolverClick: () -> Unit,
    viewModel: ConfiguracionConexionViewModel = hiltViewModel()
) {
    val configuracionActual by viewModel.configuracionActual.collectAsState()
    val estadoPrueba by viewModel.estadoPrueba.collectAsState()

    var servidor by remember { mutableStateOf("") }
    var puerto by remember { mutableStateOf("1433") }
    var baseDeDatos by remember { mutableStateOf("") }
    var usuario by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }

    // Precarga los campos una sola vez, cuando el store termina de leer el valor guardado.
    LaunchedEffect(configuracionActual) {
        configuracionActual?.let { configuracion ->
            servidor = configuracion.servidor
            puerto = configuracion.puerto.toString()
            baseDeDatos = configuracion.baseDeDatos
            usuario = configuracion.login
            password = configuracion.password
        }
    }

    fun configuracionDesdeFormulario() = ConfiguracionConexion(
        servidor = servidor,
        puerto = puerto.toIntOrNull() ?: 1433,
        baseDeDatos = baseDeDatos,
        login = usuario,
        password = password
    )

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Configuración de Conexión") },
                navigationIcon = {
                    IconButton(onClick = onVolverClick) {
                        Icon(Icons.Filled.ArrowBack, contentDescription = "Volver")
                    }
                }
            )
        }
    ) { paddingInterno ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingInterno)
                .padding(24.dp)
        ) {
            Text(
                "Datos de conexión al mismo SQL Server que usa FrontOne de escritorio.",
                style = MaterialTheme.typography.bodyMedium,
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                modifier = Modifier.padding(bottom = 20.dp)
            )

            OutlinedTextField(
                value = servidor,
                onValueChange = { servidor = it },
                label = { Text("Servidor") },
                singleLine = true,
                modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
            )

            OutlinedTextField(
                value = puerto,
                onValueChange = { puerto = it },
                label = { Text("Puerto") },
                singleLine = true,
                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
            )

            OutlinedTextField(
                value = baseDeDatos,
                onValueChange = { baseDeDatos = it },
                label = { Text("Base de datos") },
                singleLine = true,
                modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
            )

            OutlinedTextField(
                value = usuario,
                onValueChange = { usuario = it },
                label = { Text("Usuario") },
                singleLine = true,
                modifier = Modifier.fillMaxWidth().padding(bottom = 12.dp)
            )

            OutlinedTextField(
                value = password,
                onValueChange = { password = it },
                label = { Text("Contraseña") },
                singleLine = true,
                visualTransformation = PasswordVisualTransformation(),
                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password),
                modifier = Modifier.fillMaxWidth().padding(bottom = 20.dp)
            )

            when (val estado = estadoPrueba) {
                is EstadoPrueba.SinProbar -> {}
                is EstadoPrueba.Probando -> Row(
                    verticalAlignment = androidx.compose.ui.Alignment.CenterVertically,
                    modifier = Modifier.padding(bottom = 12.dp)
                ) {
                    CircularProgressIndicator(modifier = Modifier.padding(end = 12.dp))
                    Text("Probando conexión...")
                }
                is EstadoPrueba.Exito -> Text(
                    "Conexión exitosa.\nServidor: ${estado.versionServidor}",
                    color = MaterialTheme.colorScheme.primary,
                    modifier = Modifier.padding(bottom = 12.dp)
                )
                is EstadoPrueba.Error -> Text(
                    "Error: ${estado.mensaje}",
                    color = MaterialTheme.colorScheme.error,
                    modifier = Modifier.padding(bottom = 12.dp)
                )
            }

            Row(modifier = Modifier.fillMaxWidth()) {
                OutlinedButton(
                    onClick = { viewModel.guardar(configuracionDesdeFormulario()) },
                    modifier = Modifier.weight(1f).padding(end = 8.dp)
                ) {
                    Text("Guardar")
                }
                Button(
                    onClick = { viewModel.guardarYProbar(configuracionDesdeFormulario()) },
                    modifier = Modifier.weight(1f)
                ) {
                    Text("Probar Conexión")
                }
            }
        }
    }
}
