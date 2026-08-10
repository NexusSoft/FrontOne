package com.frontone.android.ui.conexion

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel

/**
 * Pantalla piloto del scaffold: confirma que Compose + Hilt + el adaptador de
 * SQL Server están correctamente conectados de punta a punta. Sirve como plantilla
 * de cómo debe verse cualquier pantalla nueva (Screen "tonta" + ViewModel con estado).
 */
@Composable
fun ProbarConexionScreen(viewModel: ProbarConexionViewModel = hiltViewModel()) {
    val estado by viewModel.estado.collectAsState()

    Scaffold { paddingInterno ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingInterno)
                .padding(24.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text("FrontOne — Prueba de Conexión", style = MaterialTheme.typography.titleLarge)

            when (val estadoActual = estado) {
                is EstadoProbarConexion.SinProbar ->
                    Text("Presiona el botón para probar la conexión a SQL Server.")

                is EstadoProbarConexion.Probando ->
                    CircularProgressIndicator()

                is EstadoProbarConexion.Exito ->
                    Text("Conexión exitosa.\nServidor: ${estadoActual.versionServidor}")

                is EstadoProbarConexion.Error ->
                    Text(
                        "Error: ${estadoActual.mensaje}",
                        color = MaterialTheme.colorScheme.error
                    )
            }

            Button(onClick = viewModel::probarConexion) {
                Text("Probar Conexión")
            }
        }
    }
}
