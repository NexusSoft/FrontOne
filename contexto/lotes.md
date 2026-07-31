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
- `Database/Seguridad/024_Seed_Modulo_Lotes.sql`: módulo `Lotes` nuevo, pantalla `Lotes`, permisos completos Administrador — clon de `022_Seed_Modulo_Recepcion.sql`.
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
