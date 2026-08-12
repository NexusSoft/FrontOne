package com.frontone.android.ui.pallets

import androidx.compose.animation.Crossfade
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import com.frontone.android.domain.model.PermisoUsuario

private sealed interface RutaPallets {
    data object Lista : RutaPallets

    // [instancia] distingue cada visita a Captura entre sí (incluidas dos altas "Nuevo"
    // seguidas, ambas con palletId null) — sin esto, hiltViewModel() reutilizaría la misma
    // instancia de PalletCapturaViewModel entre visitas (vive en el ViewModelStore de la
    // Activity, no se destruye solo porque Crossfade quite el composable de pantalla) y el
    // formulario de la visita anterior (ya con `inicializado = true`) se quedaría pegado.
    data class Captura(val palletId: Int?, val instancia: Long = System.nanoTime()) : RutaPallets
}

/**
 * Sub-navegación Lista↔Captura del módulo Pallets, encapsulada aquí — MainActivity solo conoce
 * un único destino "Pallets" (mismo criterio ya documentado en el KDoc de MainActivity.kt: sin
 * Navigation Compose todavía, un `sealed interface` + `remember` + `Crossfade` por nivel).
 */
@Composable
fun PalletsHostScreen(permisos: List<PermisoUsuario>, onVolverAInicioClick: () -> Unit) {
    var ruta by remember { mutableStateOf<RutaPallets>(RutaPallets.Lista) }

    Crossfade(targetState = ruta, label = "navegacionPallets") { rutaActual ->
        when (rutaActual) {
            is RutaPallets.Lista -> PalletsListaScreen(
                permisos = permisos,
                onVolverClick = onVolverAInicioClick,
                onPalletClick = { palletId -> ruta = RutaPallets.Captura(palletId) },
                onNuevoClick = { ruta = RutaPallets.Captura(null) }
            )

            is RutaPallets.Captura -> PalletCapturaScreen(
                palletIdInicial = rutaActual.palletId,
                claveViewModel = "pallet-captura-${rutaActual.palletId}-${rutaActual.instancia}",
                permisos = permisos,
                onVolverClick = { ruta = RutaPallets.Lista },
                onPalletEliminado = { ruta = RutaPallets.Lista }
            )
        }
    }
}
