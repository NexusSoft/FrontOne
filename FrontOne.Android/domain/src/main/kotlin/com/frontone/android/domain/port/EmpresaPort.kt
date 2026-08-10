package com.frontone.android.domain.port

/**
 * Puerto hacia Configuracion.Empresa — tabla singleton (Id=1) que ya usa FrontOne de
 * escritorio para el membrete de reportes (ver CLAUDE.md raíz del repo, excepción
 * documentada de tablas singleton). El dominio no sabe que el logo vive como
 * VARBINARY(MAX) en SQL Server, solo pide "el logo actual" como bytes crudos —
 * decodificarlo a una imagen mostrable es responsabilidad de la capa de UI (:app),
 * que sí puede depender de android.graphics.
 */
interface EmpresaPort {
    suspend fun obtenerLogo(): ByteArray?
}
