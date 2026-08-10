package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.EmpresaPort

/**
 * Primer caso de uso de negocio real del proyecto (los anteriores eran solo el piloto
 * de conectividad). Trae el logo configurado en Configuracion.Empresa para mostrarlo
 * en el login — mismo dato que ya usa FrontOne de escritorio en el membrete de reportes.
 */
class ObtenerLogoEmpresaUseCase(
    private val empresaPort: EmpresaPort
) {
    suspend operator fun invoke(): ByteArray? = empresaPort.obtenerLogo()
}
