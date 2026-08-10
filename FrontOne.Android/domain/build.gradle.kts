// Módulo domain: Kotlin puro (JVM), sin dependencia de Android ni de infraestructura.
// Equivalente a FrontOne.Domain — entidades, puertos (interfaces) y casos de uso.
// Regla dura: este módulo NUNCA depende de :data ni de :app (inversión de dependencias).
plugins {
    id("org.jetbrains.kotlin.jvm")
}

kotlin {
    jvmToolchain(17)
}

dependencies {
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-core:1.9.0")

    testImplementation("org.jetbrains.kotlin:kotlin-test")
    testImplementation("org.jetbrains.kotlinx:kotlinx-coroutines-test:1.9.0")
}
