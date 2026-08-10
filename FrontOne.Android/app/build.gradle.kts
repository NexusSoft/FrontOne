// Módulo app: adaptador primario (hexagonal) — UI Compose + composition root (Hilt).
// Equivalente a FrontOne.WinForms. Regla dura: este módulo nunca llama JDBC directo,
// solo consume casos de uso de :domain a través de la inyección de :data.
import java.util.Properties

plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("org.jetbrains.kotlin.plugin.compose")
    id("com.google.dagger.hilt.android")
    id("com.google.devtools.ksp")
}

// Credenciales de conexión SQL Server para desarrollo local — nunca hardcodeadas ni
// versionadas. Lee secrets.properties (gitignored, ver .gitignore) y las expone como
// BuildConfig.* de solo lectura. Ver CLAUDE.md, sección "Credenciales de conexión":
// esto es un atajo válido SOLO para desarrollo/pruebas — antes de producción, mover
// a EncryptedSharedPreferences capturado desde una pantalla de configuración.
val secretsFile = rootProject.file("secrets.properties")
val secrets = Properties().apply {
    if (secretsFile.exists()) secretsFile.inputStream().use { load(it) }
}
fun secret(key: String, default: String) = secrets.getProperty(key, default)

android {
    namespace = "com.frontone.android"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.frontone.android"
        minSdk = 26
        targetSdk = 35
        versionCode = 1
        versionName = "0.1.0-scaffold"

        buildConfigField("String", "SQL_SERVER_HOST", "\"${secret("sqlServer.host", "")}\"")
        buildConfigField("String", "SQL_SERVER_DATABASE", "\"${secret("sqlServer.database", "FrontOne")}\"")
        buildConfigField("String", "SQL_SERVER_USER", "\"${secret("sqlServer.user", "")}\"")
        buildConfigField("String", "SQL_SERVER_PASSWORD", "\"${secret("sqlServer.password", "")}\"")
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android-optimize.txt"), "proguard-rules.pro")
        }
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }

    kotlinOptions {
        jvmTarget = "17"
    }

    buildFeatures {
        compose = true
        buildConfig = true
    }

    // Exclusiones defensivas de META-INF — driver de datos previo (mssql-jdbc) las
    // necesitaba para no chocar con otras dependencias al empaquetar el APK. Se dejan
    // aunque hoy se usa jTDS (no las necesita) por si se vuelve a agregar un driver
    // similar más adelante.
    packaging {
        resources {
            excludes += setOf(
                "META-INF/INDEX.LIST",
                "META-INF/*.SF",
                "META-INF/*.DSA",
                "META-INF/*.RSA",
                "META-INF/LICENSE*",
                "META-INF/NOTICE*"
            )
        }
    }
}

dependencies {
    implementation(project(":domain"))
    implementation(project(":data"))

    val composeBom = platform("androidx.compose:compose-bom:2026.06.01")
    implementation(composeBom)
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-graphics")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    // "extended" (no "core") a propósito: Business/Settings que usan LoginScreen y
    // ConfiguracionConexionScreen no están en el subconjunto chico de material-icons-core.
    implementation("androidx.compose.material:material-icons-extended")
    implementation("androidx.activity:activity-compose:1.9.3")
    implementation("androidx.lifecycle:lifecycle-viewmodel-compose:2.8.7")
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.8.7")
    implementation("androidx.core:core-ktx:1.15.0")

    implementation("com.google.dagger:hilt-android:2.52")
    ksp("com.google.dagger:hilt-compiler:2.52")
    implementation("androidx.hilt:hilt-navigation-compose:1.2.0")

    // Almacenamiento cifrado de la configuración de conexión editable desde la app
    // (pantalla de Configuración de Conexión) — respaldado por Android Keystore.
    implementation("androidx.security:security-crypto:1.1.0-alpha06")

    debugImplementation("androidx.compose.ui:ui-tooling")
}
