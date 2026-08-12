package com.frontone.android.domain.usecase

import com.frontone.android.domain.port.PalletPort

/** Bloqueo irreversible — el SP ya valida Completo/no-vacío, este caso de uso solo llama al puerto. */
class BloquearPalletUseCase(private val palletPort: PalletPort) {
    suspend operator fun invoke(id: Int) = palletPort.bloquear(id)
}
