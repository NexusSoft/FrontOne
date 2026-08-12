package com.frontone.android.domain.model

sealed interface ResultadoLogin {
    /**
     * [permisos] va incluido aquí (no solo el `Usuario`) porque LoginUseCase ya los
     * trae para validar el permiso AccesoMovil — evita una segunda consulta a
     * sp_Usuario_ObtenerPermisos solo para que InicioScreen filtre qué tarjetas de
     * módulo mostrar.
     */
    data class Exitoso(val usuario: Usuario, val permisos: List<PermisoUsuario>) : ResultadoLogin
    data class Fallido(val mensaje: String) : ResultadoLogin
}
