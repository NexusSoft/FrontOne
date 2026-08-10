package com.frontone.android

import android.app.Application
import dagger.hilt.android.HiltAndroidApp

/**
 * Composition root de la app — equivalente a Program.cs de FrontOne.WinForms.
 * Hilt genera el grafo de dependencias a partir de los módulos declarados en di/.
 */
@HiltAndroidApp
class FrontOneApplication : Application()
