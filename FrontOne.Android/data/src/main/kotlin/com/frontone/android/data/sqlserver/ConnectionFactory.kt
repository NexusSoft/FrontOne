package com.frontone.android.data.sqlserver

import java.sql.Connection
import java.sql.DriverManager

/**
 * Equivalente Kotlin de FrontOne.Infrastructure.SqlServer/Factories/ConnectionFactory.cs.
 * Punto único que abre conexiones JDBC — ningún otro archivo de :data debe llamar
 * DriverManager directamente.
 *
 * Recibe la configuración como función proveedora, no como valor fijo: la pantalla de
 * Configuración de Conexión (:app) puede guardar credenciales nuevas en cualquier
 * momento (EncryptedSharedPreferences) y la próxima llamada a [abrirConexion] debe
 * verlas sin necesidad de reconstruir el grafo de Hilt ni reiniciar la app.
 */
class ConnectionFactory(
    private val obtenerConfiguracion: () -> ConfiguracionConexion
) {
    init {
        // Registro explícito del driver — en Android el auto-registro por
        // META-INF/services (ServiceLoader) de DriverManager no siempre es confiable.
        Class.forName("net.sourceforge.jtds.jdbc.Driver")
    }

    fun abrirConexion(): Connection {
        val configuracion = obtenerConfiguracion()
        return DriverManager.getConnection(configuracion.aCadenaJdbc(), configuracion.login, configuracion.password)
    }
}
