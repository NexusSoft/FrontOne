package com.frontone.android.config

import android.content.Context
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import dagger.hilt.android.qualifiers.ApplicationContext
import javax.inject.Inject
import javax.inject.Singleton

/**
 * Guarda/lee usuario+contraseña cuando el usuario marca "Recordarme" en el login —
 * mismo mecanismo que ConfiguracionConexionStore (EncryptedSharedPreferences,
 * respaldado por Android Keystore, archivo separado del de conexión SQL Server).
 *
 * Decisión de seguridad confirmada con el usuario: si el dispositivo se pierde
 * desbloqueado, quien lo tenga puede iniciar sesión sin saber la contraseña real
 * (queda precargada) — se aceptó ese trade-off a cambio de comodidad. Igual que con
 * la clave AES de CryptoService, el respaldo de cifrado en reposo (Keystore) protege
 * contra extraer el archivo de preferencias fuera del dispositivo, no contra un
 * dispositivo desbloqueado en manos de otra persona.
 */
data class CredencialesGuardadas(val usuario: String, val password: String)

@Singleton
class CredencialesRecordadasStore @Inject constructor(
    @ApplicationContext context: Context
) {
    private val preferencias = EncryptedSharedPreferences.create(
        context,
        NOMBRE_ARCHIVO,
        MasterKey.Builder(context).setKeyScheme(MasterKey.KeyScheme.AES256_GCM).build(),
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )

    fun obtener(): CredencialesGuardadas? {
        val usuario = preferencias.getString(CLAVE_USUARIO, null) ?: return null
        val password = preferencias.getString(CLAVE_PASSWORD, null) ?: return null
        return CredencialesGuardadas(usuario, password)
    }

    fun guardar(usuario: String, password: String) {
        preferencias.edit()
            .putString(CLAVE_USUARIO, usuario)
            .putString(CLAVE_PASSWORD, password)
            .apply()
    }

    fun limpiar() {
        preferencias.edit().clear().apply()
    }

    private companion object {
        const val NOMBRE_ARCHIVO = "frontone_credenciales_recordadas"
        const val CLAVE_USUARIO = "usuario"
        const val CLAVE_PASSWORD = "password"
    }
}
