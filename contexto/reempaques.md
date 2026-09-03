# Módulo Reempaques — desarmar y reconstruir Pallets

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo. Requiere entender [produccion.md](produccion.md) (Pallets/PalletDetalle) primero.

Desarma un Pallet ya armado y reconstruye uno o más Pallets nuevos sin perder la trazabilidad de Lote — caso real: un pallet mixto de varios lotes que hay que reempacar en presentaciones distintas conservando de qué lote vino cada kilo.

## Decisiones de negocio de fondo

- Solo entran pallets **con identificador** — nunca los del Pallet Neutro (`EsNeutro=1`, ver [produccion.md](produccion.md)).
- El pallet de **Entrada** entra **completo**, nunca parcial. Sus kilos regresan a su(s) lote(s) de origen, pero **reservados al folio de reempaque**, no al saldo general del lote.
- La **Corrida original nunca se toca ni se reabre** — el reempaque no afecta la liquidación de la Corrida.
- **Encadenable**: un pallet nacido de un reempaque puede volver a entrar a otro reempaque. Folio compartido con `Produccion.SeqPalletFolio`.
- **Cierre exige saldo 0 por lote**, sin compensar entre lotes — el pallet neutro de reempaque cierra cada diferencia con los mismos productos SAP `MERMA`/`DIFERENCIA PESO A FAVOR` que usa el Pallet Neutro normal.
- Etiquetado (GS1-128/VoiceCode) **solo existe en Salida, nunca en Entrada** — pedido explícito del usuario.

## Iteración 1 (2026-08-31): tablas propias — ya no vigente, ver rediseño abajo

Construido originalmente con 4 tablas propias (`Produccion.ReempaquePallet`/`ReempaquePalletDetalle` + 2 más) y ~17 SPs clonados. El pallet origen pasaba a `Estatus=6` y llenaba `Pallet.NoReempaque` (columnas que ya existían en `Produccion.Pallet`, puestas ahí a propósito para este módulo desde antes de construirlo). Hipervínculo `NoReempaque` → Reempaque en `PalletsForm`/`PalletEditarForm`.

**Diferido a propósito, no olvidado en esa iteración** (sigue pendiente tras el rediseño, no se revisó de nuevo): hipervínculo inverso (fila de Entrada → abrir `PalletEditarForm` del pallet origen) — no se implementó porque hubiera requerido enhebrar ~14 servicios de `PalletEditarForm` a través de `ReempaqueEditarForm`/`ReempaquesForm`/`MainForm` para una conveniencia menor (el folio ya es visible como texto plano en el grid). Revisar si algún día se pide explícitamente.

Reempaques se construyó **antes** que el plan pendiente de Fabricación de Lote en SAP (ver memoria `sap_fabricacion_lote_pendiente`) — ese plan asume que Reempaques ya existe, y cuando se retome habrá que agregarle el reverso de orden de producción para pallets reempacados (regla del usuario: reempacar un pallet ya fabricado en SAP dispara deshacer la orden vieja y crear una nueva).

## Rediseño (2026-09-02): Reempaques ya no vive en tablas propias

**Motivo**: con tablas propias, un pallet que nacía de un reempaque no se podía completar más tarde con cajas liberadas de *otro* reempaque distinto — cada reempaque era una isla.

**Cambio de fondo**: la salida de un reempaque ahora es una línea más de `Produccion.PalletDetalle`, distinguida por `CorridaId` **o** `ReempaqueDetalleId` (exactamente uno de los dos, nunca ambos ni ninguno) — un pallet normal y uno nacido de reempaque son la misma tabla, así que **cualquier** pallet existente (normal o ya reempacado antes) se puede completar con cajas liberadas de un reempaque nuevo.

- Se **eliminan** `Produccion.ReempaquePallet`/`ReempaquePalletDetalle` y ~12 SPs clonados de la iteración 1.
- `PalletEditarForm` gana columna **"Origen"** + hipervínculo **"No. de Reempaque"** con navegación **bidireccional** pallet↔reempaque (resuelve el pendiente diferido de la iteración 1, ya no hace falta enhebrar servicios porque ahora es la misma tabla).
- Migraciones: `Database/Produccion/022_Schema_SP_Reempaque.sql` (schema base del módulo), `024_Alter_PalletDetalle_OrigenReempaque.sql` (columna `ReempaqueDetalleId` en `PalletDetalle`), `025_Alter_Pallet_GranelEnProceso.sql` (estatus 7 "En Proceso", ver [produccion.md](produccion.md) — se agregó **en el mismo commit** del rediseño porque un pallet granel destino de reempaque se marcaba "Completo" apenas recibía la primera línea liberada, sacándolo del buscador de destinos disponibles).

## Ver también

[produccion.md](produccion.md) — Pallets/PalletDetalle, estatus calculado, Pallet Neutro, GS1/VoiceCode.
