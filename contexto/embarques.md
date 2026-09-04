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
