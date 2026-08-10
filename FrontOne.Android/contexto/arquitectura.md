# Arquitectura, decisiones y estado — FrontOne.Android

> Parte de la memoria viva del sub-proyecto — ver [contexto.md](../contexto.md) para el índice. Reglas duras viven en [CLAUDE.md](../CLAUDE.md).

## Decisiones de stack (2026-08-07, sesión de arranque)

Tras una ronda de lluvia de ideas comparando Java/Kotlin/Kotlin Multiplatform/Flutter y MVVM-simple/Clean-Architecture/Hexagonal, se confirmó:

- **Lenguaje/UI**: Kotlin + Jetpack Compose. Se descartó Flutter por incompatibilidad directa con la regla dura de conexión a SQL Server (no existe driver JDBC/ADO maduro para Dart, forzaría una API intermedia). Se descartó Kotlin Multiplatform por sobre-ingeniería — no hay hoy un segundo cliente nativo (iOS) planeado que justifique compartir código; si aparece en el futuro, la capa `:domain`/`:data` en Kotlin puro ya es compatible con migrar a KMP sin reescribir.
- **Arquitectura**: Hexagonal ligera (puertos/adaptadores), espejo de las capas que ya tiene FrontOne en C# (`Domain`/`Application`/`Infrastructure.SqlServer`/`WinForms`). Se descartó DDD táctico completo (Value Objects, Aggregate Roots, CQRS) por ser desproporcionado para una app que arranca como extensión ágil.
- **Datos**: conexión directa a SQL Server vía `mssql-jdbc` (driver oficial de Microsoft), mismo servidor que usa FrontOne de escritorio, todo por Stored Procedures. Se descartó jTDS (proyecto abandonado) y R2DBC (soporte de SQL Server inmaduro).
- **Alcance**: catálogos/permisos/roles/módulos ya construidos se quedan exclusivamente en escritorio. Android es una extensión para funcionalidad nueva que eventualmente migra a WinForms — la lógica de negocio real debe vivir donde sea fácil de portar (capa `usecase`/SPs), no enterrada en el ViewModel.
- **Red**: dispositivos siempre en red local/VPN de la empresa (confirmado con el usuario) — por eso se decidió *online-only*, sin caché/sincronización offline por ahora.
- **Ubicación**: `FrontOne.Android/` como carpeta hermana de `FrontOne.WinForms/` dentro de `E:\FrontOne` (un solo repo, contexto compartido), no un repositorio separado.

## Estado actual: scaffold inicial creado

Estructura Gradle multi-módulo (3 módulos = las 3 capas hexagonales, ver tabla en `CLAUDE.md`):

```
FrontOne.Android/
  settings.gradle.kts, build.gradle.kts, gradle.properties
  gradle/wrapper/gradle-wrapper.properties   (jar del wrapper NO generado, ver nota abajo)
  domain/   → Kotlin puro. model/ResultadoConexion.kt, port/ConexionSqlServerPort.kt, usecase/ProbarConexionUseCase.kt
  data/     → Kotlin + mssql-jdbc. sqlserver/{ConfiguracionConexion,ConnectionFactory,SqlRepositoryBase,ConexionSqlServerAdapter}.kt
  app/      → Android + Compose + Hilt. FrontOneApplication.kt, MainActivity.kt, di/DataModule.kt, ui/theme/Theme.kt, ui/conexion/{Screen,ViewModel}.kt
```

**Caso de uso piloto**: `ProbarConexionUseCase` — un flujo end-to-end completo (puerto → adaptador JDBC → Hilt → ViewModel → Compose) que ejecuta `SELECT @@VERSION` contra SQL Server y muestra el resultado en pantalla. No es un módulo de negocio real, es la plantilla de referencia para verificar que las 3 capas están correctamente conectadas antes de construir el primer módulo real. Excepción documentada a la regla "todo por SP": esta única consulta no es dato de negocio, no amerita un Stored Procedure — ver comentario en `ConexionSqlServerAdapter.kt`.

**Pendiente, no resuelto todavía en el scaffold**:
- El wrapper de Gradle (`gradlew`/`gradlew.bat` + `gradle-wrapper.jar`) no se generó — el jar es un binario que no se puede crear por texto. Al abrir la carpeta en Android Studio, este genera/gestiona el wrapper automáticamente; si se prefiere la terminal, correr `gradle wrapper` una vez con una instalación local de Gradle.
- `di/DataModule.kt` tiene credenciales de conexión **hardcodeadas como placeholder** (`servidor = "172.16.1.100"`, login/password `"TODO_..."`) — deliberado, solo para que el scaffold compile. Antes de probar contra un servidor real hay que reemplazarlas a mano (o construir ya la pantalla de configuración con `EncryptedSharedPreferences`, ver pendiente de `CLAUDE.md`).
- No hay login SQL dedicado definido todavía — el placeholder asume que se creará antes de la primera prueba real contra `172.16.1.100\FrontOne` o el servidor que corresponda al entorno.
- Ningún módulo de negocio real construido todavía — el primer módulo se elige en una sesión futura (se evaluaron como candidatos: Recepción de Fruta, escaneo GS1-128, dashboard de Almacenes de solo lectura, ver la conversación de brainstorming original).

## Cómo compilar (una vez resuelto el wrapper)

```
./gradlew :app:assembleDebug
```

o abrir la carpeta `FrontOne.Android/` directo en Android Studio (detecta el `settings.gradle.kts` y sincroniza los 3 módulos automáticamente).

## Iteración: ajuste de versiones para sincronizar en la máquina real del usuario

Primera sincronización real en Android Studio (`E:\android`, versión AI-261 / "2026.1.3") destapó una cadena de incompatibilidades de versión — herramientas de punta (JDK 25/26 recién salidos) contra un scaffold armado con versiones "seguras" de 2024. Resuelto en cascada:

1. **Gradle 8.9 no soporta JDK > ~23** ("Incompatible Gradle JVM version") → se subió el wrapper a **Gradle 9.0.0** (`gradle/wrapper/gradle-wrapper.properties`), misma versión que ya estaba instalada localmente en `C:\gradle\gradle-9.0.0`.
2. **Gradle 9.0.0 tampoco soporta JDK 25/26** — tope real de esa versión es **JDK 24**. Se seleccionó JDK 21 (`jbr-21`, JetBrains Runtime bundleado con Android Studio) como Gradle JDK del proyecto — no es una limitación del proyecto, es un techo real de Gradle 9.0.0 documentado por el propio IDE.
3. **Kotlin 2.0.21 no soporta JDK > 25** ("Incompatible JDK Version", pide Kotlin ≥ 2.1.10) → se subió el plugin de Kotlin a **2.1.10** en `build.gradle.kts` raíz (`org.jetbrains.kotlin.android`/`.jvm`), y KSP a `2.1.10-1.0.29` (debe ir siempre pegado a la versión de Kotlin, formato `{kotlinVersion}-{kspVersion}`).
4. Se subió **AGP de 8.6.0 a 8.9.1** en el mismo movimiento, para no quedar con un Gradle 9.0 corriendo un AGP viejo (riesgo de un cuarto error de compatibilidad encadenado).
5. **Efecto colateral necesario, no relacionado a las versiones de JDK**: Kotlin 2.0+ requiere aplicar el plugin `org.jetbrains.kotlin.plugin.compose` por separado en cualquier módulo con `buildFeatures.compose = true` — antes del Kotlin Gradle Plugin 2.0 el compilador de Compose se configuraba con `composeOptions.kotlinCompilerExtensionVersion`, eso ya no aplica. Se agregó el plugin en `build.gradle.kts` raíz y en `app/build.gradle.kts`; el scaffold original (creado antes de esta sincronización real) no lo tenía y hubiera fallado en la primera compilación de una pantalla Compose aunque el resto sincronizara bien.

**Lección para sesiones futuras**: en máquinas con herramientas muy recientes (JDK 25/26, Android Studio de builds nuevos), sincronizar una sola vez en el IDE real antes de dar por bueno un scaffold armado a ciegas — las versiones "estables conocidas" al momento de escribir el código quedan obsoletas rápido frente al entorno real del usuario. Ninguno de estos 4 ajustes cambió una sola línea de la arquitectura/código de negocio (`domain`/`data`/`ui`), solo números de versión de plugins — la separación de capas hexagonales absorbió el problema sin tocar lógica.

**Confirmado por el usuario (2026-08-07)**: sincronización completa en Android Studio real (`E:\android`) sin errores, con Gradle JDK 21 (`jbr-21`). El scaffold de las 3 capas hexagonales (`:domain`/`:data`/`:app`) queda validado end-to-end contra herramientas reales, no solo en teoría.

## Iteración: mssql-jdbc no funciona en runtime Android — se cambió a jTDS

Primera corrida real en dispositivo/emulador contra `172.16.1.100\FrontOne` (usuario `sa`, solo para esta prueba de humo — sigue pendiente el login dedicado). Crash inmediato al presionar "Probar Conexión":

```
FATAL EXCEPTION: main
java.lang.AssertionError: numMsgsRcvd:1 should be less than numMsgsSent:1
    at com.microsoft.sqlserver.jdbc.TDSReader.readPacket(IOBuffer.java:6879)
    ...
    at com.android.org.conscrypt.ConscryptEngineSocket.doHandshake(ConscryptEngineSocket.java:242)
    at com.microsoft.sqlserver.jdbc.TDSChannel.enableSSL(IOBuffer.java:1854)
    at com.microsoft.sqlserver.jdbc.SQLServerConnection.connectHelper(SQLServerConnection.java:3797)
```

**Causa raíz**: `mssql-jdbc` implementa su fase de "login-only encryption" (el handshake TLS obligatorio que envuelve el paquete `LOGIN7` incluso cuando `encrypt=false`) con un wrapper de socket propio que asume el comportamiento de un `SSLSocket` de un JSSE de escritorio estándar. `Conscrypt` (el proveedor SSL por defecto de Android) maneja el buffering de registros TLS de forma distinta, y esa asunción interna del driver revienta con un `AssertionError` — es una incompatibilidad conocida y documentada de `mssql-jdbc` sobre Android, no un error de configuración del proyecto. Confirma lo que ya se había marcado como riesgo desde que se eligió el driver (ver sección "Alternativas de conectividad" de la conversación original): Microsoft no certifica `mssql-jdbc` para Android.

**Fix**: se cambió el driver de `:data` de `mssql-jdbc` a **jTDS** (`net.sourceforge.jtds:jtds:1.3.1`) — no implementa ese mecanismo propietario de TLS-sobre-TDS, así que no pisa el bug. Cambios:
- `data/build.gradle.kts`: dependencia reemplazada.
- `ConfiguracionConexion.kt`: cadena JDBC cambió de formato `jdbc:sqlserver://...` a `jdbc:jtds:sqlserver://...`; campos `cifrarConexion`/`confiarEnCertificadoServidor` (semántica de mssql-jdbc) reemplazados por `usarSsl: Boolean = false` (semántica de jTDS, `ssl=off|require`). Default `false` porque ya se había acordado que la red es local/VPN confiable — evita reintroducir el mismo tipo de problema hasta que haya una razón real para cifrar en tránsito.
- `ConnectionFactory.kt`: se agregó `Class.forName("net.sourceforge.jtds.jdbc.Driver")` explícito en el `init` — no confiar en el auto-registro por `ServiceLoader`/`META-INF/services` de `DriverManager`, que en Android no siempre es confiable.
- `app/proguard-rules.pro`: nota actualizada al driver nuevo.

**No cambió nada de la arquitectura ni de `SqlRepositoryBase`** — el `ConexionSqlServerPort`/`ConexionSqlServerAdapter` siguen exactamente igual, solo cambió qué hay detrás de `ConnectionFactory.abrirConexion()`. Es la prueba de que separar en capas hexagonales valió la pena: un problema de infraestructura real (driver incompatible con la plataforma) se resolvió tocando solo `:data`, sin tocar `:domain` ni `:app`.

**Confirmado por el usuario (2026-08-07), en hardware real**: corrida en un terminal Honeywell EDA52 (dispositivo físico, no emulador — coincide con el perfil de hardware esperado para el caso de uso real de campo/planta) contra `172.16.1.100\FrontOne`. Pantalla mostró "Conexión exitosa. Servidor: Microsoft SQL Server 2022 (RTM-GDR) (KB5102334) - 16.0.1190.2 (X64)".

**Con esto queda cerrado el objetivo del scaffold**: las 3 capas hexagonales (`:domain`/`:data`/`:app`), Compose, Hilt, y la conectividad real a SQL Server (con jTDS) están verificadas de punta a punta en hardware real, no solo en teoría ni en compilación.

## Iteración: pantalla de Login (visual) + primer caso de uso de negocio real (logo) + Configuración de Conexión

Pedido del usuario: diseño profesional para la pantalla de login, integrar el logo real de la empresa, y mover el "modo desarrollador" a un ícono de ajustes que abra una pantalla de configuración de conexión (como `ConfiguracionConexionesForm` de escritorio). Se buscó primero un archivo de logo en todo el repo (`find` por extensión de imagen + `grep` de "fronterra") — no existe ninguno; el logo de la empresa vive únicamente como dato binario en `Configuracion.Empresa.Logo` (`VARBINARY(MAX)`), cargado por el usuario desde `ConfiguracionEmpresaForm` en escritorio. El usuario pidió traerlo de la base de datos en vivo, no como asset estático — esto lo convirtió en el **primer caso de uso de negocio real del proyecto** (todo lo anterior era el piloto de conectividad).

### Caso de uso: logo de Configuracion.Empresa

Patrón hexagonal completo de 6 piezas (mismo que documenta `CLAUDE.md`):
- `domain/port/EmpresaPort.kt` — `suspend fun obtenerLogo(): ByteArray?`. El dominio pide bytes crudos, nunca conoce `android.graphics.Bitmap` (eso es UI, vive en `:app`).
- `domain/usecase/ObtenerLogoEmpresaUseCase.kt`.
- `data/sqlserver/EmpresaSqlServerAdapter.kt` — extiende `SqlRepositoryBase` (primer uso real de esa clase base, antes solo la implementaba el piloto de conexión sin heredar de ella) y llama `Configuracion.sp_Empresa_Obtener` (el mismo SP que ya usa FrontOne de escritorio, sin parámetros — tabla singleton Id=1), leyendo la columna `Logo` con `ResultSet.getBytes("Logo")`.
- Binding en `app/di/DataModule.kt`.
- `app/ui/login/LoginViewModel.kt` — decodifica los bytes a `Bitmap`/`ImageBitmap` (única capa que puede, por tener acceso a `android.graphics`), con `sealed interface EstadoLogo` (Cargando/Disponible/NoDisponible). Si la consulta falla (sin red, tabla vacía, lo que sea) degrada a `NoDisponible` en silencio — el login debe seguir siendo usable aunque el logo no cargue, no es una función crítica.
- `LoginScreen.kt` muestra un ícono `Business` genérico como placeholder mientras no hay logo real cargado en la base.

### Rediseño visual del login

Paleta propia en `ui/theme/Theme.kt` (antes `lightColorScheme()`/`darkColorScheme()` vacíos, heredaban el morado default de Material3 sin intención) — azul/índigo marcado como **placeholder explícito**, no decisión de marca final, hasta que exista una paleta oficial de Fronterra. `LoginScreen.kt` reestructurado: `Surface` de fondo + `Card` con esquinas redondeadas conteniendo los campos + botón de altura fija, tipografía con jerarquía (headline para "FrontOne", body para el subtítulo).

### Configuración de Conexión — reemplaza el "modo desarrollador" + implementa `EncryptedSharedPreferences`

Esto resolvió de una vez el pendiente que había quedado abierto desde el arranque del proyecto ("mover credenciales de `BuildConfig` a `EncryptedSharedPreferences`"):

- **`app/config/ConfiguracionConexionStore.kt`** (nuevo, vive en `:app` — no en `:data`/`:domain`, porque `EncryptedSharedPreferences` necesita `Context` de Android, y la regla hexagonal del proyecto es que ni el dominio ni el adaptador de datos pueden depender del framework de Android). Lee/guarda servidor/puerto/base de datos/usuario/password cifrados (respaldados por Android Keystore, `MasterKey.KeyScheme.AES256_GCM`). Si el usuario nunca guardó nada (primer arranque), cae de vuelta a los valores de `BuildConfig`/`secrets.properties` — así el flujo de desarrollo original sigue funcionando sin configurar nada a mano.
- **`ConnectionFactory` cambió de recibir un `ConfiguracionConexion` fijo a recibir un `() -> ConfiguracionConexion`** (`data/sqlserver/ConnectionFactory.kt`) — se consulta en cada `abrirConexion()`, no una sola vez al armar el grafo de Hilt. Así, si el usuario guarda credenciales nuevas desde la pantalla de Configuración, la siguiente operación las usa de inmediato, sin reiniciar la app. `:data` sigue sin conocer `EncryptedSharedPreferences` ni Android — solo recibe una función, mantiene la pureza de la capa.
- **`ui/configuracion/ConfiguracionConexionScreen.kt` + `ConfiguracionConexionViewModel.kt`** (nuevo, equivalente móvil de `ConfiguracionConexionesForm.cs`): campos Servidor/Puerto/Base de datos/Usuario/Contraseña, botones "Guardar" y "Probar Conexión". "Probar Conexión" primero guarda los valores capturados y luego corre `ProbarConexionUseCase` — como `ConnectionFactory` ahora lee del store en cada llamada, esto garantiza que la prueba use exactamente lo que el usuario acaba de escribir, no un valor viejo en memoria.
- **`MainActivity.kt`**: el botón de texto "Modo desarrollador: Probar Conexión" se reemplazó por un `FloatingActionButton` con ícono de ajustes (`Icons.Filled.Settings`), abajo a la derecha — abre `ConfiguracionConexionScreen`. La pantalla `ui/conexion/ProbarConexionScreen.kt` (el piloto original) se queda en el proyecto como plantilla de referencia documentada en `CLAUDE.md`, pero ya no está enganchada a ningún flujo de navegación — su funcionalidad quedó absorbida por la pantalla de Configuración.
- Dependencia nueva: `androidx.security:security-crypto:1.1.0-alpha06`. También se cambió `material-icons-core` (agregado unos minutos antes, para el ícono `Business` del logo) por **`material-icons-extended`** — `Business`/`Settings` no están en el subconjunto chico de `-core`.

**Pendiente, no resuelto en esta iteración**:
- Login SQL dedicado (todo sigue probándose con `sa`) — sigue igual de pendiente que antes, ahora es más fácil de rotar porque ya existe la pantalla de Configuración para cambiar el usuario sin tocar código.
- La pantalla de Login sigue sin funcionalidad real de autenticación (el botón "Iniciar Sesión" no llama nada todavía) — el usuario pidió explícitamente construir primero solo la parte visual.
- Verificación en vivo de esta iteración completa (logo real cargando, pantalla de Configuración guardando/probando) — pendiente de que el usuario compile y pruebe en el Honeywell EDA52.

### Fix de compilación: import de `weight` sobrando en ConfiguracionConexionScreen.kt

Al compilar salió `Cannot access 'val RowColumnParentData?.weight: Float': it is internal in file`, apuntando al import `androidx.compose.foundation.layout.weight`. Primer intento (equivocado): se asumió desfase de versión Compose-Kotlin y se subió `compose-bom` de `2024.12.01` a `2026.06.01` (verificado contra el catálogo oficial de BOMs de Google, que efectivamente ya llega hasta esa versión — cambio válido igual, el proyecto estaba usando un BOM más de un año atrasado frente al resto del stack). El error persistió idéntico después de eso, lo que descartó la hipótesis de versión.

**Causa real**: `weight` no es una función de nivel superior del paquete `androidx.compose.foundation.layout` — es un miembro extensión de las interfaces `RowScope`/`ColumnScope`, ya visible automáticamente dentro del lambda de contenido de un `Row {}`/`Column {}` sin necesidad de ningún `import`. El import explícito enganchaba un símbolo distinto (de visibilidad interna, pensado para uso interno de la librería), no la función pública real. Se quitó el import — `Modifier.weight(1f)` dentro del `Row` de los botones Guardar/Probar Conexión sigue funcionando igual, solo que resuelto por scope implícito.

**Lección para sesiones futuras**: si el autocompletado del IDE ofrece importar `weight` (o cualquier extensión de `RowScope`/`ColumnScope`/`BoxScope`) como símbolo de paquete, no aceptarlo — esas funciones nunca se importan, ya están disponibles dentro del scope correspondiente.
