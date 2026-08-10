package com.frontone.android.data.sqlserver

/**
 * Datos de conexión al mismo SQL Server que usa FrontOne de escritorio.
 *
 * IMPORTANTE (ver CLAUDE.md, "Credenciales de conexión"): en este scaffold la
 * configuración llega desde BuildConfig (a su vez leído de secrets.properties,
 * gitignored) solo para poder probar la conectividad. Antes de capturar credenciales
 * de un servidor con datos reales de producción, esto debe leerse desde
 * EncryptedSharedPreferences (Jetpack Security) — nunca hardcodeado en código.
 *
 * `login` debe ser un usuario SQL dedicado con permisos acotados (GRANT EXECUTE
 * solo sobre los SPs que Android consume) — nunca "sa", regla dura del proyecto
 * (pendiente de crearlo; el scaffold usa "sa" solo para la primera prueba de humo).
 *
 * Driver: jTDS, no mssql-jdbc — ver el comentario en data/build.gradle.kts y
 * contexto/arquitectura.md para el porqué (mssql-jdbc no funciona en runtime Android).
 * jTDS no implementa TLS de la misma forma que el driver oficial, así que [usarSsl]
 * por defecto es `false` — razonable en la red local/VPN cerrada que ya se acordó
 * como supuesto de este proyecto; si algún día se necesita cifrado en tránsito,
 * cambiar a `ssl=require` (jTDS sí lo soporta, sin el bug de mssql-jdbc en Android).
 */
data class ConfiguracionConexion(
    val servidor: String,
    val baseDeDatos: String = "FrontOne",
    val login: String,
    val password: String,
    val puerto: Int = 1433,
    val usarSsl: Boolean = false
) {
    fun aCadenaJdbc(): String = buildString {
        append("jdbc:jtds:sqlserver://$servidor:$puerto/$baseDeDatos;")
        append("ssl=${if (usarSsl) "require" else "off"};")
        append("loginTimeout=10;")
    }
}
