# Módulo Embarques — Logística → Pedidos (solo lectura de SAP)

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo.

## Decisiones de negocio de fondo

- **Pedidos se capturan solo en SAP B1** (entidad `Orders` del Service Layer) — FrontOne únicamente los consulta, nunca los crea, edita ni elimina. No hay tabla propia ni SP en SQL Server: es passthrough directo a SAP, mismo espíritu que `ISapItemRepository`/`ISapProveedorRepository`.
- Listado principal: **Top 500 más recientes** (`$orderby=DocEntry desc&$top=500`, siguiendo `odata.nextLink` porque Service Layer pagina de a 20). Sin buscador ni filtros de fecha/cliente en esta primera iteración — se agrega cuando alguien necesite acotar por cliente o rango de fechas.
- Doble clic en el listado abre el detalle maestro-detalle del pedido (`Orders(docEntry)`, trae `DocumentLines` incluidas de forma nativa).
- `DocumentStatus` de SAP (`bost_Open`/`bost_Close`/`bost_Cancel`/`bost_Delivered`) se traduce a español en el repositorio (`SapPedidoRepository.TraducirEstatus`), no en la UI.
- **Folio Fronterra** es un campo de usuario (UDF) del encabezado del pedido en SAP (`U_FolioFronterra`) — se trae vía `$select` en el listado y directo en el detalle (`Orders(docEntry)` ya lo incluye), sin `$expand` porque los UDF de documento aparecen como propiedad plana del recurso.
- **Vendedor se muestra como código (`SalesPersonCode`), no como nombre** — resolver el nombre exige una llamada extra a `SalesPersons` por pedido; no se hizo hasta que alguien lo pida.
- Es el primer submódulo de la pestaña `Embarques` del Ribbon (grupo **"Logística"**) — la pestaña ya existía vacía desde antes.

## Capas C#

- `FrontOne.Domain/DTOs/SapPedidoDto.cs` — `SapPedidoDto` (renglón de listado), `SapPedidoLineaDto` (línea de detalle: Código, Descripción, Cantidad, Precio Unitario, Total, Almacén), `SapPedidoDetalleDto` (encabezado completo + `Lineas`).
- `FrontOne.Domain/Interfaces/ISapPedidoRepository.cs` → `FrontOne.Infrastructure.SapB1/Repositories/SapPedidoRepository.cs` (`ObtenerTop500Async`, `ObtenerPorDocEntryAsync`), modelos JSON en `Models/SapOrdersResponse.cs`.
- `FrontOne.Application/Services/PedidoService.cs` — servicio delgado, sin validador (no hay escritura).
- `FrontOne.WinForms/Forms/Embarques/PedidosForm.cs` (listado, molde `ProductosTerminadosForm`) y `PedidoDetalleForm.cs` (maestro-detalle read-only, `TextEdit`/`MemoEdit` con `Properties.ReadOnly = true`, sin botones Guardar/Cancelar — solo Cerrar).

## Base de Datos

Sin tablas nuevas — solo permisos: `Database/Seguridad/050_Seed_Modulo_Embarques.sql` (módulo `Embarques`, pantalla `Pedidos`, permisos completos al rol Administrador). No se agrega nada a `Inicializar_Datos_Produccion.sql` porque no hay tabla operativa propia.

## Ver también

[[produccion]] (Pallet/PalletDetalle, estatus del pallet), [[reempaques]] (excluye pallets Embarcados de su buscador de origen).

---

## Contenedor — surtido de pedidos con pallets físicos (2026-09-03)

Segundo submódulo de `Embarques` (grupo Logística del Ribbon, botón **Contenedor**). Ata un pedido
de venta **abierto** en SAP con los pallets ya armados en Producción que lo surten.

### Decisiones de negocio confirmadas con el usuario

- Un contenedor = **un** pedido SAP. El pedido se elige de un buscador (TOP 100 pedidos abiertos,
  `PedidoService.ObtenerAbiertosAsync` → `ISapPedidoRepository.ObtenerAbiertosAsync`, `$filter=
  DocumentStatus eq 'bost_Open'`) y queda fijo desde que se guarda el encabezado por primera vez —
  no se puede reasignar después. Cliente/pedido se guardan como **snapshot** en `Embarques.
  Contenedor` (mismo criterio que `PalletDetalle.CajasPorPallet`): si el pedido cambia después en
  SAP, el contenedor ya guardado no se altera.
- **Pallets elegibles**: `Estatus IN (3 Completo, 4 Excedido, 5 Empacado, 7 En Proceso)`, no Neutro,
  no Reempacado, no ya asignado a otro contenedor (mixtos incluidos). Al agregarlos al contenedor
  pasan a **Estatus 8 "Embarcado"** (nuevo, ver `Database/Produccion/026_Alter_Pallet_Embarcado.sql`)
  — con esto `sp_Reempaque_ObtenerPalletsOrigenDisponibles` los excluye automáticamente del buscador
  de Reempaques. Al quitarlos del contenedor regresan a Estatus 5 Empacado.
- Cajas/Kilogramos de **todo** el módulo (grid de pallets, resumen, y el Status Pendiente/Surtido
  del Tab Pedido) salen de `Produccion.PalletDetalle` real, nunca de una conversión teórica —
  excepto la columna "Pallet" del Tab Pedido, que sí es la conversión teórica
  `CantidadCajas / ProductoTerminado.CajasPorPallet` (informativa, cuántos pallets completos
  representa el pedido).
- "Calibre de Exportación" del resumen = `Catalogos.ProductoTerminado.CalibreCodigoExterno`.
- Posición y Temperatura del pallet dentro del contenedor se capturan en un diálogo propio
  (`ContenedorPalletAgregarForm`) al agregarlo — no hay edición en línea en el grid.
- Los cuatro grids del módulo (Pedido, Pallets, Detalle del Pallet, Resumen) llevan
  `OptionsView.ShowFooter = true` con suma en las columnas numéricas y **no** llevan
  `OptionsFind.AlwaysVisible` — excepción documentada a la regla dura de CLAUDE.md, porque son
  grids de resumen/totales, no listados que se busquen por texto.

### Base de Datos

`Database/Embarques/001_Schema_SP_Contenedor.sql` (schema `Embarques` nuevo):
`Embarques.Contenedor` / `Embarques.ContenedorPallet` (folio consecutivo de 7 dígitos,
`Embarques.SeqContenedorFolio`) + `sp_Contenedor_{Obtener,Insertar,Actualizar,Eliminar,
ObtenerPallets,ObtenerResumen,ObtenerSurtido,ObtenerPalletsDisponibles,AgregarPallet,QuitarPallet}`.

`Database/Produccion/026_Alter_Pallet_Embarcado.sql`: documenta el Estatus 8 y actualiza
`sp_Reempaque_ObtenerPalletsOrigenDisponibles`/`sp_Reempaque_AgregarPalletOrigen` para excluir/
rechazar pallets ya Embarcados.

`Database/Seguridad/051_Seed_Pantalla_Contenedores.sql`: pantalla `Contenedores` bajo el módulo
`Embarques` ya existente, permisos completos al rol Administrador.

`Embarques.Contenedor`/`Embarques.ContenedorPallet` sí se agregaron a `@Tablas` de
`Database/Utilidades/Inicializar_Datos_Produccion.sql` (son tablas operativas), con su
`ALTER SEQUENCE Embarques.SeqContenedorFolio RESTART WITH 1` al final.

### Capas C#

- `FrontOne.Domain/DTOs/ContenedorDto.cs` — `ContenedorDto`, `ContenedorPalletDto`,
  `ContenedorResumenCalibreDto`, `ContenedorSurtidoDto`, `ContenedorPedidoLineaDto`,
  `PalletDisponibleEmbarqueDto`.
- `FrontOne.Domain/Interfaces/IContenedorRepository.cs` →
  `FrontOne.Infrastructure.SqlServer/Repositories/ContenedorRepository.cs`.
- `ISapPedidoRepository`/`SapPedidoRepository` ganaron `ObtenerAbiertosAsync` (comparte el loop de
  paginación de `ObtenerTop500Async` vía un helper privado con `$filter` opcional).
- `FrontOne.Application/Services/ContenedorService.cs` — con auditoría (Crear/Modificar/Eliminar,
  módulo `"Embarques"`, mismo patrón que `ReempaqueService`). `ObtenerLineasPedidoAsync` combina
  las líneas de SAP con `ProductoTerminadoService.ObtenerAsync()` (resuelve CajasPorPallet/
  Presentacion por CodigoSap) y `ObtenerSurtidoAsync` (cajas/kilos ya embarcados) para armar
  `ContenedorPedidoLineaDto` con el Status Pendiente/Surtido.

### WinForms

`FrontOne.WinForms/Forms/Embarques/`: `ContenedoresForm` (listado, molde `ReempaquesForm`),
`ContenedorEditarForm` (Tab Pedido + Tab Embarque con `SplitContainerControl` anidados: pallets a
la izquierda, detalle del pallet seleccionado arriba-derecha —reusa `PalletService.
ObtenerDetalleAsync`, sin SP propio—, resumen por calibre abajo-derecha), `ContenedorPedidoBuscarForm`
(picker de pedidos SAP abiertos) y `ContenedorPalletAgregarForm` (buscador de pallets + Posición/
Temperatura en un solo diálogo). `PalletsForm.NombreEstatus` ganó el caso `8 => "Embarcado"`.

Ribbon: botón **Contenedor** agregado al grupo Logística (`_grpLogistica`) en `MainForm`, junto a
Pedidos.

### Ajustes posteriores al build inicial (2026-09-04)

- **Fix Dapper**: `sp_Contenedor_ObtenerPallets` regresaba `NoRegistro` como `bigint` (`ROW_NUMBER()`
  sin cast) contra un DTO `int` — `InvalidOperationException` al abrir/guardar un contenedor. Se
  cambió a `CAST(ROW_NUMBER() ... AS INT)`.
- **Layout Tab Embarque**: reordenado a 3 secciones lado a lado/apiladas para calzar con el mockup
  del usuario — `_splitPrincipal.Horizontal = true` (Panel1 pallets a la izquierda, Panel2
  `_splitDerecho`) y `_splitDerecho.Horizontal = false` (Panel1 detalle arriba, Panel2 resumen
  abajo). Ojo: en `SplitContainerControl` de DevExpress, `Horizontal = true` es paneles lado a
  lado, no apilados — al revés de lo intuitivo.
- Los botones Agregar/Eliminar Pallet del grid izquierdo van en un `Panel` propio
  (`_pnlBotonesPallets`, `Dock = Bottom`) agregado **después** del grid (`Dock = Fill`) a la misma
  colección de controles — un `Anchor = Bottom` a mano contra un contenedor de alto variable los
  dejaba fuera del área visible.
- **Filtro de pallets disponibles por producto pendiente**: `sp_Contenedor_ObtenerPalletsDisponibles`
  ganó `@CodigosSap NVARCHAR(MAX)` (CSV de `CodigoSap`, vía `STRING_SPLIT`) — al abrir
  "Agregar Pallet" desde el Tab Embarque, solo se listan pallets de los productos del pedido que
  **no** llegaron a 100% surtido (`ContenedorEditarForm.BtnAgregarPallet_Click` calcula la lista
  contra `_lineasPedido` en memoria). Los pallets Mixtos siempre se incluyen (no hay una sola
  columna de producto que comparar contra el CSV).
- Columna **% Surtido** agregada al grid del Tab Pedido (`ContenedorPedidoLineaDto.PorcentajeSurtido`,
  `cajasSurtidas / CantidadCajas * 100`, calculada en `ContenedorService.ObtenerLineasPedidoAsync`).
- **Validación de posición duplicada**: no se puede agregar dos pallets con la misma Posición dentro
  del mismo contenedor. Se valida en dos capas — `ContenedorPalletAgregarForm.BtnGuardar_Click`
  (contra la lista de posiciones ya ocupadas, pasada desde `ContenedorEditarForm`, para que el
  diálogo de captura no se cierre antes de mostrar el error) y `sp_Contenedor_AgregarPallet` como
  respaldo servidor.
- Temperatura del pallet se captura y muestra en **°F** (antes °C), rango -80 a 140.
- Botones del módulo migrados al nuevo estándar de 28px de alto (ver "Estándar de botones CRUD" en
  `CLAUDE.md`) — antes 22/23px, el ícono quedaba recortado.

### Reportes de Carga de Contenedor (2026-09-04)

7 reportes nuevos pedidos por el usuario con layouts de referencia exactos (imágenes): **Carga de
Contenedor**, **Detalle por Lote**, **Detalle por Huerta**, **Resumen por Calibre**, **Resumen por
Lote**, **Resumen por Huerta** y **Resumen por Huerta (sin Kg)**. Se agregan como combo (`_cmbReporte`,
`LookUpEdit` con lista estática en memoria, no catálogo) + botón `_btnImprimirReporte` debajo de
`_tabs` en `ContenedorEditarForm` (visible en ambos tabs, `Anchor = Bottom|Left`, a la misma altura
que `_btnCerrar`). Ambos controles solo se habilitan cuando **todas** las líneas del pedido llegan a
100% de surtido (`_lineasPedido.All(l => l.PorcentajeSurtido >= 100m)`, recalculado en
`ActualizarEstadoReporte()` cada vez que se recarga `CargarLineasPedidoAsync`).

**Una sola SP de datos, no siete**: `Embarques.sp_Contenedor_ObtenerCargaParaReporte(@ContenedorId)`
(`Database/Embarques/002_SP_Contenedor_ObtenerCargaParaReporte.sql`) regresa el detalle plano
(una fila por combinación pallet+línea de lote) con todos los campos que necesitan los 7 layouts.
Cada `XtraReport` agrupa o pre-agrega esa misma lista con lo que le hace falta — los 3 reportes de
detalle (Carga/DetalleLote/DetalleHuerta) usan `DetailReportBand` con `GroupHeaderBand`/
`GroupFooterBand` (agrupando por Posición de pallet, Lote o Huerta); los 4 de resumen
(Calibre/Lote/Huerta/HuertaSinKg) pre-agregan con LINQ `GroupBy`+`Sum` en `CargarDatos` antes de
asignar `_detailReportBand.DataSource`, y usan un `ReportFooterBand` **anidado dentro del
`DetailReportBand`** (no en el reporte raíz) para el "Total General" — `Sum([Campo])` ahí resuelve
correctamente contra el `DataSource` propio del `DetailReportBand`, sin exponer un total
precalculado en el encabezado.

**Huerta no cuelga de Lote ni de PalletDetalle directo.** La cadena real (confirmada contra la BD
viva, no solo el `.sql` de schema — ver nota de `MunicipioId` abajo) es
`Produccion.PalletDetalle.LoteId → Lotes.Lote → Lotes.LoteRecepcion → Recepcion.RecepcionFrutaOrdenCorte
→ Acopio.OrdenCorte.HuertaId → Catalogos.Huerta` (mismo patrón que
`Produccion.sp_Pallet_ObtenerEtiquetaSagarpaPorDetalle`), con `OUTER APPLY TOP 1` por si un lote
tuviera más de una recepción/huerta.

**Ojo con `contexto`/`.sql` de schema desactualizado vs. BD viva**: `011_Schema_Huerta.sql` describe
`Huerta.Municipio` como `NVARCHAR(100)` plano, pero la base real ya tiene `Huerta.MunicipioId` (FK a
`Catalogos.Municipio`, migración posterior no reflejada en ese script) — la SP nueva hace
`LEFT JOIN Catalogos.Municipio` para resolver el nombre. Si se vuelve a tocar algo de Huerta, no
confiar ciegamente en el `.sql` de schema más viejo sin verificar contra la BD.

**Mapeo de columnas del mockup a datos reales** (para no repetir la investigación si se ajusta el
layout): "SAGARPA" = `Huerta.RegistroSagarpa` (ya incluye el prefijo `HUE...`); "GlobalGAP" =
`Huerta.NumeroGlobalGap` si `CertificadoGlobalGap = 1`; "Presentación" tipo "11.3 KG" = calculado
`FORMAT(pt.PesoNeto, 'N1') + ' KG'`, no el enum `PresentacionProducto`; "Producto" = `DescripcionSap`
tal cual; "Marca" = `Catalogos.Marca` vía `ProductoTerminado.MarcaId`; "Calibre" =
`CalibreCodigoExterno` (mismo campo que ya usa `sp_Contenedor_ObtenerResumen`); "°F" =
`Embarques.ContenedorPallet.Temperatura`. **"TAG 000000000"** no existe como dato real en ningún
lado del esquema — se renderiza como texto fijo en el layout (no se inventó una columna nueva,
criterio YAGNI; si el negocio confirma que hace falta un Tag real capturable, se agrega después).

Registro de los 7 códigos nuevos (mismo patrón que cualquier reporte del proyecto, 3 puntos en
código): `FrontOne.Domain/Constants/ReportesDisponibles.cs`, el switch de
`FrontOne.WinForms/Reports/CatalogoReportes.ObtenerFactory`, y los dos switches de
`FrontOne.WinForms/Forms/Sistema/ReportesForm.cs` (`ConectarOrigenDatos`/`DesconectarOrigenDatos`).
**No se sembró permiso de reporte para Administrador** — siguiendo el precedente ya establecido
desde que se agregaron Pallet/Incidencias/ProcesoLote/LiquidacionProductor (solo
`RecepcionFruta` tiene seed en `026_Seed_ReportePermiso_Administrador.sql`, comentario explícito de
que los reportes agregados después se conceden manualmente desde "Permisos de Reportes").

Clases nuevas en `FrontOne.WinForms/Reports/`: `ReporteContenedorComun.cs` (helper compartido —
`VistaEncabezadoContenedor`, membrete, bloque "Orden de Empaque/Fecha/Pedido/Cliente`, helpers de
columna/celda — evita repetir la construcción del membrete en los 7 archivos),
`ReporteContenedorCarga`, `ReporteContenedorDetallePorLote`, `ReporteContenedorDetallePorHuerta`,
`ReporteContenedorResumenCalibre`, `ReporteContenedorResumenLote`, `ReporteContenedorResumenHuerta`,
`ReporteContenedorResumenHuertaSinKg`. `ContenedorEditarForm` ganó dependencias nuevas en su
constructor (`EmpresaConfiguracionService`, `SessionContext`) para armar/mostrar el reporte —
`ContenedoresForm`/`MainForm.BtnContenedores_ItemClick` las reenvían.

Pendiente de verificación manual en la app (no se pudo probar clic a clic desde esta sesión): abrir
`ContenedorEditarForm` con un pedido <100% y confirmar que el combo/botón quedan deshabilitados;
imprimir los 7 reportes con datos reales y comparar totales contra los grids ya existentes del Tab
Embarque. La SP nueva sí se probó contra datos reales (`ContenedorId = 1`) y regresa filas correctas.

### Ajuste visual del picker de Pedido SAP (2026-09-05)

`_txtPedidoSap` pasó de `TextEdit` + `SimpleButton` "..." (`_btnBuscarPedido`) a un `ButtonEdit`
con `ButtonPredefines.Search` (mismo patrón que `_cmbProductor` en `AcuerdoCorteEditarForm`) —
`NullValuePrompt = "Buscar pedido SAP..."`, `ReadOnly = true`, wireado a `ButtonClick`
(`TxtPedidoSap_ButtonClick`, filtra `e.Button.Kind == ButtonPredefines.Search`). El habilitar/
deshabilitar la búsqueda (pedido fijo tras guardar) ahora se hace sobre
`_txtPedidoSap.Properties.Buttons[0].Enabled`, no sobre un botón aparte. `_cmbReporte` (combo de
reportes de Carga de Contenedor) ganó `Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo))`
explícito para que la flecha de despliegue se vea sin depender del comportamiento automático de
DevExpress cuando `Buttons.Count == 0`.

### Selección múltiple en "Agregar Pallet al Contenedor" (2026-09-05)

`ContenedorPalletAgregarForm` ganó `_gridView.OptionsSelection.MultiSelect = true` +
`MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect` — se pueden marcar varios pallets del
buscador y agregarlos todos con un solo "Guardar", en vez de repetir el diálogo uno por uno.
`PalletIdSeleccionado`/`Posicion` (singulares) se reemplazaron por
`PalletsSeleccionados: IReadOnlyList<(int PalletId, int Posicion)>` — cada pallet marcado toma la
siguiente posición libre a partir de la "Posición inicial" capturada (salta las ya ocupadas,
incluidas las que se van asignando dentro del mismo lote), la Temperatura capturada aplica igual
a todos. `ContenedorEditarForm.BtnAgregarPallet_Click` ahora hace un `foreach` llamando
`AgregarPalletAsync` por cada pallet del lote — si falla a la mitad, igual refresca los grids
antes de mostrar el error (algunos ya quedaron agregados). Formulario ensanchado de 674 a 950px
para que el grid respire con las columnas existentes.

### Posición/Temperatura editables directo en el grid del Tab Embarque (2026-09-05)

`Embarques.sp_Contenedor_ActualizarPallet(@ContenedorPalletId, @Posicion, @Temperatura)` (nuevo,
`Database/Embarques/003_SP_Contenedor_ActualizarPallet.sql`, desplegado y probado) — misma
validación de posición única que `sp_Contenedor_AgregarPallet`, excluyendo el propio renglón.
`IContenedorRepository.ActualizarPalletAsync` → `ContenedorService.ActualizarPalletAsync`
(con auditoría Modificar, mismo patrón que `AgregarPalletAsync`).

`_gridViewPallets` (grid izquierdo del Tab Embarque) pasó a `OptionsBehavior.Editable = true`,
pero solo las columnas Posición y Temperatura quedan editables
(`OptionsColumn.AllowEdit = false` explícito en el resto — No. Registro, No. Pallet, Cajas,
Kilogramos) — con `RepositoryItemSpinEdit` propios (`_repoSpinPosicionPallet` 1-9999 entero,
`_repoSpinTemperaturaPallet` -80 a 140 con 2 decimales, mismos rangos que
`ContenedorPalletAgregarForm`). El guardado es inmediato al cambiar la celda
(`GridViewPallets_CellValueChanged`, mismo patrón que `GastoLoteForm.GridViewFruta_CellValueChanged`)
— para la columna que cambió se usa `e.Value` (valor ya confirmado), para la otra el valor actual
de `fila` (no se asume que `ContenedorPalletDto`, un record con propiedades `init`, quede mutado
por la edición del grid). Si el SP rechaza (posición duplicada), se avisa y se recarga el grid con
los valores reales de BD.
