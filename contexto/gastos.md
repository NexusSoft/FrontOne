# Módulo Gastos (schema `Gastos`) — liquidación de costos por Lote

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo. Requiere que el Lote tenga su [Corrida](produccion.md) ya finalizada.

Para cada Lote con su Corrida ya finalizada, calcula el costo de **Fruta** (por Materia Prima, Fijo o "a Banda" según el Acuerdo de Corte), permite capturar y ajustar los costos de **Cosecha** y **Acarreo** por Recepción, y genera 2 reportes de liquidación: "Reporte de Proceso" y "Reporte de Proceso y Liquidación para Productor".

## Estructura

- `GastoLoteForm` (form principal), 3 pestañas: **Fruta**, **Cosecha**, **Acarreo** — cada una con su propio grid + panel de Ajustes/Totales.
- Catálogo nuevo **Tipos de Ajuste**.
- `GastoFrutaCategoria`: costo de fruta calculado desde `Acopio.ListaPrecioFruta`, según el Acuerdo de Corte del Lote sea Fijo o a Banda.
- `GastoRecepcion`: costo de Cosecha/Acarreo, uno por Recepción del Lote — columna **Servicio** + botón "Actualizar Precio" que jala el precio vigente de Lista de Precios de Corte/Acarreo (con auditoría, no silencioso).
- Wireado en el Ribbon (botón "Gastos", antes placeholder deshabilitado), permisos propios, bloque 17 del tracker de QA.

## Reanclaje de `Acopio.ListaPrecioFruta` a Categoría×Calibre APEAM

`ListaPrecioFruta` dejó de identificarse por `ItemCode`/`ItemName` (SAP en vivo) + `VariedadId`, y pasó a identificarse por `CategoriaId`+`CalibreApeamId`, tomados del catálogo local `Catalogos.MateriaPrima` — **ya no consulta SAP** para esto. El `LookUpEdit` de Variedad se retiró por completo del formulario. Las columnas de precio se renombraron de `Lista1/2/3` a sus nombres reales **Convencional/Orgánica/Nacional** en toda la pila (BD, entidad, DTO, grid) — el renombre se propagó a los combos/grids que las usan en Acuerdo de Corte y en el propio módulo de Gastos.

El grid de captura/edición de `ListaPrecioFruta` se ajusta al **contenido** de cada columna (no a la pantalla), y colorea en pastel las columnas de precio y el grupo de Categoría (Cat 1/Cat 2/Nal) para distinguir a simple vista.

Este cambio de identificación obligó a corregir el match de precio "a banda" en `sp_GastoFrutaCategoria_Obtener` y `sp_GastoFrutaCategoria_ObtenerResumenMercado`, pivoteando por `Catalogos.MateriaPrima` para llegar a Categoría+Calibre APEAM en vez de ItemCode+Variedad — si no, el cálculo de costo de Fruta en Gastos dejaba de encontrar precio.

## Bugs de cálculo corregidos (dos rondas, tras probar en la app real)

- **Acarreo multiplicaba el precio de tramo** (`Acopio.ListaPrecioAcarreo`) **por Peso Neto**, como si fuera tarifa por kg — el precio de tramo ya es un monto fijo. `Cantidad` ahora siempre `1` para las pestañas Cosecha y Acarreo (no una cantidad calculada). La misma corrección se propagó al SP que alimenta los reportes de Proceso de Lote y Liquidación al Productor.
- En las pestañas Cosecha/Acarreo, "Concepto" se renombró a **"Empresa de Servicio"** para que coincida con el nombre real del dato que se está mostrando.
- Grid de cada pestaña fijado a 360px de alto (antes 260px) — se probó `Anchor` dinámico y `Dock.Fill` primero, pero ambos rompían el panel de totales (`ShowFooter`) de la pestaña Fruta al redimensionar el form en tiempo de ejecución; un tamaño estático más grande evitó el problema. Controles de abajo (Vigencia/Reporte en Fruta, Ajustes/Totales en Cosecha/Acarreo) se recorrieron la misma distancia.

## Ver también

[produccion.md](produccion.md) — Corridas (el Lote debe tener su Corrida finalizada antes de poder liquidarse en Gastos).
