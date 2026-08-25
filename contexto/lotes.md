# Módulo Lotes (schema `Lotes`) — Conformación de Lotes

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo. Reglas fundacionales viven en [arquitectura.md](arquitectura.md).

## Módulo "Lotes" (schema `Lotes`, nuevo — página de Ribbon propia junto a Recepción)

Proceso siguiente al de Recepción de Fruta: un **Lote** agrupa una o varias Recepciones ya capturadas (`Recepcion.RecepcionFruta`) para conformar un embarque. El usuario mandó capturas de un sistema de referencia (listado "Conformación de Lotes" + diálogo "Actualizar registro") y precisó las reglas de negocio por escrito, incluyendo el descifrado colaborativo de la fórmula del campo "Referencia" (ver abajo).

**Decisiones de negocio confirmadas con el usuario:**
- Un Lote puede tomar **varias Recepciones**, pero todas deben compartir: mismo proveedor de "Pagar el Corte a" (`Acopio.OrdenCorte.PagarCorteACardCode`), misma Huerta (`HuertaId`) y mismo Acuerdo de Corte (`AcuerdoCorteId`) — validado contra la primera Orden de Corte de cada Recepción (`Recepcion.RecepcionFrutaOrdenCorte`, `CROSS APPLY TOP 1 ORDER BY Id`).
- Columna "Tickets" del listado = **número (COUNT)** de Recepciones que componen el Lote, no la lista de folios de ticket concatenados (se construyó primero como `STRING_AGG`, siguiendo el mismo patrón que "Huertas" en el listado de Recepciones, pero el usuario corrigió tras probar en la app real: quiere el conteo — ver iteración abajo).
- Una misma Recepción **no puede estar en dos Lotes** — `UNIQUE` en `Lotes.LoteRecepcion.RecepcionFrutaId` (no solo validación de aplicación).
- **Una vez que una Recepción está en un Lote, se bloquea su edición por completo** — `RecepcionFrutaService.ActualizarAsync` valida `ILoteRepository.RecepcionEstaEnLoteAsync` al inicio y lanza `ValidationException` si ya está tomada; hay que quitarla del Lote primero (`sp_Lote_Eliminar` o borrar la línea) para poder volver a editarla.
- `Kilogramos` del encabezado = suma automática (en vivo, en la UI) del `PesoNeto` de las Recepciones agregadas — nunca se captura a mano.
- `Personalizado`: texto libre genérico, sin lógica especial todavía.
- `Estatus` (0=Pendiente/1=Procesado/2=Error en Conformación en el modelo de datos): sin lógica de negocio real todavía, todo Lote nuevo queda en **Pendiente** — se define en una iteración futura.
- Botones "Imprimir Conformación del Lote"/"Imprimir Etiquetas" del mockup: **fuera de alcance** de esta iteración.
- Pestañas del listado de referencia (por Fecha/por Línea de Producción, etc.): esta iteración construye **un solo listado** (patrón `RecepcionesFrutaForm`, con el buscador estándar del proyecto) — quedan pendientes para después.
- Columna "No. de Registro" del mockup: sin significado identificado, se omitió.

## Fórmula de "Referencia" (folio juliano, 11 dígitos, auto-generado server-side)

Reverse-engineered en vivo con el usuario a partir de 2 ejemplos reales de un sistema legado (folio 162/163 de 2026 y folio 1 de 2024):

```
089    → fijo, código de la empresa (se repite igual todos los años)
D1     → primer dígito (centena) del día juliano del año de la Fecha del Lote, formateado "000"
FFFFF  → Folio del Lote (el mismo consecutivo de Lotes.SeqLoteFolio), a 5 dígitos con ceros a la izquierda
D2D3   → los otros 2 dígitos (decena + unidad) del día juliano
```

Verificado: 30/07/2026 → día juliano 211 → D1="2", D2D3="11". Folio=162 → "00162" → `089`+`2`+`00162`+`11` = `08920016211` (coincide exacto con la captura del usuario). También: 19/11/2024 → día juliano 324, folio 1 → `089`+`3`+`00001`+`24` = `08930000124`. Implementada en `LoteService.CalcularReferencia(DateTime fecha, string folio)` — código de empresa `"089"` hardcoded como constante (`CodigoEmpresaReferencia`); si cambiara habría que tocar código, mismo criterio que otros códigos SAP hardcoded del proyecto.

**Detalle de implementación — problema del huevo y la gallina Folio/Referencia:** el `Folio` del Lote se genera DENTRO del `INSERT` (vía `Lotes.SeqLoteFolio`, mismo patrón que Recepción/Acopio), pero la fórmula de `Referencia` lo necesita. Se resolvió insertando primero con `Referencia = NULL` y completando con un `UPDATE` inmediato en la misma llamada a `LoteService.CrearAsync` en cuanto se conoce el Folio — por eso `Lotes.Lote.Referencia` es `NVARCHAR(11) NULL` (SQL Server permite varios `NULL` bajo un índice `UNIQUE`, a diferencia de varias cadenas vacías que sí chocarían si dos inserts concurrentes usaran un placeholder fijo). `sp_Lote_Obtener` usa `ISNULL(Referencia, '')` para que el DTO nunca vea `null`.

## Base de datos

- `Database/Catalogos/026_Schema_LineaProduccion.sql` + `027_SP_LineaProduccion.sql` + `028_Seed_LineaProduccion.sql`: catálogo nuevo `Catalogos.LineaProduccion` (Id/Nombre/Activo), clon exacto del patrón `Acopio.Variedad`, sembrado con `CANADA`/`ORGANICO`/`USA` (`VALUES` + `WHERE NOT EXISTS`, patrón de `010_Seed_Producto.sql`). Pantalla Seguridad `LineasProduccion` en `029_Seed_Pantalla_LineasProduccion.sql` (módulo `Catalogos` existente).
- `Database/Lotes/001_Schema_Lote.sql` (schema nuevo): `CREATE SCHEMA Lotes`, `SEQUENCE Lotes.SeqLoteFolio` (folio 7 dígitos). `Lotes.Lote` (encabezado, `Referencia NVARCHAR(11) NULL` — ver detalle arriba) y `Lotes.LoteRecepcion` (detalle, `RecepcionFrutaId UNIQUE` + FK a `Recepcion.RecepcionFruta`, ambas FK sin `CASCADE`).
- `002_SP_Lote.sql`: CRUD del encabezado. `sp_Lote_Obtener` trae `Tickets` (`COUNT(*)` de líneas del Lote — ver iteración abajo) y `HuertaNombre`/`ProductorNombre` (de la primera Recepción del Lote, vía `OUTER APPLY TOP 1` — son iguales en todas las líneas por la validación de compatibilidad). `sp_Lote_Eliminar` borra detalle y encabezado en el mismo SP, liberando las Recepciones de vuelta.
- `003_SP_LoteRecepcion.sql`: CRUD del detalle — sin `Actualizar` (nada editable por línea, mismo criterio que `Recepcion.RecepcionFrutaOrdenCorte`... salvo que ahí sí hay `Kilogramos`; aquí ni eso). `Obtener` hace join a `Recepcion.RecepcionFruta` para Folio/Ticket/Fecha/COPREF-BICO/PesoNeto/%MateriaSeca.
- `004_SP_RecepcionFruta_DisponiblesParaLote.sql`: viven en schema `Lotes` (no `Recepcion`) porque son específicos de este flujo — mismo criterio que `Recepcion.sp_OrdenCorte_ObtenerTop100ParaRecepcion` vive en `Acopio`. `sp_RecepcionFruta_ObtenerTop100ParaLote`/`sp_RecepcionFruta_BuscarParaLote` (patrón Top100+Buscar) excluyen Recepciones ya en cualquier Lote y aceptan `@HuertaId`/`@AcuerdoCorteId`/`@PagarCorteACardCode` opcionales (NULL si el Lote todavía no tiene líneas) para filtrar solo compatibles. `sp_RecepcionFruta_ObtenerParaLote(@RecepcionFrutaId)` trae la Huerta/Acuerdo/Proveedor de UNA Recepción puntual — usado por `LoteService.AgregarLineaAsync` para validar compatibilidad server-side (defensa en profundidad: el picker ya filtra en SQL, pero el Service no confía ciegamente en la UI).
- `005_SP_RecepcionFruta_EstaEnLote.sql`: `sp_RecepcionFruta_EstaEnLote(@RecepcionFrutaId)` — usado por `RecepcionFrutaService.ActualizarAsync` para el bloqueo de edición.
- `Database/Seguridad/029_Seed_Modulo_Lotes.sql` (renombrado de `024_...` al fusionar con `origin/main` — el número 024 chocó con `024_Schema_ReportePermiso.sql` que el equipo agregó en paralelo): módulo `Lotes` nuevo, pantalla `Lotes`, permisos completos Administrador — clon de `022_Seed_Modulo_Recepcion.sql`.
- `Inicializar_Datos_Produccion.sql`: se agregaron `Lotes.LoteRecepcion`/`Lotes.Lote`/`Catalogos.LineaProduccion` al arreglo `@Tablas` (detalle antes que encabezado) y el reinicio de `Lotes.SeqLoteFolio`.

## Capas C#

Patrón estándar del proyecto: `Lote`/`LoteRecepcion`/`LineaProduccion` entities, `LoteDto`/`LoteRecepcionDto`/`LineaProduccionDto`/`RecepcionDisponibleParaLoteDto` (DTOs), `ILoteRepository`/`LoteRepository`, `ILineaProduccionRepository`/`LineaProduccionRepository`. `IRecepcionFrutaRepository`/`RecepcionFrutaRepository` se extendieron con `ObtenerTop100ParaLoteAsync`/`BuscarParaLoteAsync`/`ObtenerParaLotePorIdAsync` (nueva entidad ligera `RecepcionDisponibleParaLote`, mismo criterio que `OrdenCorteDisponible` del módulo Recepción).

`LoteService` (auditoría completa Crear/Actualizar/Eliminar, módulo `"Lotes"`): `CrearAsync`/`ActualizarAsync` (calculan/preservan `Referencia`), `AgregarLineaAsync(loteId, recepcionFrutaId)` (valida disponibilidad + compatibilidad Huerta/Acuerdo/Proveedor contra la primera línea ya agregada, `ValidationException` clara), `EliminarLineaAsync`, `ObtenerRecepcionesDisponiblesTop100Async`/`BuscarRecepcionesDisponiblesAsync` (para el picker). `LineaProduccionService` es clon exacto de `VariedadService`.

`RecepcionFrutaService` ganó una nueva dependencia de constructor `ILoteRepository` — `ActualizarAsync` ahora empieza validando `RecepcionEstaEnLoteAsync` antes de tocar nada.

## WinForms

- `Forms/Catalogos/LineasProduccionForm` + `LineaProduccionEditarForm`: clon exacto de `VariedadesForm`/`VariedadEditarForm` (mismos íconos, mismo patrón).
- `Forms/Lotes/` (carpeta nueva): `LotesForm` (listado: Folio/Referencia/Tickets/Línea de Producción/Fecha/Huerta/Peso Neto/Materia Seca/Productor, `BestFitColumns` + ajuste de columna sobrante como en `RecepcionesFrutaForm`); `LoteEditarForm` (Folio/Referencia readonly "se genera al guardar", Fecha, Observaciones, Kilogramos readonly recalculado en vivo, Personalizado, `LookUpEdit` de Línea de Producción con NullText+Combo+Plus abriendo `LineasProduccionForm`, % Materia Seca readonly recalculado en vivo (promedio simple del % Materia Seca de las Recepciones agregadas — pedido explícito del usuario, ya no se captura a mano), grid de detalle solo lectura con Nuevo/Borrar — patrón "todo se persiste hasta el Guardar" con `BindingList<FilaDetalleLote>`, igual que `RecepcionFrutaEditarForm`); `SeleccionarRecepcionForm` (picker TOP100+Buscar, patrón `SeleccionarOrdenCorteForm`, recibe Huerta/Acuerdo/Proveedor opcionales para filtrar a compatibles cuando el Lote ya tiene líneas).
- Ribbon: nueva `RibbonPage _pageLotes` ("Lotes") junto a `_pageRecepcion`, grupo "Conformación de Lotes" con botón `_btnLotes` (Id=28). Nuevo grupo `_grpCatalogosLotes` ("Lotes") en la página **Catálogos** con el botón "Líneas de Producción" (`_btnLineasProduccion`, Id=29) — mismo precedente que Acopio/Acarreo (catálogos chicos de un módulo cuelgan de la página Catálogos). `_ribbon.MaxItemId` subió de 27 a 29.

## Verificado end-to-end contra la BD real (`172.16.1.100\FrontOne`)

Se insertó un Lote de prueba con una Recepción real ya existente (Folio `0000007`, ticket `asd`), se confirmó que `Referencia` calculada a mano coincide con `LoteService.CalcularReferencia`, que `sp_Lote_Obtener` trae `Tickets`/`HuertaNombre`/`ProductorNombre` correctamente vía los joins, que `sp_RecepcionFruta_EstaEnLote` devuelve `1` mientras está en el Lote y que la Recepción desaparece de `sp_RecepcionFruta_ObtenerTop100ParaLote` mientras está tomada. Se confirmó que `sp_Lote_Eliminar` borra detalle+encabezado y libera la Recepción de vuelta (`EstaEnLote` vuelve a `0`, vuelve a aparecer en el picker). Los datos de prueba se limpiaron por completo (no queda ningún Lote en la base). UI no se probó visualmente en este entorno (sin acceso a Visual Studio/pantalla) — el usuario prueba el flujo completo desde la app.

## Iteración: bugfix del picker + "No. de Lote" en Recepción se llena al guardar el Lote

Dos ajustes pedidos por el usuario tras probar el módulo en la app real:

- **Bug**: `Lotes.sp_RecepcionFruta_ObtenerTop100ParaLote`/`BuscarParaLote` nunca trajeron `PorcentajeMateriaSeca` ni `CoprefBico` en su `SELECT`, así que al agregar una Recepción al Lote esas dos columnas del grid de detalle quedaban en `0`/vacío aunque la Recepción sí tuviera esos datos capturados. Corregido agregando ambas columnas a los 3 SPs del picker (`ObtenerTop100ParaLote`/`BuscarParaLote`/`ObtenerParaLote`), a `RecepcionDisponibleParaLote`/`RecepcionDisponibleParaLoteDto`, y al mapeo en `LoteEditarForm.BtnDetalleNuevo_Click` (antes solo copiaba Folio/Ticket/Fecha/PesoNeto/Huerta/Acuerdo/Proveedor de la fila seleccionada).
- **Nuevo**: al guardar el Lote, cada Recepción que queda incluida recibe en su campo `Recepcion.RecepcionFruta.NoLote` (columna que ya existía sin uso — ver [`contexto/recepcion.md`](recepcion.md)) el **Folio del Lote** (7 dígitos, no la Referencia juliana — decisión explícita del usuario). Al quitar una línea del Lote o borrar el Lote completo, `NoLote` se limpia de vuelta a `NULL`.
  - `Recepcion.sp_RecepcionFruta_ActualizarNoLote(@Id, @NoLote)` (`Database/Lotes/006_SP_RecepcionFruta_ActualizarNoLote.sql`): update ligero dedicado, no pasa por `sp_RecepcionFruta_Actualizar` completo.
  - `Lotes.sp_LoteRecepcion_Eliminar` ahora hace `DELETE ... OUTPUT DELETED.RecepcionFrutaId` para que `LoteService.EliminarLineaAsync` sepa a qué Recepción limpiarle el `NoLote`.
  - `Lotes.sp_Lote_Eliminar` limpia el `NoLote` de **todas** las Recepciones del Lote (`UPDATE ... FROM ... INNER JOIN Lotes.LoteRecepcion`) antes de borrar detalle+encabezado — necesario porque ahí se borran varias líneas de golpe, no una por una vía `EliminarLineaAsync`.
  - `IRecepcionFrutaRepository.ActualizarNoLoteAsync(id, noLote)` nuevo; `ILoteRepository.EliminarDetalleAsync` cambió su firma de `Task` a `Task<int?>` (devuelve el `RecepcionFrutaId` liberado).
  - `LoteService.AgregarLineaAsync`, después de insertar la línea, relee el Lote (`ObtenerAsync(loteId)`) para tomar su `Folio` y llama `ActualizarNoLoteAsync`. `LoteService.EliminarLineaAsync` usa el `RecepcionFrutaId` que devuelve `EliminarDetalleAsync` para limpiar.

## Iteración: columna "Tickets" del listado pasó de lista de folios a conteo

El usuario probó el listado en la app real y reportó que "Tickets" mostraba mal la información — tras confirmar con captura de pantalla, resultó que el diseño original (`STRING_AGG` de `NumeroTicket`) sí funcionaba correctamente a nivel SQL (verificado con una prueba directa contra la BD real usando el mismo `LoteRepository.ObtenerAsync` que usa la app), pero el usuario en realidad quiere que la columna muestre el **número de Recepciones/tickets** del Lote (ej. "1", "2"), no el texto concatenado de folios. Cambio:
- `Lotes.sp_Lote_Obtener`: el `SELECT` de `Tickets` pasó de `STRING_AGG(rf.NumeroTicket, ', ')` a `COUNT(*)` sobre `Lotes.LoteRecepcion`.
- `Lote.Tickets` (entity) y `LoteDto.Tickets`: tipo cambió de `string?` a `int`.
- `LoteEditarForm.BtnGuardar_Click`: el `LoteDto` que arma para Crear/Actualizar pasa `0` en vez de `null` para `Tickets` (ya no es nullable).

## Iteración: "Referencia" se extiende de 11 a 16 dígitos — ahora incluye el Id de la Huerta

El usuario descubrió que la `Referencia` que calculamos es exactamente el valor que él captura a mano en el AI `(10)` (Batch/Lot Number) de una etiqueta de exportación con código de barras **GS1-128** (mockup real de etiqueta de caja, con AI `(01)` GTIN + `(13)` fecha de empaque + `(10)` lote). El estándar GS1-128 permite hasta 20 caracteres alfanuméricos en el AI(10) — con los 11 dígitos originales sobraba margen, así que se aprovechó para agregar más trazabilidad.

**Decisión (confirmada con el usuario, quien propuso el orden final):** como todo Lote garantiza una única Huerta entre sus Recepciones (regla de negocio ya existente — ver arriba), se agrega el **Id de la Huerta** a la fórmula. Nueva fórmula, 16 dígitos:

```
089 (3, fijo, código de empresa) + HuertaId (5, con ceros a la izquierda)
  + Folio del Lote (5) + día juliano de la Fecha (3, ya no partido en dos como el formato original)
```

Verificado con una prueba real contra la BD (`LoteRepository`/`LoteService.CrearAsync` reales, Lote de prueba creado y borrado en el mismo test): Huerta 82303, Folio 18, Fecha 31/07/2026 (día juliano 212) → `0898230300018212`, coincide exacto con la fórmula.

**Se evaluaron y se descartaron por ahora** (quedan documentados por si se retoman): agregar Línea de Producción (1 dígito, CANADA/ORGÁNICO/USA) y/o Año (2 dígitos) — el usuario decidió que con Huerta+Folio+Juliano es suficiente. Quedan **4 dígitos de margen** (16 de 20) por si se necesita algo más adelante.

**Problema nuevo que introduce esta fórmula — orden de las operaciones al guardar:** a diferencia del Folio (que se genera dentro del propio `INSERT` vía secuencia), la **Huerta no vive en el encabezado del Lote** — se conoce solo a través de sus líneas (Recepciones). Antes de este cambio, `LoteService.CrearAsync(LoteDto)` no necesitaba saber nada de las líneas para calcular la Referencia. Ahora sí. Solución:
- `LoteService.CrearAsync` ganó un parámetro obligatorio `int huertaId` — lanza `ValidationException` si es `<= 0` ("Agrega al menos una Recepción antes de guardar el Lote — la Referencia necesita saber de qué Huerta es.").
- `LoteEditarForm.BtnGuardar_Click`: agregó una validación explícita — si es un Lote nuevo (`_loteExistente is null`) y `_filas.Count == 0`, no deja guardar (mensaje informativo, ni siquiera intenta llamar al servicio). Si hay al menos una línea, toma `_filas[0].HuertaId` (ya disponible en memoria desde que se seleccionó la Recepción en el picker, sin necesidad de otro viaje a la base) y se lo pasa a `CrearAsync`.
- **Antes de esta iteración no existía ningún límite de "mínimo 1 Recepción por Lote"** — técnicamente se podía guardar un Lote vacío. Ahora es un requisito real, derivado de que la Referencia ya no se puede calcular sin Huerta.
- `LoteService.CalcularReferencia` ganó el parámetro `huertaId` y cambió de orden interno: antes partía el día juliano en dos mitades (una antes y otra después del Folio, por ser así como venía del sistema legado); ahora el juliano completo (3 dígitos) va al final, después del Folio y la Huerta.

**Bug encontrado y corregido durante la verificación**: los parámetros `@Referencia NVARCHAR(11)` de `Lotes.sp_Lote_Insertar`/`sp_Lote_Actualizar` (`Database/Lotes/002_SP_Lote.sql`) truncaban el valor nuevo de 16 caracteres a 11 sin error visible — SQL Server trunca silenciosamente un parámetro `NVARCHAR(n)` más corto que el valor recibido, no lanza excepción. Se detectó con una prueba end-to-end real (crear Lote → leer Referencia guardada) que regresó `08982303000` en vez de `0898230300018212`. Corregido ampliando ambos parámetros a `NVARCHAR(16)`. **Lección para el proyecto**: cuando se amplía el ancho de una columna (`ALTER TABLE ... ALTER COLUMN`), hay que revisar también los parámetros `NVARCHAR(n)` de los SPs que la alimentan — no basta con cambiar la columna, cada SP tiene su propio ancho declarado y trunca en silencio si no coincide.

**Base de datos**: `Database/Lotes/008_Alter_Lote_Referencia_Huerta.sql` — `ALTER TABLE Lotes.Lote ALTER COLUMN Referencia NVARCHAR(16) NULL` (los Lotes ya creados con el formato viejo de 11 dígitos NO se recalculan, se quedan como están). `002_SP_Lote.sql` actualizado con los parámetros `@Referencia` a 16.

Build 0 errores, `dotnet test` en verde, cambio desplegado y verificado end-to-end contra `172.16.1.100\FrontOne` (Lote de prueba creado y eliminado, sin dejar datos huérfanos). UI no probada visualmente en este entorno — pendiente que el usuario confirme en la app que al guardar un Lote nuevo sin ninguna Recepción agregada, ahora se bloquea con el mensaje correcto, y que la Referencia mostrada en `LoteEditarForm` para un Lote nuevo con Recepciones ya trae los 16 dígitos.

## Iteración: renombrado de "Referencia" a "Código de Trazabilidad"

El usuario pidió renombrar el campo porque "Referencia" era ambiguo (se confundía con el `Folio` del Lote). Nombre elegido: **`CodigoTrazabilidad`** (C#/SQL) / **"Código de Trazabilidad"** (UI) — describe con precisión que es el valor de rastreo que alimenta el AI(10) del código de barras GS1-128.

Rename aplicado en todas las capas:
- **SQL**: `Lotes.Lote.Referencia` → `CodigoTrazabilidad` vía `sp_rename` (`Database/Lotes/009_Rename_Lote_Referencia_a_CodigoTrazabilidad.sql`, renombra columna y constraint `UQ_Lotes_Lote_Referencia` → `UQ_Lotes_Lote_CodigoTrazabilidad`, sin tocar datos). `001_Schema_Lote.sql`/`002_SP_Lote.sql` actualizados con el nombre nuevo para que una instalación limpia desde cero ya nazca con el nombre correcto (`008_Alter_Lote_Referencia_Huerta.sql` se dejó tal cual, es un paso histórico ya ejecutado bajo el nombre viejo, válido para ese momento).
- **Domain**: `Lote.CodigoTrazabilidad` (entity), `LoteDto.CodigoTrazabilidad`.
- **Application**: `LoteService.CalcularReferencia` → `CalcularCodigoTrazabilidad`; constante `CodigoEmpresaReferencia` → `CodigoEmpresaTrazabilidad`.
- **WinForms**: `LoteEditarForm._txtReferencia`/`_lblReferencia` → `_txtCodigoTrazabilidad`/`_lblCodigoTrazabilidad`, texto de la etiqueta "Referencia:" → "Código de Trazabilidad:" (se ensanchó el label/textbox en el mismo renglón para que quepa el texto más largo, sin mover el resto del formulario). `LotesForm` agregó caption explícito "Código de Trazabilidad" a la columna del listado (antes no tenía caption propio, mostraba el nombre de la propiedad tal cual).

Verificado contra la BD real: rename aplicado sin pérdida de datos — los Lotes con formato viejo de 11 dígitos se quedaron intactos, y un Lote creado por el usuario en la app real durante esta misma sesión (con la fórmula nueva de 16 dígitos, formato `089`+Huerta+Folio+Juliano) también se conservó correctamente bajo el nombre de columna nuevo. Build 0 errores, `dotnet test` en verde.

## Iteración: columna "Tickets" renombrada a "Recepciones"

Mismo motivo que el rename de Referencia — nombre más claro para lo que ya se había convertido en un conteo (ver iteración anterior "columna Tickets del listado pasó de lista de folios a conteo"). Cambio de nombre en las 4 capas: `Lotes.sp_Lote_Obtener` (`AS Tickets` → `AS Recepciones`), `Lote.Recepciones`/`LoteDto.Recepciones` (antes `Tickets`), `LotesForm` con caption explícito "Recepciones" en la columna del listado (antes sin caption propio). No es columna persistida (se calcula con `COUNT(*)` en el SP), así que no hizo falta ningún `sp_rename` en base de datos — solo redesplegar el SP.

Verificado contra la BD real: `sp_Lote_Obtener` ya regresa la columna `Recepciones` con el valor correcto. Build 0 errores, `dotnet test` en verde.

## Iteración: una Recepción solo entra a un Lote si su camión ya está destarado

Regla nueva pedida por el usuario: *"si en recepcion no esta desatarado no se puede crear el lote"*. Mientras el camión no se destara falta la pesada en vacío, así que los kilos y el conteo de cajas de esa Recepción todavía pueden cambiar — no tiene caso conformar un Lote con ella. Es el mismo flag (`Recepcion.RecepcionFruta.CamionDestarado`) que dispara el paso de la caja de campo a la cuenta `Produccion` del Almacén (ver `contexto/almacenes.md`, iteración 4).

`Database/Lotes/010_SP_RecepcionFruta_DisponiblesParaLote_Destarado.sql` redefine los **3** SPs de `004_SP_RecepcionFruta_DisponiblesParaLote.sql` (no solo los 2 que cambian) para ser la única "última palabra" — mismo criterio que `Database/Recepcion/011`, así un replay en orden numérico en una BD nueva termina con la versión correcta sin depender de cuál corrió al final:

- `sp_RecepcionFruta_ObtenerTop100ParaLote` y `sp_RecepcionFruta_BuscarParaLote`: `+ AND rf.CamionDestarado = 1`. El picker simplemente deja de mostrar las no destaradas.
- `sp_RecepcionFruta_ObtenerParaLote` (búsqueda por Id, usada para validar): **selecciona** `rf.CamionDestarado` pero **no filtra** por él, a propósito — así `LoteService` puede distinguir entre "esa Recepción no existe" y "existe pero le falta destarar", y dar el mensaje correcto en cada caso.

`RecepcionDisponibleParaLote` gana `CamionDestarado` (la entidad; el DTO del picker no lo necesita porque el SP ya filtra). `LoteService.AgregarLineaAsync` valida el flag antes que la disponibilidad y lanza `ValidationException` nombrando el folio: *"La Recepción '0000020' todavía no tiene el camión destarado. Marca "Camión destarado" en la Recepción antes de agregarla a un Lote."* — defensa en profundidad, igual que las validaciones de compatibilidad Huerta/Acuerdo/Proveedor que ya estaban.

Verificado contra la BD real: de 3 Recepciones (12 y 13 destaradas, 14 no), el filtro aislado devuelve exactamente 12 y 13. El picker completo devuelve 0 porque las 3 ya están en un Lote — filtro preexistente (`NOT EXISTS ... Lotes.LoteRecepcion`), no relacionado con este cambio. Build 0 errores.

## Iteración: Variedad promovida a campo duro del encabezado (`Lotes.Lote.VariedadId`)

El usuario pidió mostrar la Variedad en el módulo de Lotes. Al preguntarle por el alcance aclaró el motivo real: la Variedad va a ser la llave que en el futuro módulo de **Costos** (precios a la banda) vincule cada Lote a una única Lista de Precio (`Acopio.AcuerdoCorte.ListaPrecioNumero`: Convencional/Orgánico/Nacional) — por eso no basta con mostrarla derivada al vuelo (como Huerta/Productor), pidió persistirla como campo duro de solo lectura en el encabezado.

También explicó que la razón detrás de su pregunta sobre "el mismo número de Acuerdo" es la misma: dentro de un Lote la Variedad no puede variar entre líneas, porque el Lote completo va a apuntar a una sola Lista de Precio. **Esto ya estaba garantizado sin ningún cambio**: `LoteService.AgregarLineaAsync` ya rechaza agregar una Recepción cuyo `AcuerdoCorteId` no coincida con el de la primera línea del Lote (junto con Huerta y Proveedor) — y como cada `Acopio.AcuerdoCorte` tiene una única `VariedadId`, esa regla (caso QA 8.2) ya es más estricta que "misma Variedad". No se agregó ninguna validación nueva en `AgregarLineaAsync`, sería redundante.

**A diferencia de Huerta/Productor** (que siguen derivándose al vuelo en `sp_Lote_Obtener` vía `OUTER APPLY` sobre la primera Recepción, nunca se persisten), `VariedadId` sí se guarda en `Lotes.Lote` porque el módulo de Costos la va a necesitar sin tener que recorrer Recepciones cada vez. Se fija una sola vez al crear el Lote y nunca se recalcula en un `Actualizar` — mismo criterio que `CodigoTrazabilidad`. A diferencia de `CodigoTrazabilidad` (que depende del Folio generado por la secuencia dentro del propio `INSERT`, y por eso necesita el truco de insertar en `NULL` y completar con `UPDATE`), `VariedadId` no tiene ese problema de huevo-gallina: ya se conoce desde que el usuario agrega la primera línea en el grid (viene de la Orden de Corte de esa Recepción), así que se manda directo en el `INSERT`.

**Base de datos:**
- `Database/Lotes/013_Alter_Lote_VariedadId.sql`: `ALTER TABLE Lotes.Lote ADD VariedadId INT NULL` + `FK_Lotes_Lote_Variedad` a `Acopio.Variedad(Id)` (sin `CASCADE`). Incluye backfill para los Lotes ya existentes, derivando la Variedad de la Orden de Corte de su primera Recepción (misma cadena que ya usaba `sp_Lote_Obtener` para Huerta/Productor).
- `002_SP_Lote.sql`: `sp_Lote_Obtener` agrega `VariedadId`/`VariedadNombre` (join directo a `Acopio.Variedad`, ya no hace falta pasar por Recepciones). `sp_Lote_Insertar`/`sp_Lote_Actualizar` ganan el parámetro `@VariedadId`.
- `Database/Lotes/014_SP_RecepcionFruta_DisponiblesParaLote_Variedad.sql`: redefine los 3 SPs del picker (`ObtenerTop100ParaLote`/`ObtenerParaLote`/`BuscarParaLote`, ahora la última palabra de estos 3) agregando `VariedadId`/`VariedadNombre` de la Orden de Corte — necesario para que el formulario conozca la Variedad de la Recepción **antes** de guardar el Lote, cuando `Lotes.Lote.VariedadId` todavía no existe.

**Capas C#:** `Lote`/`LoteDto` ganan `VariedadId`/`VariedadNombre`. `RecepcionDisponibleParaLote`/`RecepcionDisponibleParaLoteDto` también (vienen del picker). `LoteService.CrearAsync` gana el parámetro `variedadId` (mismo guard que `huertaId`: `ValidationException` si es `<= 0`) y asigna `entidad.VariedadId` antes de insertar; `ActualizarAsync` lo preserva del registro anterior, igual que `CodigoTrazabilidad`. `FilaDetalleLote` (WinForms) gana `VariedadId`/`VariedadNombre` solo para que `BtnGuardar_Click` pueda leer `_filas[0].VariedadId` al crear el Lote — mismo rol que ya cumplía `HuertaId` ahí.

**WinForms:** `LoteEditarForm` gana el campo de solo lectura "Variedad:" (`_txtVariedad`) entre Kilogramos/% Materia Seca y Personalizado — mismo texto placeholder "(se genera al guardar)" que Folio/Código de Trazabilidad mientras el Lote es nuevo. `LotesForm` agrega la columna "Variedad" al listado, junto a Huerta.

**Fuera de alcance de esta iteración:** mostrar Variedad como columna en el picker `SeleccionarRecepcionForm` — ese grid hoy ni siquiera muestra el folio del Acuerdo; se puede agregar después si hace falta.

Verificado contra la BD real (`172.16.1.100\FrontOne`, la única de las dos bases de prueba donde el módulo Lotes está desplegado — `LIDER-TI` no tiene el schema `Lotes`): el backfill dejó `VariedadId=2` (Convencional) en los 3 Lotes reales ya existentes, coincidiendo exacto contra la Variedad de la Orden de Corte de su primera Recepción. Se creó un Lote de prueba (`sp_Lote_Insertar` con `@VariedadId=2`), se confirmó que `sp_Lote_Obtener` lo trae correcto, que un `sp_Lote_Actualizar` posterior (cambiando Observaciones) preserva `VariedadId`, y se borró el Lote de prueba (`sp_Lote_Eliminar`) sin dejar datos huérfanos. `dotnet build` 0 errores, `dotnet test` en verde. UI no se probó visualmente en este entorno (sin acceso a Visual Studio/pantalla) — pendiente que el usuario confirme desde la app real que el campo "Variedad" aparece de solo lectura en `LoteEditarForm` y como columna en `LotesForm`.
