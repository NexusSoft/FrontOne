# Arquitectura, convenciones y decisiones fundacionales

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo. Este archivo cubre lo fundacional (capas, infraestructura, convenciones, reglas de UI) que cambia poco. El detalle de cada módulo de negocio vive en su propio archivo dentro de esta carpeta.

## Arquitectura (capas)

```
FrontOne.Domain              Entidades, DTOs (records), interfaces de repositorio, enums
FrontOne.Application          Servicios de negocio (orquestan repos + validación + auditoría)
FrontOne.Infrastructure.SqlServer   Repositorios Dapper contra SQL Server (solo stored procedures)
FrontOne.Infrastructure.SapB1       Cliente del SAP Service Layer
FrontOne.Shared               Cross-cutting: Result/PagedResult, excepciones, CryptoService, Options, interfaces de seguridad
FrontOne.WinForms             UI (DevExpress), composition root (Program.cs + DI)
FrontOne.Tests                xUnit
```

Reglas de dependencia: `Application` no puede depender de `WinForms`. El puente para saber "qué usuario está haciendo esto" es `ICurrentUserProvider` (en `Shared/Security`), implementado por `SessionContext` en WinForms.


## Infraestructura / credenciales de desarrollo

- **SQL Server**: instancia `Lider-TI`, base `FrontOne`. Auth SQL (usuario `sa`, no Windows). Ejecutar scripts con `sqlcmd -S "Lider-TI" -d FrontOne -E -b -f 65001 -i archivo.sql` (el `-f 65001` es necesario para que los acentos no se corrompan).
- **SAP B1 Service Layer**: `https://fronterra.vdv.one:50000/b1s/v1`, compañía `TEST_PROD_FASA`, usuario `manager`.
- Ambas credenciales completas están en la memoria persistente de Claude (`sql_test_credentials.md`, `sap_test_credentials.md`), no se repiten aquí por seguridad.


## Estado por fase (todo lo marcado está construido, compilado y probado contra la BD real)

### Fase 1 — Esqueleto de capas
`Result`/`PagedResult`/excepciones/Options (Shared), `SqlRepositoryBase`/`ConnectionFactory` (Infra.SqlServer), `SapServiceLayerClient` (Infra.SapB1), extensiones de DI por capa, composition root en `Program.cs`.

### Fase 2 — Seguridad base + Auditoría + Conexiones
- Esquema `Seguridad`: `Usuario`, `Rol`, `Permiso` (Módulo → Pantalla → Acción), `UsuarioRol`.
- Esquema `Auditoria`: registro de Crear/Modificar/Eliminar por módulo, JSON antes/después.
- `CryptoService` (AES-256, clave derivada fija y portable — no atada a la máquina) usado para **todas** las contraseñas del sistema (conexiones, Usuario, Productor, SAP).
- `LoginForm`, `SessionContext` (sesión en memoria + permisos), `ConfiguracionConexionesForm` (prueba y guarda credenciales SQL/SAP), `RegistryConnectionStore`.


## Decisiones de UI ya tomadas (no volver a preguntar)

- **Ribbon de `MainForm`**: 3 pestañas por módulo — **Catálogos** (grupos "Ubicaciones": Países/Estados; "Socios de Negocio": Productores/Huertas), **Seguridad** (grupo "Usuarios y Roles": Usuarios/Roles/Permisos), **Sistema** (grupos "Configuración": Conexiones; "Aplicación": Salir). Botones en estilo `RibbonItemStyles.Large` (ícono arriba, texto abajo).
- **MDI con pestañas**: `MainForm` usa `DevExpress.XtraTabbedMdi.XtraTabbedMdiManager` — los forms MDI-child (Productores, Huertas) se muestran como pestañas de documento arriba, no como ventanas flotantes.
- **Flujo de catálogos vía lookupedit (regla fija)**: el botón "+" (quick-add) de un `LookUpEdit` **no** abre directo el form de alta — abre el **listado** del catálogo (`{EntidadPlural}Form`, con Nuevo/Editar/Eliminar/Cerrar), para poder editar y borrar desde ahí también, no solo crear. Excepción: Productor, cuyo "+" abre `ProductorEditarForm` directo porque ese form YA es maestro-detalle completo (navigator + Nuevo/Eliminar/Guardar), abrir el picker ahí sería un downgrade.
- **Todo `LookUpEdit` del proyecto** (regla dura, ver `CLAUDE.md`): `Properties.NullText = "Seleccionar"` siempre; si tiene botón custom (`Plus`, etc.) hay que agregar explícitamente `ButtonPredefines.Combo` antes, porque DevExpress oculta la flecha automática en cuanto agregás cualquier botón a `Properties.Buttons`.
- **Íconos de botones CRUD** (regla dura, ver `CLAUDE.md`): todo `_btnNuevo`/`_btnEditar`/`_btnEliminar`/`_btnCerrar` usa el mismo ícono en todo el proyecto, tomado de `FrontOne.WinForms/Forms/Catalogos/EstadosForm.resx`; todo `_btnGuardar`/`_btnCancelar` (de cualquier `{Entidad}EditarForm`, incluidos los maestro-detalle) usa el ícono de `FrontOne.WinForms/Forms/Catalogos/ProductoEditarForm.resx` — son dos fuentes distintas, no confundir. Los `.resx` de DevExpress son XML de texto plano (bitmap en base64), así que se pueden copiar los bloques `<data>` a mano sin abrir Visual Studio — el `.resx` tiene que vivir en la misma carpeta/namespace que su `.cs` o `resources.GetObject(...)` no encuentra el ícono en runtime.
- **Estructura de carpetas de `Forms/`**: subcarpetas por módulo — `Forms/Seguridad`, `Forms/Catalogos`, `Forms/Sistema` (cada una con su propio namespace `FrontOne.WinForms.Forms.{Carpeta}`). `MainForm` se queda en la raíz de `Forms/` por ser el shell de la app, no pertenece a ningún módulo.
- **Orden de botones** (regla dura, ver `CLAUDE.md`): en los forms maestro-detalle (`ProductorEditarForm`/`HuertaEditarForm`) el grupo izquierdo va **Nuevo, Guardar, Eliminar** en ese orden (los 3 con `Anchor = Bottom, Left`), y `Cancelar` siempre solo a la derecha (`Bottom, Right`). Mismo criterio en los listados: Nuevo/Editar/Eliminar a la izquierda, Cerrar a la derecha.


## Patrones de código establecidos (repetir en todo módulo nuevo)

- **Patrón clásico VS Designer** en todo `XtraForm`: `{Form}.Designer.cs` (parcial, controles como campos privados, `InitializeComponent()`) + `{Form}.cs` (ctor vacío para el diseñador + ctor con servicios inyectados `: this()`). Nunca lambdas inline en `Designer.cs` — rompe el diseñador de VS.
- **Maestro-detalle con `DataNavigator`**: `BindingSource` sobre `List<TDto>` cacheada, `CurrentChanged` recarga el form, "Nuevo" limpia campos sin tocar la posición del navigator, "Guardar" inserta/actualiza y reposiciona.
- **Auditoría obligatoria**: todo servicio Application con Crear/Actualizar/Eliminar inyecta `AuditService` + `ICurrentUserProvider`, relee antes/después, serializa a JSON completo (no diff campo por campo).
- **Todo módulo nuevo repite**: Entidad → DTO → Interfaz repo → Repositorio (`SqlRepositoryBase`) → SPs (`{Schema}.sp_{Entidad}_{Accion}`) → Servicio (con auditoría) → Validador → Form(s) (listado + editar, ambos con íconos e patrón clásico).
- **Permiso "quick-add" en lookups**: antes de abrir el listado/editor de un catálogo relacionado desde un "+", se valida `SessionContext.TienePermiso(modulo, pantalla, "Crear")`; si no tiene, `XtraMessageBox` de advertencia y no abre nada.


## Convenciones de nombres

Ver `CLAUDE.md` completo — resumen: sustantivos de negocio en español (`Cliente`, `Huerta`), sufijos técnicos en inglés (`Service`, `Repository`, `Form`, `Dto`), SPs `{Schema}.sp_{Entidad}_{Accion}` en español, DevExpress obligatorio sin excepción (ni en forms simples).


## Idioma: comentarios y mensajes al usuario (regla dura, ver `CLAUDE.md`)

Todo comentario de código (`//`, `/* */`, `///`, comentarios `.sql`) y todo mensaje visible para el usuario (`XtraMessageBox`, mensajes de excepción que llegan a UI, `Text`/`Caption`/`NullText`) va en **español**. Se hizo un barrido inicial sobre el código existente (el proyecto ya estaba mayormente en español; se corrigieron los pocos textos sueltos en inglés: "Password:" → "Contraseña:" en `ProductorEditarForm`/`UsuarioEditarForm`, "Status" → "Estatus" en el módulo de Huertas, y un comentario en `003_Seed_Manager.sql`). Cualquier cosa nueva que se agregue de acá en adelante debe nacer en español, no traducirse después.

**Nota de nomenclatura fijada en este barrido**: el campo de estado de vida de una huerta (catálogo `StatusHuerta`, valores tipo Nueva/Producción/etc.) se etiqueta en UI como **"Estatus"**, no "Estado" — porque el mismo form ya tiene un campo "Estado:" para el estado geográfico (`Catalogos.Estado`, ligado a País). Usar "Estado" para ambos generaba colisión visual. De paso se corrigió `_lblEstatusActivo` (el combo Activa/Baja) que estaba mal etiquetado como "Estado:" → ahora dice "Activo:". Los nombres de clase/tabla (`StatusHuerta`, `StatusHuertaService`, etc.) NO se tocaron — es solo el texto visible en pantalla el que cambió.


## Ribbon de `MainForm`: pestaña "Producción" (fusión Recepción + Lotes)

Las pestañas "Recepción" y "Lotes" del Ribbon se fusionaron en una sola pestaña **"Producción"**, en la posición donde antes estaba "Recepción" (orden final: Catálogos, Acopio, Producción, Seguridad, Sistema). Dentro de "Producción" quedan dos grupos, en este orden: "Recepción de Fruta" (`_grpRecepcionFruta`, botón `_btnRecepcionesFruta`) y "Conformación de Lotes" (`_grpLotes`, botón `_btnLotes`).

Todo el cambio vivió en `FrontOne.WinForms/Forms/MainForm.Designer.cs`: se reusó el campo `_pageRecepcion` (cambiando su `Text` a "Producción") y se movió `_grpLotes` a `_pageRecepcion.Groups`; el campo `_pageLotes` se eliminó por completo (declaración, instanciación y bloque de configuración), junto con su entrada en `_ribbon.Pages.AddRange(...)`. `MainForm.cs` no se tocó — los handlers `ItemClick` y `AplicarPermisos()` están atados a los botones, no a la página contenedora.

Motivo: Recepción y Lotes son conceptualmente el mismo flujo de negocio (producción), aunque viven en schemas de BD distintos (`Recepcion`/`Lotes`) — la organización del Ribbon no tiene que calcar 1:1 el nombre del schema.

**Regla dura nueva:** antes de agregar un módulo/pantalla nuevo a `MainForm` (WinForms Ribbon), **siempre preguntar al usuario en qué pestaña (`RibbonPage`) y grupo (`RibbonPageGroup`) debe colocarse**, sin asumir la ubicación por similitud de nombre de módulo/schema. Aplica también a reorganizaciones de pestañas existentes (fusiones, movimientos de grupos entre pestañas).

**Bug incidental corregido de paso (no relacionado a la fusión):** `MainForm.Designer.cs` tenía `Id` duplicados entre `BarButtonItem` (28 compartido por `_btnLotes`/`_btnReportePermisos`, 29 compartido por `_btnLineasProduccion`/`_btnLicenciaTecit`) — bug latente de DevExpress Bars, no lo detecta el compilador. Se corrigió reasignando `_btnReportePermisos.Id = 30` y `_btnLicenciaTecit.Id = 31`, con `_ribbon.MaxItemId = 31`.


## Ribbon: texto de la leyenda de grupo cortado con "..." — `RibbonPageGroup.AllowTextClipping`

En varias pestañas (Acopio, Producción), los grupos con solo 1-2 botones mostraban su leyenda inferior cortada con puntos suspensivos (ej. "Precios de A...", "Conformaci..."). Causa: el ancho de un `RibbonPageGroup` se calcula a partir de sus botones, no de su `Text`, así que un grupo angosto (pocos botones) no alcanza a mostrar una leyenda larga completa. Probamos primero rellenar el `Text` con espacios al final (funciona parcialmente, mueve el punto de corte pero no garantiza mostrar todo) — la solución real es la propiedad **`RibbonPageGroup.AllowTextClipping = false`**, que evita el recorte. Se aplicó a los 14 grupos de `MainForm.Designer.cs` (todas las pestañas), regla dura desde ahora para cualquier grupo nuevo del Ribbon.


## Regla dura: los folios se calculan con `MAX(Folio)+1`, nunca con `SEQUENCE`

Petición del usuario: si se captura el Lote `0000026` y luego se elimina, el siguiente Lote debe volver a ser `0000026`, no saltar al `0000027`. Con `SEQUENCE` eso es imposible — una secuencia jamás devuelve un valor ya entregado, aunque la fila que lo usaba se haya borrado.

Los **4 folios del proyecto** (`Acopio.AcuerdoCorte`, `Acopio.OrdenCorte`, `Recepcion.RecepcionFruta`, `Lotes.Lote`) pasaron de `NEXT VALUE FOR <seq>` a calcular el folio desde la propia tabla, dentro de la **misma transacción** del `INSERT`:

```sql
SET XACT_ABORT ON;
DECLARE @Folio NVARCHAR(7);
BEGIN TRANSACTION;
    SELECT @Folio = RIGHT('0000000' + CAST(ISNULL(MAX(TRY_CAST(Folio AS INT)), 0) + 1 AS VARCHAR(7)), 7)
    FROM <Tabla> WITH (UPDLOCK, HOLDLOCK);
    INSERT INTO <Tabla> (Folio, ...) VALUES (@Folio, ...);
COMMIT TRANSACTION;
SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
```

Comportamiento resultante (verificado contra la BD real):
- Se borra el **último** folio → se reutiliza. ✅ (lo que pidió el usuario)
- Se borra uno **intermedio** → el hueco **no** se rellena (`MAX` no cambia). A propósito: rellenar huecos intermedios rompería el orden cronológico del folio.
- Se **vacía** la tabla → vuelve a arrancar en `0000001` solo, sin resetear nada a mano.

**Por qué el `UPDLOCK, HOLDLOCK` no es opcional:** sin él, dos capturas simultáneas leen el mismo `MAX` y calculan el mismo folio. Ese hint toma un lock de rango que serializa a la segunda hasta que la primera hace `COMMIT`. El índice `UNIQUE` de `Folio` (existe en las 4 tablas) queda como red de seguridad. `XACT_ABORT ON` garantiza que cualquier error haga rollback y no deje la transacción abierta.

Archivos: `Database/Lotes/011_SP_Lote_Folio_Reutilizable.sql`, `Database/Recepcion/012_SP_RecepcionFruta_Folio_Reutilizable.sql`, `Database/Acopio/031_SP_Folio_Reutilizable.sql`. Cada uno redefine **solo** el `_Insertar` correspondiente (es el único que genera folio), copiando el cuerpo verbatim de la definición vigente.

**Las 4 `SEQUENCE` quedan sin uso pero NO se eliminan**: archivos viejos del repo (`Lotes/002`, `Recepcion/002-006`, `Acopio/011-013-023`) todavía las referencian, y borrarlas rompería un replay completo en una base nueva. Se verificó con `sys.sql_expression_dependencies` que ningún SP vivo depende ya de ellas.

**Cómo verificar que un cambio así no rompió nada**: antes de redefinir SPs grandes, capturar la firma de parámetros (`sys.parameters`) y compararla después con `diff`. Dapper mapea por nombre de propiedad → nombre de parámetro, así que un typo no lo detecta el compilador y sale hasta runtime. En este cambio se compararon las 71 firmas de los 4 SPs y salieron idénticas.

⚠️ **Ojo al verificar SPs con `OBJECT_DEFINITION(...) LIKE '%texto%'`**: el texto de los **comentarios** también cuenta. En este cambio, buscar `'%NEXT VALUE FOR%'` dio falsos positivos en 2 SPs porque el comentario decía "en vez de NEXT VALUE FOR". Para saber de qué depende realmente un SP, usar `sys.sql_expression_dependencies`, no búsqueda de texto.


## `Database/Utilidades/Limpiar_Datos_Operativos.sql` — dejar el movimiento en cero conservando catálogos

Script nuevo, complementario a `Inicializar_Datos_Produccion.sql` (que vacía **todo**, incluidos catálogos, y es de un solo uso antes de salir a producción). Éste está pensado para **limpiar pruebas durante el desarrollo** sin tener que volver a capturar productores, huertas y precios.

- **Borra** (en orden de FK, hijos antes que padres): `Almacenes.MovimientoCajaCampo`, `Produccion.Corrida`, `Lotes.LoteRecepcion`, `Lotes.Lote`, `Recepcion.RecepcionFrutaOrdenCorte`, `Recepcion.RecepcionFruta`, `Acopio.OrdenCorte`, `Acopio.AcuerdoCorte`.
- **Conserva**: todo `Catalogos.*`, las listas de precios (`Acopio.ListaPrecioFruta`/`ListaPrecioCorte`, `Acarreo.ListaPrecioAcarreo`/`Zona`), los catálogos de apoyo de Acopio, `Configuracion.*`, todo `Seguridad.*` y **`Auditoria.Registro`** (decisión explícita del usuario: la bitácora se queda como historial porque incluye las cargas masivas de catálogos, no solo pruebas).
- Usa `DELETE` + `DBCC CHECKIDENT (..., RESEED, 0)` en vez de `TRUNCATE`, porque `TRUNCATE` no se permite en tablas referenciadas por FK y no queremos desactivar constraints (menos riesgo). Todo dentro de `TRY/CATCH` con `XACT_ABORT ON`, así un error hace rollback completo.
- **No toca ninguna `SEQUENCE`**: con los folios en `MAX+1`, vaciar la tabla ya los reinicia solos.
- Es idempotente, se puede correr las veces que haga falta.
