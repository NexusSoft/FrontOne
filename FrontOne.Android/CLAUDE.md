# FrontOne.Android — Reglas del proyecto

> Sub-proyecto dentro de `E:\FrontOne`. Extensión móvil de FrontOne (ERP de escritorio en C#/WinForms) — **no** es un proyecto independiente: comparte base de datos, vocabulario de negocio y espíritu arquitectónico con el proyecto de escritorio. Este archivo se carga automáticamente en toda sesión de Claude Code sobre esta carpeta, igual que el `CLAUDE.md` de la raíz del repo se carga sobre `E:\FrontOne` completo.

## Qué es y qué NO es esta app

FrontOne.Android es una **extensión ágil** para capturar/consultar en campo/planta funcionalidad que eventualmente se traslada a escritorio — no un ERP completo aparte.

**Se queda exclusivamente en escritorio (WinForms), nunca se replica aquí:**
- Catálogos (País/Estado/Municipio/Población, Productores, Huertas, etc.)
- Seguridad: Usuarios, Roles, Permisos, matriz de permisos
- Cualquier módulo ya construido y estable en WinForms, salvo que el usuario pida explícitamente portarlo

**Si un módulo nuevo nace aquí y después se traslada a escritorio**: la lógica de negocio (validaciones, cálculos) debe quedar documentada de forma que sea fácil de "traducir" a un `{Entidad}Service.cs` — ver regla de arquitectura hexagonal abajo. La fuente de verdad de las reglas de negocio vive en los Stored Procedures/capa `usecase`, Android nunca debe ser el único lugar donde una regla de negocio existe si se planea portar a escritorio.

## Regla dura: arquitectura Hexagonal (Clean, versión ligera)

Espejo directo de las capas que ya tiene FrontOne en C# — no es una arquitectura nueva, es la misma idea en Kotlin:

| Capa Android (Gradle module) | Equivalente en FrontOne (C#) | Contenido |
|---|---|---|
| `:domain` | `FrontOne.Domain` | Entidades, DTOs (`data class`), **puertos** (`interface`), casos de uso (`usecase/`) |
| `:data` | `FrontOne.Infrastructure.SqlServer` | Adaptadores que implementan los puertos, vía JDBC + Stored Procedures |
| `:app` | `FrontOne.WinForms` | UI Compose (adaptador primario) + composition root (Hilt) |

Reglas de dependencia (idénticas al principio "`Application` no puede depender de `WinForms`" del proyecto de escritorio):
- `:domain` **nunca** depende de `:data` ni de `:app`. Solo Kotlin puro + coroutines.
- `:data` depende de `:domain` (implementa sus puertos), nunca de `:app`.
- `:app` depende de ambos, pero solo instancia adaptadores concretos en la capa `di/` (Hilt) — el resto del código de `:app` (ViewModels, pantallas) solo ve tipos de `:domain`.

**Versión "ligera" del hexagonal — evitar sobre-ingeniería:**
- ✅ Puertos como interfaces simples, casos de uso como clases planas (`class XUseCase(private val port: XPort)`).
- ❌ Sin Value Objects envolviendo cada primitivo, sin Aggregate Roots, sin CQRS/Mediator. Mismo nivel de simplicidad que ya usan los `Service` de `FrontOne.Application` en C#.

**Todo módulo nuevo repite este patrón** (equivalente al "Entidad → DTO → Interfaz → Repositorio → SPs → Servicio → Validador → Form(s)" de `CLAUDE.md` raíz):

```
1. Entidad/DTO en :domain/model
2. Puerto (interfaz) en :domain/port
3. Caso de uso en :domain/usecase (orquesta el puerto, valida)
4. Adaptador en :data/sqlserver (extiende SqlRepositoryBase, llama el/los SP)
5. Binding en :app/di (Hilt @Provides o @Binds conectando puerto → adaptador)
6. ViewModel + Screen en :app/ui/{modulo}
```

## Regla dura: Kotlin + Jetpack Compose

- UI 100% Compose — sin XML de layouts, sin Views clásicas (mismo espíritu que "UI 100% DevExpress, sin excepción" del proyecto de escritorio, adaptado a la plataforma).
- Patrón de pantalla: `{Modulo}Screen` (Composable "tonto", solo observa estado) + `{Modulo}ViewModel` (`@HiltViewModel`, `StateFlow` de un `sealed interface Estado{Modulo}`) — ver `ui/conexion/ProbarConexionScreen.kt` y `ProbarConexionViewModel.kt` como plantilla de referencia.
- DI con Hilt (`@Module @InstallIn(SingletonComponent::class)`), un módulo por responsabilidad en `app/di/` — ver `DataModule.kt`.
- Coroutines para toda operación asíncrona; acceso a datos siempre en `Dispatchers.IO` (ya resuelto dentro de `SqlRepositoryBase`/adaptadores — un ViewModel nunca debe especificar el dispatcher manualmente).

## Regla dura: SQL Server, mismo servidor que FrontOne de escritorio

- **Conexión directa a SQL Server, nunca una API intermedia.** Mismo servidor/base que usa `FrontOne.WinForms` (ver `contexto/arquitectura.md` de la raíz del repo para el servidor/instancia vigente en cada entorno).
- **Todo acceso a datos vía Stored Procedure existente o nuevo** — nunca SQL crudo armado en Kotlin. Si un flujo mobile necesita datos que hoy no expone ningún SP, se agrega un SP nuevo en `Database/{Schema}/` siguiendo exactamente las mismas convenciones que ya usa el proyecto (`{Schema}.sp_{Entidad}_{Accion}`, comentarios en español).
- Toda llamada a SP pasa por `SqlRepositoryBase.ejecutarProcedimiento` (`:data/sqlserver/SqlRepositoryBase.kt`) — ningún adaptador nuevo debe abrir su propio `PreparedStatement`/`CallableStatement` fuera de esa clase base.
- **Login SQL dedicado, nunca `sa`**: antes de conectar contra un servidor real, crear un login con `GRANT EXECUTE` acotado solo a los SPs que la app consume — pendiente de definir el detalle exacto cuando se elija el primer módulo real (hoy el scaffold usa un placeholder, ver `di/DataModule.kt`).
- Red asumida: local/VPN de la empresa (confiable) — **sin caché offline por ahora**, la app es *online-only*. Si algún módulo futuro lo requiere, evaluar entonces una cola de reintentos local, no diseñarlo por adelantado.
- Auditoría: cuando un caso de uso real escriba datos de negocio, debe llamar el mismo esquema `Auditoria.Registro` que ya usa el proyecto de escritorio (vía un SP existente o nuevo, mismo patrón JSON antes/después) — no se crea un sistema de auditoría paralelo.

## Credenciales de conexión (pendiente de resolver antes de producción)

El scaffold trae la configuración de conexión hardcodeada como placeholder en `app/di/DataModule.kt` **solo para poder compilar y probar conectividad**. Antes de conectar contra un servidor con datos reales:

1. Mover la configuración a `EncryptedSharedPreferences` (Jetpack Security), cargada desde una pantalla de configuración inicial (equivalente a `ConfiguracionConexionesForm` de WinForms) — la captura un admin de TI una sola vez por dispositivo, no cada usuario.
2. Nunca hardcodear password/servidor en código versionado ni en `BuildConfig`.
3. Confirmar con el usuario el login SQL dedicado antes de usar cualquier credencial con privilegios amplios.

## Nomenclatura

Mismo criterio que el proyecto de escritorio: sustantivos de negocio en **español** (`RecepcionFruta`, `Huerta`), identificadores técnicos/patrones en inglés donde ya es estándar de Kotlin/Android (`ViewModel`, `UseCase`, `Repository`/`Port`/`Adapter`). Comentarios de código y todo texto visible en la UI (labels, mensajes de error, botones) van en **español** — misma regla dura que `CLAUDE.md` de la raíz.

## Estructura de paquetes (paquete base `com.frontone.android`)

```
domain/model/       → entidades y DTOs
domain/port/        → interfaces (puertos)
domain/usecase/      → casos de uso
data/sqlserver/       → ConnectionFactory, SqlRepositoryBase, adaptadores concretos
app/di/               → módulos Hilt
app/ui/{modulo}/       → Screen + ViewModel por módulo/pantalla
app/ui/theme/         → tema Compose compartido
```

## Cómo seguir trabajando en este sub-proyecto (para cualquier sesión nueva de Claude)

1. Leer este archivo + `contexto.md`/`contexto/arquitectura.md` de esta misma carpeta, y también el `CLAUDE.md`/`contexto.md` de la raíz de `E:\FrontOne` (documentan el backend/SPs que este proyecto consume).
2. Antes de un módulo nuevo, copiar el patrón de `ui/conexion/` (el piloto de conectividad) como plantilla de las 6 piezas listadas arriba.
3. Compilar con Gradle (`./gradlew :app:assembleDebug` una vez generado el wrapper — ver nota en `contexto/arquitectura.md`).
4. Actualizar `contexto/arquitectura.md` al cerrar cualquier cambio de alcance medio/grande, mismo criterio append-only que ya usa `contexto/*.md` en la raíz del repo.
