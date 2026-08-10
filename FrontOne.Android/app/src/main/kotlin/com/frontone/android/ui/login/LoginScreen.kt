package com.frontone.android.ui.login

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Business
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel

/**
 * Pantalla de login — SOLO visual por ahora (el envío de usuario/contraseña no está
 * conectado a Seguridad.Usuario todavía). El logo SÍ es real: se trae en vivo de
 * Configuracion.Empresa vía LoginViewModel/ObtenerLogoEmpresaUseCase.
 *
 * Cuando se conecte la autenticación real, el estado de los campos se mueve de este
 * Composable a un StateFlow del propio LoginViewModel (mismo patrón que ya usa
 * EstadoLogo aquí y EstadoProbarConexion en ui/conexion) — hoy se queda local
 * (remember/mutableStateOf) porque no hay nada todavía que reaccione a esos valores.
 */
@Composable
fun LoginScreen(
    onIniciarSesionClick: (usuario: String, contrasena: String) -> Unit = { _, _ -> },
    viewModel: LoginViewModel = hiltViewModel()
) {
    var usuario by remember { mutableStateOf("") }
    var contrasena by remember { mutableStateOf("") }
    val estadoLogo by viewModel.estadoLogo.collectAsState()

    Scaffold { paddingInterno ->
        Surface(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingInterno),
            color = MaterialTheme.colorScheme.background
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(24.dp),
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                LogoEmpresa(estadoLogo)

                Text(
                    "FrontOne",
                    style = MaterialTheme.typography.headlineMedium,
                    fontWeight = FontWeight.Bold,
                    color = MaterialTheme.colorScheme.primary
                )
                Text(
                    "Inicia sesión con tu usuario de FrontOne",
                    style = MaterialTheme.typography.bodyMedium,
                    color = MaterialTheme.colorScheme.onSurfaceVariant,
                    modifier = Modifier.padding(top = 4.dp, bottom = 32.dp)
                )

                Card(
                    modifier = Modifier.fillMaxWidth(),
                    shape = RoundedCornerShape(20.dp),
                    elevation = CardDefaults.cardElevation(defaultElevation = 2.dp),
                    colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surface)
                ) {
                    Column(modifier = Modifier.padding(24.dp)) {
                        OutlinedTextField(
                            value = usuario,
                            onValueChange = { usuario = it },
                            label = { Text("Usuario") },
                            singleLine = true,
                            shape = RoundedCornerShape(12.dp),
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(bottom = 16.dp)
                        )

                        OutlinedTextField(
                            value = contrasena,
                            onValueChange = { contrasena = it },
                            label = { Text("Contraseña") },
                            singleLine = true,
                            shape = RoundedCornerShape(12.dp),
                            visualTransformation = PasswordVisualTransformation(),
                            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password),
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(bottom = 24.dp)
                        )

                        Button(
                            onClick = { onIniciarSesionClick(usuario, contrasena) },
                            shape = RoundedCornerShape(12.dp),
                            modifier = Modifier
                                .fillMaxWidth()
                                .height(50.dp)
                        ) {
                            Text("Iniciar Sesión", style = MaterialTheme.typography.titleMedium)
                        }
                    }
                }
            }
        }
    }
}

@Composable
private fun LogoEmpresa(estado: EstadoLogo) {
    Box(
        modifier = Modifier
            .size(160.dp)
            .padding(bottom = 16.dp),
        contentAlignment = Alignment.Center
    ) {
        when (estado) {
            is EstadoLogo.Cargando -> CircularProgressIndicator(modifier = Modifier.size(40.dp))

            is EstadoLogo.Disponible -> Image(
                bitmap = estado.imagen,
                contentDescription = "Logo de la empresa",
                modifier = Modifier.fillMaxSize()
            )

            is EstadoLogo.NoDisponible -> Icon(
                imageVector = Icons.Filled.Business,
                contentDescription = null,
                modifier = Modifier.size(96.dp),
                tint = MaterialTheme.colorScheme.primary
            )
        }
    }
}
