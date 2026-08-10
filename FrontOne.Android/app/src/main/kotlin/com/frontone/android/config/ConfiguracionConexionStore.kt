package com.frontone.android.config

import android.content.Context
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import com.frontone.android.BuildConfig
import com.frontone.android.data.sqlserver.ConfiguracionConexion
import dagger.hilt.android.qualifiers.ApplicationContext
import javax.inject.Inject
import javax.inject.Singleton

/**
 * Guarda/lee la configuración de conexión a SQL Server capturada desde la pantalla de
 * Configuración (equivalente móvil de ConfiguracionConexionesForm.cs de FrontOne
 * WinForms, que usa RegistryConnectionStore — aquí el respaldo es Android Keystore vía
 * EncryptedSharedPreferences en vez del Registro de Windows).
 *
 * Mientras el usuario no haya guardado nada todavía (primera vez que se abre la app),
 * cae de vuelta a los valores de BuildConfig (secrets.properties) — así el piloto de
 * desarrollo sigue funcionando sin configurar nada a mano.
 *
 * Vive en :app (no en :data ni :domain) porque EncryptedSharedPreferences necesita
 * Context de Android — la regla hexagonal del proyecto (ver CLAUDE.md) es que ni el
 * dominio ni el adaptador de datos pueden depender del framework de Android.
 */
@Singleton
class ConfiguracionConexionStore @Inject constructor(
    @ApplicationContext context: Context
) {
    private val preferencias = EncryptedSharedPreferences.create(
        context,
        NOMBRE_ARCHIVO,
        MasterKey.Builder(context).setKeyScheme(MasterKey.KeyScheme.AES256_GCM).build(),
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )

    fun obtenerActual(): ConfiguracionConexion = ConfiguracionConexion(
        servidor = preferencias.getString(CLAVE_SERVIDOR, null) ?: BuildConfig.SQL_SERVER_HOST,
        puerto = preferencias.getInt(CLAVE_PUERTO, 1433),
        baseDeDatos = preferencias.getString(CLAVE_BASE_DATOS, null) ?: BuildConfig.SQL_SERVER_DATABASE,
        login = preferencias.getString(CLAVE_USUARIO, null) ?: BuildConfig.SQL_SERVER_USER,
        password = preferencias.getString(CLAVE_PASSWORD, null) ?: BuildConfig.SQL_SERVER_PASSWORD
    )

    fun guardar(configuracion: ConfiguracionConexion) {
        preferencias.edit()
            .putString(CLAVE_SERVIDOR, configuracion.servidor)
            .putInt(CLAVE_PUERTO, configuracion.puerto)
            .putString(CLAVE_BASE_DATOS, configuracion.baseDeDatos)
            .putString(CLAVE_USUARIO, configuracion.login)
            .putString(CLAVE_PASSWORD, configuracion.password)
            .apply()
    }

    private companion object {
        const val NOMBRE_ARCHIVO = "frontone_conexion_sqlserver"
        const val CLAVE_SERVIDOR = "servidor"
        const val CLAVE_PUERTO = "puerto"
        const val CLAVE_BASE_DATOS = "baseDeDatos"
        const val CLAVE_USUARIO = "usuario"
        const val CLAVE_PASSWORD = "password"
    }
}
