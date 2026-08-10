// Módulo data: adaptador secundario (hexagonal) contra SQL Server.
// Equivalente a FrontOne.Infrastructure.SqlServer — implementa los puertos de :domain
// usando JDBC + Stored Procedures. Regla dura: nunca SQL crudo, nunca una API intermedia.
plugins {
    id("org.jetbrains.kotlin.jvm")
}

kotlin {
    jvmToolchain(17)
}

dependencies {
    implementation(project(":domain"))
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-core:1.9.0")

    // jTDS, no el driver oficial de Microsoft. Se probó mssql-jdbc primero (mismo
    // fabricante que SQL Server) pero revienta en runtime Android con
    // "AssertionError: numMsgsRcvd should be less than numMsgsSent" durante el
    // handshake TLS — incompatibilidad conocida entre el manejo interno de SSL de
    // mssql-jdbc y Conscrypt (el proveedor SSL de Android), no soportado oficialmente
    // en esa plataforma. Ver contexto/arquitectura.md para el detalle completo.
    implementation("net.sourceforge.jtds:jtds:1.3.1")

    testImplementation("org.jetbrains.kotlin:kotlin-test")
    testImplementation("org.jetbrains.kotlinx:kotlinx-coroutines-test:1.9.0")
}
