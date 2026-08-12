package com.frontone.android.domain.port

import com.frontone.android.domain.model.PermisoUsuario
import com.frontone.android.domain.model.UsuarioAutenticacion

/** Equivalente a IUsuarioRepository (FrontOne C#) — ObtenerPorNombreUsuarioAsync + ObtenerPermisosAsync. */
interface UsuarioPort {
    suspend fun obtenerPorNombreUsuario(nombreUsuario: String): UsuarioAutenticacion?
    suspend fun obtenerPermisos(usuarioId: Int): List<PermisoUsuario>
}
