package com.frontone.android.ui.configuracion

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.frontone.android.config.ConfiguracionConexionStore
import com.frontone.android.data.sqlserver.ConfiguracionConexion
import com.frontone.android.domain.model.ResultadoConexion
import com.frontone.android.domain.usecase.ProbarConexionUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import javax.inject.Inject

sealed interface EstadoPrueba {
    data object SinProbar : EstadoPrueba
    data object Probando : EstadoPrueba
    data class Exito(val versionServidor: String) : EstadoPrueba
    data class Error(val mensaje: String) : EstadoPrueba
}

/**
 * Equivalente móvil de ConfiguracionConexionesForm.cs (FrontOne.WinForms) — captura,
 * prueba y guarda los datos de conexión a SQL Server. El respaldo es
 * ConfiguracionConexionStore (EncryptedSharedPreferences) en vez del
 * RegistryConnectionStore que usa la versión de escritorio.
 *
 * "Probar Conexión" primero guarda los valores capturados y luego corre
 * ProbarConexionUseCase — ConnectionFactory lee la configuración vigente del store en
 * cada llamada (ver DataModule.kt), así que guardar antes de probar garantiza que la
 * prueba use exactamente los valores que el usuario acaba de escribir, no los viejos.
 */
@HiltViewModel
class ConfiguracionConexionViewModel @Inject constructor(
    private val store: ConfiguracionConexionStore,
    private val probarConexionUseCase: ProbarConexionUseCase
) : ViewModel() {

    private val _configuracionActual = MutableStateFlow<ConfiguracionConexion?>(null)
    val configuracionActual: StateFlow<ConfiguracionConexion?> = _configuracionActual.asStateFlow()

    private val _estadoPrueba = MutableStateFlow<EstadoPrueba>(EstadoPrueba.SinProbar)
    val estadoPrueba: StateFlow<EstadoPrueba> = _estadoPrueba.asStateFlow()

    init {
        viewModelScope.launch {
            _configuracionActual.value = withContext(Dispatchers.IO) { store.obtenerActual() }
        }
    }

    fun guardar(configuracion: ConfiguracionConexion) {
        viewModelScope.launch(Dispatchers.IO) {
            store.guardar(configuracion)
        }
    }

    fun guardarYProbar(configuracion: ConfiguracionConexion) {
        _estadoPrueba.value = EstadoPrueba.Probando
        viewModelScope.launch {
            withContext(Dispatchers.IO) { store.guardar(configuracion) }
            _estadoPrueba.value = when (val resultado = probarConexionUseCase()) {
                is ResultadoConexion.Exitosa -> EstadoPrueba.Exito(resultado.versionServidor)
                is ResultadoConexion.Fallida -> EstadoPrueba.Error(resultado.mensaje)
            }
        }
    }
}
