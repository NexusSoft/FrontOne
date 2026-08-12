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

/**
 * Consulta puntual de un permiso Módulo/Pantalla/Acción — usada para gatear botones de
 * Crear/Modificar/Eliminar dentro de un módulo móvil ya visible (a diferencia del filtro de
 * tarjetas de `InicioScreen`, que solo revisa "Consultar" para decidir si la tarjeta aparece).
 * Las 4 acciones estándar (Consultar/Crear/Modificar/Eliminar) ya existen para las 9 pantallas
 * móviles desde que se sembraron (`Accion` es una tabla global, `sp_Permiso_ObtenerEstructura` la
 * cruza contra toda Pantalla) — se administran desde la pantalla de escritorio "Permisos de
 * Aplicación Móvil" (`PermisosAplicacionMovilForm.cs`), separada de la matriz general de Permisos.
 */
fun List<PermisoUsuario>.tienePermiso(pantalla: String, accion: String, modulo: String = "AplicacionMovil"): Boolean =
    any { it.modulo == modulo && it.pantalla == pantalla && it.accion == accion }
