# Módulo Almacenes (schema `Almacenes`) — control de inventario de Caja de Campo

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo. Reglas fundacionales viven en [arquitectura.md](arquitectura.md).

## Proceso de negocio (explicado por el usuario, verbatim resumido)

El catálogo "Caja de Campo" (`Catalogos.CajaCampo`: ROJA/AZUL/BLANCA/AMARILLA) necesita llevar control de existencias. Proceso real:

1. Hay una existencia de cajas por color en el almacén/empaque.
2. Al crear una **Orden de Corte** se manda cierta cantidad de cajas vacías al campo con la cuadrilla — **salida** del almacén.
3. Al día siguiente, cuando llega el camión con la fruta a la **Recepción de Fruta**, las cajas regresan — **entrada** al almacén. Si no regresan todas, la diferencia son **cajas perdidas**, y ese faltante se refleja en el inventario.
4. Un **dashboard** (`AlmacenCajaCampoDashboardForm`) muestra, por color: existencia actual y cajas perdidas del mes — y desde ahí se registra una compra de cajas nuevas o un ajuste manual.

## Decisiones de negocio confirmadas

- Nuevo combo **"Color de Caja"** (`CajaCampoId`) en `OrdenCorteEditarForm`, con NullText+Combo+Plus (abre `CajasCampoForm`). La cantidad que sale es la que ya existía, `CajasEntregadas` (combo 300/400/500).
- En Recepción, **"Cajas Perdidas" es un campo nuevo y separado** de "Diferencia" — el usuario rechazó explícitamente reusar/renombrar "Diferencia" ("Necesito un campo aparte").
- Tras aclarar el significado real de cada campo de control de cajas en Recepción (Por Entregar = viene de la Orden de Corte; Entregadas = lo que realmente salió con la cuadrilla; Cortadas = volvió con fruta; Vacías = volvió sin fruta):
  - `CajasDiferencia` (cambia de fórmula): `CajasPorEntregar − CajasEntregadas` → ¿salió del almacén lo que la Orden de Corte comprometía?
  - `CajasPerdidas` (nuevo campo): `CajasEntregadas − CajasCortadas − CajasRecibidasVacias` → de lo que salió con la cuadrilla, ¿cuánto no volvió en ninguna forma? Este es el que dispara el ajuste de inventario.
- Los movimientos automáticos (salida al crear/editar Orden de Corte, entrada al crear/editar/agregar-línea de Recepción) se corrigen solos con el patrón **"borra y vuelve a insertar"**: en cada `Crear`/`Actualizar`/`Eliminar` se llama primero `EliminarMovimientosCajaCampoPorOrigenAsync(origenModulo, origenId)` y luego (si aplica) se inserta el movimiento recalculado — nunca se calculan deltas, más simple y sin riesgo de desfase.
- "Existencia" y "cuánta caja hay en el empaque" son el mismo número (`SUM(Entrada) - SUM(Salida)`) — no hay una segunda cifra separada.

## Base de datos

- `Database/Acopio/030_Alter_OrdenCorte_CajaCampo.sql`: agrega `CajaCampoId INT NULL` a `Acopio.OrdenCorte` + FK a `Catalogos.CajaCampo` (sin `CASCADE`, regla dura). Nullable para no romper órdenes ya existentes; `OrdenCorteService.ResolverYValidarAsync` exige el campo para órdenes nuevas vía validación de aplicación, no vía `NOT NULL` de BD.
- `Database/Acopio/023_SP_OrdenCorte.sql` (modificado): `sp_OrdenCorte_Obtener`/`_Insertar`/`_Actualizar` ganan `CajaCampoId`/`CajaCampoNombre` (join a `Catalogos.CajaCampo`).
- `Database/Recepcion/010_Alter_RecepcionFruta_CajasPerdidas.sql`: agrega `CajasPerdidas SMALLINT NOT NULL DEFAULT (0)` a `Recepcion.RecepcionFruta`.
- `Database/Recepcion/011_SP_RecepcionFruta_CajasPerdidas.sql`: **redefinición consolidada** de `sp_RecepcionFruta_Obtener`/`_Insertar`/`_Actualizar`. Necesaria porque se detectó que `002_SP_RecepcionFruta.sql` (trae `Huertas`/`OrdenCorteFolio`/`AcuerdoCorteFolio`/`EstaEnLote` en el Obtener) y `006_Alter_RecepcionFruta_CajasPorEntregar.sql` (trae `CajasPorEntregar`/`TicketPesadaArchivo` en Insertar/Actualizar) definían versiones incompletas y mutuamente inconsistentes de los mismos 3 SPs — en un fresh-install replay en orden numérico, 006 corre después de 002 y pisaría el Obtener completo con uno viejo. `011_...` es ahora la versión completa y definitiva (todas las columnas de ambos archivos + `CajasPerdidas` nuevo), y corrige ese bug latente de paso.
- `Database/Almacenes/001_Schema_MovimientoCajaCampo.sql` (schema nuevo `Almacenes`): tabla `Almacenes.MovimientoCajaCampo` (`Id`, `Fecha DATE`, `CajaCampoId` FK a `Catalogos.CajaCampo`, `TipoMovimiento` CHECK `Entrada`/`Salida`, `Cantidad SMALLINT` siempre positivo — el signo lo da `TipoMovimiento`, `OrigenModulo` CHECK `OrdenCorte`/`Recepcion`/`Manual`, `OrigenId INT NULL` referencia lógica sin FK dura — cruzar `OrdenCorte`/`RecepcionFruta` con FK complicaría el borrado del registro origen sin aportar nada, el movimiento histórico no debe desaparecer si se borra el origen —, `Observaciones`, `Usuario`, `FechaCreacion`). Índice en `(OrigenModulo, OrigenId)`.
- `Database/Almacenes/002_SP_MovimientoCajaCampo.sql`: `sp_MovimientoCajaCampo_Insertar`, `sp_MovimientoCajaCampo_EliminarPorOrigen(@OrigenModulo, @OrigenId)`, `sp_MovimientoCajaCampo_ObtenerExistencias` (`SUM(CASE Entrada +Cantidad / Salida -Cantidad)` agrupado por color, solo colores `Activo=1`), `sp_MovimientoCajaCampo_ObtenerPerdidaMes(@Anio, @Mes)` (`SUM(RecepcionFruta.CajasPerdidas)` vía join `RecepcionFrutaOrdenCorte` → `OrdenCorte.CajaCampoId`, filtrado por año/mes de `RecepcionFruta.Fecha`).
- `Database/Seguridad/033_Seed_Modulo_Almacenes.sql`: módulo `Almacenes` nuevo + pantalla `AlmacenCajaCampo`, permisos completos para rol Administrador (mismo patrón que `029_Seed_Modulo_Lotes.sql`).
- `Inicializar_Datos_Produccion.sql`: agregado `Almacenes.MovimientoCajaCampo` al arreglo `@Tablas`.

**Nota sobre `sp_MovimientoCajaCampo_ObtenerPerdidaMes`**: si una Recepción llegara a tener más de una línea de Orden de Corte (hoy el UI solo permite una, ver `contexto/recepcion.md`), la pérdida se contaría una vez por cada línea — es un reporte agregado para el dashboard, no la fuente de verdad del movimiento (el movimiento real de entrada se calcula correctamente vía `RecepcionFrutaService`, que resuelve el color con la primera línea de detalle).

## Capas C#

- **Domain**: `TipoMovimientoAlmacen` (`Entrada`/`Salida`), `OrigenMovimientoAlmacen` (`OrdenCorte`/`Recepcion`/`Manual`) enums; entidad `MovimientoCajaCampo`; `ExistenciaCajaCampo`/`PerdidaCajaCampoMes` (proyecciones agregadas); `AlmacenCajaCampoDto` (fila del dashboard); `IMovimientoAlmacenRepository`. `OrdenCorte`/`OrdenCorteDto` ganan `CajaCampoId`/`CajaCampoNombre` (al final del record posicional). `RecepcionFruta`/`RecepcionFrutaDto` ganan `CajasPerdidas` (justo después de `CajasDiferencia`).
- **Infrastructure.SqlServer**: `MovimientoAlmacenRepository` (wrapping directo de los 4 SPs). `OrdenCorteRepository`/`RecepcionFrutaRepository` agregan el parámetro nuevo a sus `InsertarAsync`/`ActualizarAsync`.
- **Application**:
  - `MovimientoAlmacenService`: `ObtenerDashboardAsync()` (junta existencias + pérdida del mes en curso en un `AlmacenCajaCampoDto` por color), `RegistrarMovimientoManualAsync(cajaCampoId, tipo, cantidad, observaciones)` (compra/ajuste desde el dashboard, `OrigenModulo="Manual"`, `OrigenId=null`).
  - `OrdenCorteService`: valida `CajaCampoId` obligatorio en `ResolverYValidarAsync`; `CrearAsync`/`ActualizarAsync`/`EliminarAsync` llaman `RegistrarMovimientoSalidaAsync` (borra-y-reinserta) al final.
  - `RecepcionFrutaService`: `Validar()` tiene las fórmulas nuevas de `CajasDiferencia`/`CajasPerdidas`. El movimiento de entrada **no** se registra en `CrearAsync` (todavía no hay línea de detalle en ese punto — el flujo de UI llama `CrearAsync` y luego `AgregarLineaAsync` por cada fila), se registra/recalcula en `AgregarLineaAsync`, `EliminarLineaAsync` (firma cambiada a `(int id, int recepcionFrutaId)`) y `ActualizarAsync`, todas vía el mismo método privado `RegistrarMovimientoEntradaAsync(recepcionFrutaId)` que relee el estado actual de la Recepción y su primera línea de detalle para resolver el color.
- **WinForms**:
  - `OrdenCorteEditarForm`: nuevo `LookUpEdit _cmbCajaCampo` ("Color de Caja") en el grupo de cajas existente, filas de controles debajo desplazadas 26px. `CajaCampoService` se agregó a la cadena de inyección de `OrdenCorteEditarForm`/`OrdenesCorteForm`/`RecepcionesFrutaForm`/`LoteEditarForm`/`LotesForm`/`MainForm` (todos los que instancian `OrdenCorteEditarForm` directa o indirectamente).
  - `RecepcionFrutaEditarForm`: nuevo `SpinEdit _spnCajasPerdidas` (readonly, calculado en vivo junto con Diferencia) en el grupo "Control de Cajas" (grupo creció 30px, controles debajo desplazados).
  - `Forms/Almacenes/AlmacenCajaCampoDashboardForm` (nueva): `GridControl` con `AlmacenCajaCampoDto` (Color, Existencia, Pérdida del Mes) + botones "Registrar Compra"/"Ajuste" que abren `MovimientoCajaCampoEditarForm` (combo Color con `+` a `CajasCampoForm`, Cantidad, Observaciones) y refrescan al volver.
  - Ribbon: nueva `RibbonPage _pageAlmacenes` ("Almacenes", después de "Producción"), grupo "Caja de Campo", botón `_btnAlmacenCajaCampo` (Id=34, `MaxItemId` 33→34) que abre el dashboard. Gate de permiso: módulo `Almacenes`, pantalla `AlmacenCajaCampo`.

## Verificación (sesión de implementación)

`dotnet build` limpio en las 5 capas (`FrontOne.WinForms.csproj`, que arrastra todo el resto, y `FrontOne.Tests.csproj`). SQL desplegado contra `172.16.1.100\FrontOne` con `sqlcmd`, confirmado por catálogo (`sys.columns`, `sys.tables`, `INFORMATION_SCHEMA.ROUTINES`). Simulación end-to-end contra la BD real (inserts/deletes directos a los SPs de `Almacenes`, sin dejar datos de prueba):

1. Salida 300 (ROJA, origen `OrdenCorte`/999999) + Entrada 290 (origen `Recepcion`/888888) → existencia neta `-10`. ✅
2. Editar la Orden de Corte a 400 (`EliminarPorOrigen` + reinsertar) → existencia `-110` (`-400+290`, no se duplicó el movimiento viejo de 300). ✅
3. Borrar la Recepción (`EliminarPorOrigen`) → existencia `-400` (la entrada desaparece). ✅
4. Limpieza final (`EliminarPorOrigen` del origen de Orden de Corte) → existencia vuelve a `0` en las 4 filas, `0` renglones huérfanos. ✅

UI (combos nuevos, dashboard, diálogo de movimiento manual) no se pudo probar visualmente en este entorno — pendiente de que el usuario la pruebe en la app real.

## Iteración 2 — modelo de 3 cuentas (Existencia / En Campo / En Producción)

El usuario probó el dashboard v1 (una sola cifra "Existencia" que ya restaba lo que salía en la Orden de Corte) y pidió separar explícitamente **dónde está** la caja, no solo el neto:
> "las que se descuenten en el dashboard se ponga como en campo osea si tengo 1500 en existencia y la orden de corte se hace por 300 seria en existencia: 1200 en Campo: 300 y tenga otro campo en produccion"

Rediseño: una caja de campo pasa por **3 cuentas** (`CuentaAlmacen`: `Existencia`, `EnCampo`, `Produccion`), no un solo saldo neto:

- **Existencia**: cajas vacías físicamente en el almacén/empaque.
- **EnCampo**: cajas que salieron con la cuadrilla vía Orden de Corte y todavía no se sabe qué pasó con ellas (ni se han recibido).
- **Produccion**: cajas que volvieron con fruta en una Recepción — están en la línea de empaque, no en el almacén de cajas vacías.
- Cajas **perdidas** no entran a ninguna cuenta — simplemente salen de circulación (se ven reflejadas restando del total, nunca como saldo positivo en ningún lado).

**Movimientos por evento** (siempre par o trío, mismo `OrigenModulo`/`OrigenId`, se reemplazan juntos con el patrón borra-y-reinserta ya existente):
- **Orden de Corte** (`OrdenCorteService.RegistrarMovimientoSalidaAsync`): `Existencia/Salida` + `EnCampo/Entrada`, ambos por `CajasEntregadas`.
- **Recepción** (`RecepcionFrutaService.RegistrarMovimientoEntradaAsync`): `EnCampo/Salida` por `CajasEntregadas` completo (el total que salió con la cuadrilla, ya diagnosticado — cortado, vacío o perdido, no importa) + `Produccion/Entrada` por `CajasCortadas` (si > 0) + `Existencia/Entrada` por `CajasRecibidasVacias` (si > 0). La resta (`CajasPerdidas`) no genera movimiento — al no volver a ninguna cuenta, simplemente deja de estar "EnCampo" sin aparecer en otro lado, que es justo el efecto de una pérdida.
- **Manual** (`MovimientoAlmacenService.RegistrarMovimientoManualAsync`, compra/ajuste desde el dashboard): siempre `Cuenta=Existencia` — es la única cuenta que el usuario corrige a mano; `EnCampo`/`Produccion` solo se mueven automáticamente.

### Base de datos
- `Database/Almacenes/003_Alter_MovimientoCajaCampo_Cuenta.sql`: agrega `Cuenta NVARCHAR(20) NOT NULL DEFAULT('Existencia')` + `CHECK IN ('Existencia','EnCampo','Produccion')` a `Almacenes.MovimientoCajaCampo`. El `DEFAULT` backfillea correctamente la única fila real que ya existía en producción (una compra manual de 1500 ROJA capturada por el usuario antes de este cambio) sin UPDATE aparte. **Nota de sintaxis**: el `CHECK` va inline en el mismo `ADD` (no en un `ALTER TABLE ADD CONSTRAINT` separado) — separarlos en el mismo batch sin `GO` de por medio revienta con "nombre de columna Cuenta no válido" porque SQL Server resuelve nombres de todo el batch antes de ejecutar cualquier statement.
- `Database/Almacenes/004_SP_MovimientoCajaCampo_Saldos.sql`: `sp_MovimientoCajaCampo_Insertar` gana `@Cuenta`. Se **elimina** `sp_MovimientoCajaCampo_ObtenerExistencias` (`DROP PROCEDURE IF EXISTS`) y se reemplaza por `sp_MovimientoCajaCampo_ObtenerSaldos`, que pivotea las 3 cuentas en columnas (`Existencia`, `EnCampo`, `Produccion`) por color, cada una `SUM(CASE Entrada +Cantidad / Salida -Cantidad)` filtrado por `Cuenta`.

### Capas C#
- Domain: enum `CuentaAlmacen` (`Existencia`/`EnCampo`/`Produccion`) nuevo. `MovimientoCajaCampo` gana `Cuenta`. Entidad `ExistenciaCajaCampo` **renombrada** a `SaldoCajaCampo` (Id/Nombre/Existencia/EnCampo/Produccion). `AlmacenCajaCampoDto` gana `EnCampo`/`Produccion` (antes de `PerdidaMes`). `IMovimientoAlmacenRepository.ObtenerExistenciasCajaCampoAsync` renombrado a `ObtenerSaldosCajaCampoAsync`.
- Infrastructure: `MovimientoAlmacenRepository` actualizado a los nombres/parámetros nuevos.
- Application: los 3 puntos de inserción (`OrdenCorteService`, `RecepcionFrutaService`, `MovimientoAlmacenService`) actualizados según el detalle de arriba.
- WinForms: `AlmacenCajaCampoDashboardForm` — grid ahora muestra `Color de Caja | Existencia | En Campo | En Producción | Pérdida del Mes` (antes solo `Existencia`); form ensanchado de 520 a 720px de ancho para que quepan las columnas nuevas.

### Verificación (simulación end-to-end contra la BD real, sin dejar datos de prueba)
Partiendo del saldo real (1500 ROJA en Existencia, la compra manual del usuario):
1. Orden de Corte por 300 → `Existencia: 1200, EnCampo: 300, Produccion: 0`. ✅ (coincide exacto con el ejemplo del usuario)
2. Recepción con Cortadas=280, Vacías=10 (Perdidas=10 implícito) → `Existencia: 1210, EnCampo: 0, Produccion: 280`. Total contabilizado 1210+280=1490, +10 perdidas = 1500 original — cuadra. ✅
3. Limpieza (`EliminarPorOrigen` de ambos orígenes de prueba) → vuelve exacto a `Existencia: 1500, EnCampo: 0, Produccion: 0`, 1 sola fila en la tabla (la compra manual real del usuario, intacta). ✅

## Iteración 3 — corrección: `CajasPerdidas` debe basarse en `CajasPorEntregar`, no en `CajasEntregadas`

El usuario probó con datos reales (Por Entregar 300, Entregadas 250, Cortadas 200, Vacías 50) y el dashboard mostró `Perdidas: 0` cuando esperaba `50`. Causa: la fórmula usaba `CajasEntregadas` (250) como base — `250 - 200 - 50 = 0` — pero `CajasEntregadas` es un número intermedio/parcial capturado en el momento (aquí menor a lo solicitado), no lo que físicamente salió del almacén. Lo que realmente salió del almacén es `CajasPorEntregar` (copiado de `OrdenCorte.CajasEntregadas` al crear la Orden, y es el mismo número que se mueve a la cuenta `EnCampo`) — con esa base, `300 - 200 - 50 = 50`, que es el valor correcto.

Corregido en dos lugares de `RecepcionFrutaService.cs` (deben usar la misma base para que `EnCampo` cierre en cero):
- `Validar()`: `CajasPerdidas = CajasPorEntregar - CajasCortadas - CajasRecibidasVacias` (antes usaba `CajasEntregadas`).
- `RegistrarMovimientoEntradaAsync()`: el movimiento `EnCampo/Salida` ahora usa `recepcion.CajasPorEntregar` (antes `recepcion.CajasEntregadas`) — así siempre cierra exacto contra el `EnCampo/Entrada` que se registró al crear la Orden de Corte (mismo número, `OrdenCorte.CajasEntregadas` == `RecepcionFruta.CajasPorEntregar`).

`CajasDiferencia` (`PorEntregar - Entregadas`) no cambió — sigue siendo informativo: cuántas de las cajas solicitadas realmente llegaron a repartirse con la cuadrilla.

**Corrección de datos reales**: la única Recepción capturada hasta este punto (Folio 0000017, Id 11) ya tenía `CajasPerdidas=0` guardado con la fórmula vieja y sus movimientos de Almacenes (`EnCampo/Salida` en 250) reflejaban el bug. Se corrigió directo en la BD (mismo criterio que aplicaría un re-guardado desde la UI): `UPDATE` a `CajasPerdidas=50`, `EliminarPorOrigen('Recepcion', 11)` + reinserción de los 3 movimientos con `EnCampo/Salida=300`. Verificado: `Existencia: 1250, EnCampo: 0, Produccion: 200`, `Pérdida del Mes (ROJA, agosto 2026): 50`.
