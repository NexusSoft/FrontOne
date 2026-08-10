# Contexto del proyecto FrontOne.Android

> Memoria viva de este sub-proyecto, mismo espíritu que [`../contexto.md`](../contexto.md) en la raíz de `E:\FrontOne`. Las reglas duras viven en [`CLAUDE.md`](CLAUDE.md) (se carga automáticamente en toda sesión de Claude Code sobre esta carpeta). Este archivo es el índice; el detalle vive en `contexto/`.

## Qué es FrontOne.Android

Extensión móvil (Android, Kotlin + Jetpack Compose) de FrontOne — el ERP de escritorio en `E:\FrontOne`. Comparte la misma base de datos SQL Server, el mismo vocabulario de negocio y el mismo espíritu arquitectónico (capas con inversión de dependencias) que el proyecto de escritorio, adaptado a arquitectura Hexagonal en Kotlin. No reemplaza ni duplica catálogos/seguridad/módulos ya construidos en WinForms — ver `CLAUDE.md` para el alcance exacto.

## Índice — dónde está cada cosa

| Archivo | Contenido |
|---|---|
| [`contexto/arquitectura.md`](contexto/arquitectura.md) | Decisiones de stack/arquitectura ya tomadas, estado del scaffold inicial, cómo compilar, pendientes. |

## Cómo seguir trabajando en este sub-proyecto (para cualquier sesión nueva de Claude)

1. Leer este índice + `CLAUDE.md` (de esta carpeta), y también `CLAUDE.md`/`contexto.md` de la raíz de `E:\FrontOne` — este proyecto consume el mismo backend/SPs que ya documentan esos archivos.
2. Antes de construir un módulo nuevo, copiar el patrón de `app/src/main/kotlin/com/frontone/android/ui/conexion/` (piloto de conectividad SQL Server) como plantilla de las 6 piezas descritas en `CLAUDE.md` (entidad → puerto → caso de uso → adaptador → binding Hilt → ViewModel/Screen).
3. **Actualizar `contexto/arquitectura.md`** al cerrar cualquier cambio de alcance medio/grande (módulo nuevo, decisión de arquitectura, cambio de estructura) — mismo criterio append-only que ya usa `contexto/*.md` en la raíz del repo: agregar secciones nuevas al final, nunca editar/borrar una entrada vieja.
4. Si un cambio afecta también al backend (SP nuevo, tabla nueva), documentarlo en el `contexto/{modulo}.md` correspondiente de la **raíz** del repo (`E:\FrontOne\contexto\`), no aquí — este `contexto/` es solo del lado Android.
