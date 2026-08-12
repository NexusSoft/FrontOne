package com.frontone.android.domain.model

/**
 * Proyección de una fila de Seguridad.sp_Usuario_ObtenerPermisos (el mismo SP que ya
 * usa FrontOne de escritorio para armar SessionContext) — Módulo/Pantalla/Acción por
 * nombre, agregados vía Rol → Permiso.
 */
data class PermisoUsuario(
    val modulo: String,
    val pantalla: String,
    val accion: String
)
