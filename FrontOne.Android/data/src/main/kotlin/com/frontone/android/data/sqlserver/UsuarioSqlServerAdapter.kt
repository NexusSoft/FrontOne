package com.frontone.android.data.sqlserver

import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.UsuarioAutenticacion
import com.frontone.android.domain.port.UsuarioPort

/**
 * Adaptador contra Seguridad.sp_Usuario_ObtenerPorNombreUsuario y
 * Seguridad.sp_Usuario_ObtenerPermisos — los mismos SPs que ya usa FrontOne de
 * escritorio (Database/Seguridad/002_SP_Usuario.sql). Ninguno de los dos compara
 * contraseña ni evalúa permisos por sí mismo; eso pasa en LoginUseCase — este
 * adaptador es I/O puro.
 */
class UsuarioSqlServerAdapter(
    connectionFactory: ConnectionFactory
) : SqlRepositoryBase(connectionFactory), UsuarioPort {

    override suspend fun obtenerPorNombreUsuario(nombreUsuario: String): UsuarioAutenticacion? =
        ejecutarProcedimiento(
            nombreProcedimiento = "Seguridad.sp_Usuario_ObtenerPorNombreUsuario",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setString(1, nombreUsuario) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    if (resultado.next()) {
                        UsuarioAutenticacion(
                            id = resultado.getInt("Id"),
                            nombreUsuario = resultado.getString("NombreUsuario"),
                            nombreCompleto = resultado.getString("NombreCompleto"),
                            email = resultado.getString("Email"),
                            activo = resultado.getBoolean("Activo"),
                            passwordEncriptado = resultado.getString("PasswordEncriptado")
                        )
                    } else {
                        null
                    }
                }
            }
        )

    override suspend fun obtenerPermisos(usuarioId: Int): List<PermisoUsuario> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Seguridad.sp_Usuario_ObtenerPermisos",
            cantidadParametros = 1,
            asignarParametros = { statement -> statement.setInt(1, usuarioId) },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList {
                        while (resultado.next()) {
                            add(
                                PermisoUsuario(
                                    modulo = resultado.getString("Modulo"),
                                    pantalla = resultado.getString("Pantalla"),
                                    accion = resultado.getString("Accion")
                                )
                            )
                        }
                    }
                }
            }
        )
}
