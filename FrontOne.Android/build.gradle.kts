// Raíz — solo declara los plugins con sus versiones (resueltos aquí, aplicados en cada módulo).
plugins {
    id("com.android.application") version "8.9.1" apply false
    id("com.android.library") version "8.9.1" apply false
    id("org.jetbrains.kotlin.android") version "2.1.10" apply false
    id("org.jetbrains.kotlin.jvm") version "2.1.10" apply false
    id("org.jetbrains.kotlin.plugin.compose") version "2.1.10" apply false
    id("com.google.dagger.hilt.android") version "2.52" apply false
    id("com.google.devtools.ksp") version "2.1.10-1.0.29" apply false
}
