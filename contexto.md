# Contexto del proyecto FrontOne

> Este archivo es la memoria viva del proyecto para cualquier sesión de Claude (u otro dev) que se sume al equipo. Resume qué se construyó, cómo está organizado, qué reglas rigen y qué falta. **Se actualiza cada vez que se agrega o cambia algo relevante** — no es un documento que se escribe una vez y se olvida.
>
> Las reglas duras de nomenclatura y patrones de código viven en [`CLAUDE.md`](CLAUDE.md) (se cargan automáticamente en cada sesión de Claude Code sobre este repo). Este archivo (`contexto.md`) es el índice y el resumen fundacional; el detalle de cada módulo vive en archivos separados dentro de `contexto/` (ver tabla abajo) — se dividió así porque el proyecto es colaborativo (dos o más personas editando en paralelo) y un solo archivo enorme generaba conflictos de git constantes y ya no cabía en una sola lectura.

## Qué es FrontOne

ERP para una empacadora/exportadora de aguacate. WinForms de escritorio (.NET 10), arquitectura en capas, SQL Server vía stored procedures (Dapper, sin EF Core), integración con SAP Business One Service Layer, UI 100% DevExpress 26.1.

Documento de arquitectura original: `Reglas/Arquitecto Senior .NET - Generación de ERP Empresarial.docx`.

## Índice — dónde está cada cosa

| Archivo | Contenido |
|---|---|
| [`contexto/arquitectura.md`](contexto/arquitectura.md) | Capas del proyecto, infraestructura/credenciales de desarrollo, Fase 1 (esqueleto) y Fase 2 (Seguridad/Auditoría/Conexiones), decisiones de UI ya tomadas, patrones de código establecidos, convenciones de nombres, regla de idioma (comentarios/mensajes en español). |
| [`contexto/catalogos.md`](contexto/catalogos.md) | Catálogos base (País/Estado/Municipio/Población), Seguridad UI, Productores, Huertas (incluye el mapa GMap.NET y su layout), importación masiva de datos reales, jerarquía geográfica, navegación por botones (reemplazo de `DataNavigator`). |
| [`contexto/acopio.md`](contexto/acopio.md) | Todo el schema `Acopio` + `Acarreo`: Lista de Precio Fruta, Tipos de Corte/Pago, Acuerdo de Corte, Zona, Lista de Precios de Acarreo, Jefes de Acopio, Lista de Precios de Corte, Orden de Corte. |
| [`contexto/recepcion.md`](contexto/recepcion.md) | Schema `Recepcion`: módulo Recepción de Fruta (báscula, cajas, ticket de pesada). |
| [`contexto/reportes.md`](contexto/reportes.md) | Infraestructura de reportes (DevExpress XtraReports), primer reporte "Recepción de Fruta". |
| [`contexto/lotes.md`](contexto/lotes.md) | Schema `Lotes`: Conformación de Lotes a partir de Recepciones de Fruta, fórmula del folio juliano "Referencia", catálogo Líneas de Producción. |
| [`contexto/almacenes.md`](contexto/almacenes.md) | Schema `Almacenes`: control de inventario de Caja de Campo (existencias/pérdidas), movimientos automáticos desde Orden de Corte/Recepción, dashboard. |
| [`contexto/produccion.md`](contexto/produccion.md) | Schema `Produccion`: Corridas (proceso de un Lote, Peso Factor), Pallets (armado de tarimas, estatus calculado, Pallet Neutro, báscula), Etiquetado (GS1-128/VoiceCode/Sagarpa), sincronización SAP de Productos Terminados (grupos PT/ST). |
| [`contexto/reempaques.md`](contexto/reempaques.md) | Desarmar un Pallet y reconstruir uno o más nuevos sin perder trazabilidad de Lote — vive sobre `Produccion.PalletDetalle`, no en tablas propias. |
| [`contexto/gastos.md`](contexto/gastos.md) | Schema `Gastos`: liquidación de costos de Fruta/Cosecha/Acarreo por Lote, reanclaje de `ListaPrecioFruta` a Categoría×Calibre APEAM. |
| [`contexto/embarques.md`](contexto/embarques.md) | Módulo Embarques → Logística → Pedidos: consulta de solo lectura de Pedidos de Venta capturados en SAP (`Orders`), sin tabla propia en SQL. |

## Pendientes / ideas no implementadas todavía

- Íconos asignados a mano en algunos botones de módulos viejos podrían no coincidir 1:1 si se tocan de nuevo en el diseñador de VS — si eso pasa, restaurar desde `EstadosForm.resx` (ver `contexto/arquitectura.md`).
- Paginación/filtrado server-side para Huertas/Productores más allá de lo ya hecho (ver `contexto/catalogos.md`, sección de importación masiva).

## Cómo seguir trabajando en este proyecto (para cualquier sesión nueva de Claude)

1. Leer este índice + `CLAUDE.md`, y luego el archivo de `contexto/` que corresponda al módulo que se va a tocar (no hace falta leer los 5 completos si el cambio es acotado a uno).
2. Antes de construir un módulo nuevo, copiar el patrón de un módulo ya hecho (Huertas, en `contexto/catalogos.md`, es el más completo: maestro-detalle + catálogos de apoyo + lookups encadenados).
3. Compilar con `dotnet build FrontOne.slnx -p:UseAppHost=false` (si la app está corriendo, el `.dll` queda bloqueado — pedir al usuario que la cierre antes de compilar).
4. Scripts SQL nuevos van numerados dentro de `Database/{Schema}/`, y se ejecutan con `sqlcmd` contra el servidor activo (ver `contexto/arquitectura.md` para credenciales/instancia). Nunca asumir que ya corrieron — confirmar contra la BD real.
5. **Actualizar (o crear) el archivo de `contexto/` correspondiente** al cerrar cualquier cambio de alcance medio/grande — regla dura, ver la sección "Regla dura: todo módulo nuevo actualiza o crea su archivo en `contexto/`" en [`CLAUDE.md`](CLAUDE.md) (se carga automáticamente en cada sesión, a diferencia de este archivo).

### Trabajo colaborativo con `contexto/` (dos o más personas, sincronizando por git)

- Cada archivo de `contexto/*.md` es **append-only**: agregar secciones nuevas al final, nunca editar/borrar una entrada vieja de otra sesión — si algo cambió, se agrega una nota nueva que lo señale (mismo criterio que ya se usaba antes de la división, ej. la sección de idioma en `arquitectura.md` documenta un barrido retroactivo así). Este archivo raíz (`contexto.md`) es la excepción: su índice y su intro sí se editan en el lugar cuando hace falta.
- Antes de empezar una sesión: `git pull`. Al terminar: commitear y pushear los cambios de `contexto/*.md` **junto con el código que documentan** (mismo commit/PR), no dejarlos sueltos localmente — así la siguiente sesión (propia o de otro dev) arranca siempre con el contexto real más reciente.
- `contexto/*.md` tiene `merge=union` en `.gitattributes` — si dos personas agregan una sección al final el mismo día, git las combina automáticamente sin marcar conflicto, siempre que la disciplina de "solo agregar, nunca editar lo viejo" se respete (si alguien edita una entrada vieja, el union merge puede mezclar mal el resultado — revisar el diff igual antes de pushear).
- Si un cambio toca más de un módulo, se documenta en el archivo del módulo "dueño" del cambio principal.

## Prueba de flujo colaborativo - 2026-07-24
Este cambio prueba el flujo de ramas y Pull Requests del equipo.
