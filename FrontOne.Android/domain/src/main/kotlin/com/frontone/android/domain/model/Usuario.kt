package com.frontone.android.domain.model

/** Equivalente a UsuarioDto.cs (FrontOne C#) — sin el password, nunca sale del login. */
data class Usuario(
    val id: Int,
    val nombreUsuario: String,
    val nombreCompleto: String,
    val email: String?,
    val activo: Boolean
)

/**
 * Proyección interna usada solo dentro de LoginUseCase — incluye el password
 * cifrado tal como viene de Seguridad.Usuario. Nunca se expone fuera del caso de uso
 * (ResultadoLogin.Exitoso solo entrega [Usuario], sin este campo).
 */
data class UsuarioAutenticacion(
    val id: Int,
    val nombreUsuario: String,
    val nombreCompleto: String,
    val email: String?,
    val activo: Boolean,
    val passwordEncriptado: String
)
