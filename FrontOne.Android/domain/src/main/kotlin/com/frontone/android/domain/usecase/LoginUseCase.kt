package com.frontone.android.domain.usecase

import com.frontone.android.domain.crypto.CryptoService
import com.frontone.android.domain.model.ResultadoLogin
import com.frontone.android.domain.model.Usuario
import com.frontone.android.domain.port.UsuarioPort

/**
 * Réplica del flujo de AuthService.LoginAsync (FrontOne.Application, C#), más un
 * paso extra que el escritorio no tiene: verificar el permiso "AccesoMovil" (módulo
 * Seguridad) antes de dar el login por exitoso.
 *
 * 1. Trae el usuario por nombre (sp_Usuario_ObtenerPorNombreUsuario).
 * 2. Rechaza si no existe o está inactivo.
 * 3. Descifra PasswordEncriptado (CryptoService, AES-256) y compara texto plano.
 * 4. NUEVO: trae los permisos del usuario (sp_Usuario_ObtenerPermisos — el mismo SP
 *    que ya usa SessionContext en escritorio) y exige que tenga
 *    Seguridad/AccesoMovil/Consultar. Sin este permiso, la contraseña puede ser
 *    perfectamente correcta y aun así se rechaza — es un gate aparte, no un error de
 *    autenticación. El rol Administrador (y por lo tanto "manager") lo tiene desde
 *    que se sembró `Database/Seguridad/035_Seed_Pantalla_AccesoMovil.sql`; cualquier
 *    otro rol se habilita/deshabilita desde la pantalla Permisos de escritorio, sin
 *    tocar código.
 *
 * Mensaje de error siempre genérico para usuario/password incorrectos (no distingue
 * el motivo, mismo criterio que el escritorio) — pero el rechazo por falta de
 * permiso de acceso móvil SÍ tiene mensaje propio, porque ahí la contraseña ya se
 * validó correctamente y el usuario necesita saber que el problema es otro.
 */
class LoginUseCase(
    private val usuarioPort: UsuarioPort
) {
    suspend operator fun invoke(nombreUsuario: String, password: String): ResultadoLogin {
        val usuario = usuarioPort.obtenerPorNombreUsuario(nombreUsuario.trim())
            ?: return ResultadoLogin.Fallido(MENSAJE_GENERICO)

        if (!usuario.activo) {
            return ResultadoLogin.Fallido(MENSAJE_GENERICO)
        }

        val passwordDescifrado = try {
            CryptoService.decrypt(usuario.passwordEncriptado)
        } catch (ex: Exception) {
            return ResultadoLogin.Fallido(MENSAJE_GENERICO)
        }

        if (passwordDescifrado != password) {
            return ResultadoLogin.Fallido(MENSAJE_GENERICO)
        }

        val permisos = usuarioPort.obtenerPermisos(usuario.id)
        val tieneAccesoMovil = permisos.any {
            it.modulo == MODULO_SEGURIDAD && it.pantalla == PANTALLA_ACCESO_MOVIL && it.accion == ACCION_CONSULTAR
        }
        if (!tieneAccesoMovil) {
            return ResultadoLogin.Fallido(MENSAJE_SIN_ACCESO_MOVIL)
        }

        return ResultadoLogin.Exitoso(
            usuario = Usuario(
                id = usuario.id,
                nombreUsuario = usuario.nombreUsuario,
                nombreCompleto = usuario.nombreCompleto,
                email = usuario.email,
                activo = usuario.activo
            ),
            permisos = permisos
        )
    }

    private companion object {
        const val MENSAJE_GENERICO = "Usuario o contraseña incorrectos"
        const val MENSAJE_SIN_ACCESO_MOVIL = "Tu usuario no tiene permiso para usar la aplicación móvil. Contacta a tu administrador."
        const val MODULO_SEGURIDAD = "Seguridad"
        const val PANTALLA_ACCESO_MOVIL = "AccesoMovil"
        const val ACCION_CONSULTAR = "Consultar"
    }
}
