# Módulo Producción (schema `Produccion`) — Corridas, Pallets, Etiquetado

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo. Reglas fundacionales viven en [arquitectura.md](arquitectura.md).

Proceso siguiente al de [Lotes](lotes.md): una vez que un Lote está conformado, entra a producción para convertirse en Pallets de producto terminado. Cubre 3 sub-módulos que comparten schema `Produccion`: **Corridas** (control del proceso de un Lote), **Pallets** (armado físico de tarimas) y **Etiquetado** (impresión GS1/Sagarpa de lo que se armó).

## Corridas (`Produccion.Corrida`)

Registra cuándo un Lote entra y termina su proceso de producción — un Lote solo puede tener **una** Corrida (Iniciar Proceso / Finalizar Corrida). `KilosAProcesar` es un snapshot del `Kilogramos` del Lote al iniciar; `KilosProcesados` se va acumulando con los Pallets que se le van armando encima (el mecanismo real de acumulación vive en Pallets, ver abajo). No se puede eliminar una Corrida ya Procesada o con `KilosProcesados > 0`. `sp_Corrida_Finalizar` exige `KilosProcesados == KilosAProcesar` exacto antes de dejar cerrar (mismo check duplicado en el cliente antes de llamar al servidor).

**Peso Factor** (columna calculada, base del futuro módulo de Ajuste de Lotes): fórmula real del sistema legado Fronterra (`Calcula_Factor_Update`), no la fórmula simple ingenua que se intentó primero. Separa:
- **Exportación** = líneas de detalle tipo Caja de todos los Pallets de la Corrida (siempre valor teórico, peso estándar × cajas).
- **Nacional** = líneas tipo Granel (siempre peso real capturado).
- Resta la **Merma** capturada vía Pallet Neutro (ver abajo) del teórico de Exportación antes de comparar.
- Sin líneas Caja el factor es `1.000000` (todo ya se pesó real en báscula, nada que corregir).
- Líneas **Diferencia a Favor** quedan fuera del cálculo.

Todo vive en `sp_Corrida_Obtener` — sin tocar capas .NET ni UI.

`SqlRepositoryBase` propaga el mensaje de negocio de un `THROW 50000` en vez de envolverlo genérico (para que errores como "Kilos Procesados no coincide" lleguen legibles a la UI).

## Pallets (`Produccion.Pallet` / `Produccion.PalletDetalle`)

Maestro-detalle de armado de tarimas contra un Lote **En Proceso**; cada línea de detalle consume `Kilogramos` de `Produccion.Corrida.KilosProcesados` — es el mecanismo que le da vida a `KilosProcesados` (Corridas por sí sola no lo llena).

**Estatus calculado** (no capturado a mano): Vacío / Incompleto / Completo / Excedido / Empacado / Reempacado / **En Proceso** (7, agregado con el redisño de Reempaques — ver [reempaques.md](reempaques.md)). Reglas clave:
- Pallet **no mixto** (un solo producto/lote) llega a Completo solo si su producto es Caja (objetivo de cajas cumplido) — un producto **Granel** no tiene tope de kilogramos, así que nunca debía marcarse "Completo" solo por tener algo capturado; eso es justo lo que motivó el estatus 7 "En Proceso" (antes un granel se volvía "Completo" con la primera línea y desaparecía de flujos que buscan pallets abiertos, como el buscador de destino de Reempaques).
- Pallet **mixto** se rotula "PALLET MIXTO" (encabezado y grid principal) y se puede bloquear sin restricción de estatus; uno no-mixto exige Completo para bloquear.
- % Materia Seca del encabezado pondera por **Kilogramos** (no por Cajas) — válido también para pallets mixtos con líneas Caja+Granel combinadas.
- Al agregar una línea con mismo Lote+Producto que una ya existente, las cajas se **suman** en vez de duplicar el renglón.
- No hay validación de saldo insuficiente del Lote al capturar detalle (el peso estándar del producto no siempre coincide con el peso real recepcionado — la diferencia se ajusta con Pallet Neutro / futuro módulo de Ajuste de Lotes, no bloqueando la captura).
- `Produccion.Pallet.FechaModificacion` + `sp_Pallet_ObtenerUltimaModificacion`: soporte de polling cada 5s en WinForms (`PalletsForm`, botón "Todo actualizado"/"Actualizar") y Android (silencioso) para que varios dispositivos armando pallets en paralelo se enteren de cambios ajenos sin pisarse.

**Pallet Neutro** (`sp_Pallet_CrearNeutro`, columna `EsNeutro=1`, folio `"0-{LoteFolio}"`): ajusta `Corrida.KilosProcesados` con un monto positivo (**Merma**) o negativo (**Diferencia a Favor**) para cerrar Kilos Restantes en 0 y poder Finalizar la Corrida — botón "Diferencia" al final del grid de `CorridasForm`. Usa productos SAP dedicados `MERMA`/`DIFERENCIA PESO A FAVOR` (por eso el `LookUpEdit` de Producto del encabezado de Pallet debe poder mostrar un producto ya asignado aunque esté inactivo en SAP). Los Pallets Neutro nunca entran a Reempaques (ver [reempaques.md](reempaques.md)).

**Sincronización SAP de Productos Terminados — grupos PT y ST**: `ProductoTerminadoService.SincronizarConSapAsync` traía solo grupo SAP "PT" (Producto Terminado); se extendió a traer también "ST" (Semiterminado) para cubrir productos de reempaque. Columna `GrupoSap` visible en el listado, se actualiza automáticamente si un producto ya sincronizado cambia de grupo en SAP. `IProductoTerminadoRepository.ActualizarDatosSapAsync` ganó el parámetro `GrupoSap` (firma + implementación + SP `sp_ProductoTerminado_ActualizarDatosSap` extendidos, migración `044`).

**Producto Terminado — Presentación Caja/Granel**: campo del encabezado, Granel bloquea/limpia Peso Estándar y Cajas por Pallet (forzado también en servidor, no solo UI). Columna "Datos Capturados" (check de solo lectura) en el listado para ver de un vistazo qué productos ya tienen su información de negocio completa tras sincronizar con SAP — apoyada por la plantilla `Database/ImportarProductosTerminados_FrontOne.xlsx` de importación masiva.

**Báscula**: módulo Sistema > Configuración de Báscula (puerto COM/baud rate/parity, parser genérico configurable) + botón "Tomar Lectura" en el encabezado de Pallet.

**Bug de infraestructura — `QUOTED_IDENTIFIER`**: `sp_PalletDetalle_InsertarDesdeReempaque` (y otras DML sobre `PalletDetalle`) fallaban por configuración `QUOTED_IDENTIFIER` inconsistente en el SP. Corregido en migración `024` (`Database/Produccion/006_Alter_Pallet_BloquearSoloCompleto.sql` en adelante todo SP de este schema debe crearse con `SET QUOTED_IDENTIFIER ON` explícito antes del `CREATE PROCEDURE`, igual que el resto del proyecto).

## Etiquetado (schema `Etiquetado`, integrado a Pallet)

Catálogo `Etiquetado.Etiqueta` (soft-delete, wizard de alta, duplicado) reutiliza el `DisenadorReporteForm` generalizado (mismo Diseñador de Reportes que el resto del proyecto — ver la regla dura "todo reporte nuevo declara su origen de datos" en [`CLAUDE.md`](../CLAUDE.md) y el skill `reporte-designer-integracion`). 3 tipos de etiqueta por línea de Pallet: **Caja**, **Pallet** (papeleta de encabezado) y **Sagarpa** (Registro Sagarpa) — form compartido `PalletImprimirEtiquetaForm` con selector de etiqueta/impresora instalada e impresión directa sin diálogo.

- **GS1-128** `(01)GTIN(13)Fecha(10)CódigoTrazabilidad` y **VoiceCode** (algoritmo PTI/GS1, `VoicePickCodeCalculator` en `FrontOne.Shared`) se calculan y **persisten a nivel de línea de detalle** del Pallet al capturarla/editarla (no se recalculan cada vez que se imprime). `PalletService` los recalcula en cada modificación de línea (agregar/editar/eliminar) y al Bloquear el Pallet, para que una línea que quedó en blanco por falta de GTIN/Código de Trazabilidad se autocorrija en cuanto el catálogo se completa — sin correr el SP manualmente (`sp_PalletDetalle_RecalcularGs1128Masivo`/`_ObtenerParaRecalcularVoiceCode`, `@PalletId` opcional).
- `XRBarcodeControl` (TEC-IT) requiere `BoundingRectangleF`/`Dpi` explícitos para respetar su tamaño real en el reporte, y registra `CampoDato` vía `ExpressionBindingDescriptor.SetPropertyDescription` para que el Expression Editor del Diseñador lo detecte como bindeable — sin esto solo mostraba las propiedades nativas de `XRPictureBox`. La licencia TEC-IT debe cargarse también fuera del Diseñador (antes solo cargaba ahí, y todo reporte fuera de él salía "Demo").
- Al conectar el origen de datos para diseño (`ConectarOrigenDatos`, ver patrón en [`CLAUDE.md`](../CLAUDE.md)), enlazar explícitamente `DataSource`/`DataMember` a la consulta principal de cada tipo — si no, arrastrar un campo del Field List genera `[NombreConsulta].[Campo]` en vez de `[Campo]` plano, y esa ruta con prefijo nunca resuelve en la vista previa/impresión real. Tipo Pallet usa un SP de diseño combinado (encabezado+empresa en una sola fila) para que también salga plano.
- El texto de Status de la etiqueta usa el catálogo real de estatus del Pallet (`PalletsForm.NombreEstatus`), no un binario Completo/Incompleto.
- Logo USDA Organic agregado a `Configuracion.Empresa`.

## FrontOne.Web

Listado de Lotes de Producción agregado al sitio (SP, entidad, DTO, repositorio, servicio, página), siguiendo el mismo patrón de Catálogos documentado en la sección "Convenciones de `FrontOne.Web`" de [`CLAUDE.md`](../CLAUDE.md) — sin lógica propia adicional, es solo consulta/listado.

## Ver también

- [reempaques.md](reempaques.md) — desarmar un Pallet ya armado y reconstruir uno o más nuevos sin perder trazabilidad; vive sobre `Produccion.PalletDetalle`, no en tablas propias.
- [gastos.md](gastos.md) — liquidación de costos de Fruta/Cosecha/Acarreo, consume la Corrida ya finalizada de un Lote.
