package com.frontone.android

import android.app.Application
import dagger.hilt.android.HiltAndroidApp
import org.osmdroid.config.Configuration

/**
 * Composition root de la app — equivalente a Program.cs de FrontOne.WinForms.
 * Hilt genera el grafo de dependencias a partir de los módulos declarados en di/.
 */
@HiltAndroidApp
class FrontOneApplication : Application() {
    override fun onCreate() {
        super.onCreate()

        // Configuración global de osmdroid (mapa del submódulo Acopio > Huertas), una
        // sola vez al arrancar la app. El User-Agent es obligatorio por la política de
        // uso de los tile servers públicos de OpenStreetMap — sin uno identificable,
        // rechazan las peticiones (fue justo la causa de que a escritorio le bloquearan
        // OSM con DevExpress.XtraMap, que no permitía configurar este header; osmdroid
        // sí lo permite y lo exige, ver contexto/catalogos.md).
        Configuration.getInstance().load(this, getSharedPreferences("osmdroid", MODE_PRIVATE))
        Configuration.getInstance().userAgentValue = packageName
    }
}
