---
name: reporte-designer-integracion
description: Conectar un reporte nuevo (XtraReport) al Diseñador de Reportes de FrontOne (DisenadorReporteForm) — origen de datos temporal para el Field List, wireado en ReportesForm, y binding declarativo de las etiquetas del layout. Usar al crear o modificar un XtraReport nuevo en FrontOne.WinForms/Reports.
---

## Regla dura: todo reporte nuevo declara su origen de datos para el Diseñador

El Diseñador de Reportes (`FrontOne.WinForms/Forms/Sistema/DisenadorReporteForm.cs`, envuelve `XRDesignForm` de DevExpress) deja personalizar el layout de cualquier reporte y guardar el XML resultante en `Configuracion.ReportePlantilla`. Para que el Field List del Diseñador muestre columnas reales (y no aparezca vacío), el reporte necesita un `SqlDataSource` conectado *mientras se diseña* — pero **nunca** en runtime normal ni al guardar el layout, porque `SaveLayoutToXml` serializaría la contraseña de conexión dentro del XML guardado.

Patrón obligatorio, mismo molde que `ReportePallet.cs`/`ReporteRecepcionFruta.cs` (referencia):

1. En el `.cs` del reporte, dos métodos que usan el helper compartido `ReporteConexionSql.CrearOrigenDatos` (`FrontOne.WinForms/Reports/ReporteConexionSql.cs`) contra el **mismo stored procedure** que ya llena `CargarDatos`:
   ```csharp
   private SqlDataSource? _origenDatos;

   public void ConectarOrigenDatos(SqlOptions sqlOptions, /* mismos parámetros que CargarDatos necesita para identificar el registro */)
   {
       DesconectarOrigenDatos();
       _origenDatos = ReporteConexionSql.CrearOrigenDatos(sqlOptions, "{CodigoReporte}", "{Schema}.sp_{Entidad}_ObtenerParaReporte", /* QueryParameter(s) */);
       ComponentStorage.Add(_origenDatos);
       // Basta con ComponentStorage.Add — el Field List del Diseñador arrastra campos de
       // cualquier fuente de datos registrada ahí, sin necesidad de que sea el DataSource
       // "activo" del reporte/banda. NO asignar DataSource/DataMember acá: el DataSource real
       // en runtime lo pone CargarDatos (ver regla de binding declarativo, punto 4 de abajo).
   }

   public void DesconectarOrigenDatos()
   {
       if (_origenDatos is null) return;
       ComponentStorage.Remove(_origenDatos);
       _origenDatos.Dispose();
       _origenDatos = null;
   }
   ```
2. Wireado en `FrontOne.WinForms/Forms/Sistema/ReportesForm.cs`, en los DOS `switch` (`ConectarOrigenDatos` por Código y `DesconectarOrigenDatos` por tipo) — agregar el `case` del nuevo reporte en ambos. Sin este paso el método del punto 1 existe pero nunca se llama (bug real que pasó con `ReportePallet`: el método estaba escrito pero el `case` nunca se agregó al switch, dejando el Diseñador sin campos silenciosamente).
3. Los parámetros del SP para el Diseñador son un valor de referencia que no depende de que exista una fila real (mismo criterio que `id: 0` en Pallet/RecepcionFruta, o `DateTime.Today` en reportes con rango de fecha) — `RebuildResultSchema()` solo necesita la metadata de columnas, no datos reales.
4. **Las etiquetas de valor del layout deben usar binding declarativo real, no solo tener un origen de datos disponible para arrastrar.** Que el Diseñador tenga Field List no sirve de nada si las etiquetas del layout son texto estático que el código llena a mano después (bug real que pasó con `ReporteRecepcionFruta`/`ReportePallet`: el Field List sí existía, pero ninguna etiqueta del layout default estaba enlazada — solo se notaba al abrir el Diseñador porque se veían vacías). Toda etiqueta que muestre un campo 1:1 del SP (sin lógica de negocio de por medio) va así:
   ```csharp
   // .Designer.cs — en vez de dejar la etiqueta sin ExpressionBindings:
   _lblNoLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[NoLote]"));
   _lblPesoBruto.TextFormatString = "{0:N2}";                 // formato numérico
   _lblPesoBruto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PesoBruto]"));
   _lblFecha.TextFormatString = "{0:dd/MM/yyyy}";              // formato de fecha
   _lblFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Fecha]"));
   ```
   ```csharp
   // .cs — CargarDatos asigna el DataSource real en vez de `_lblXxx.Text = datos.Xxx` uno por
   // uno; las ExpressionBindings se resuelven solas contra ese DataSource al CreateDocument().
   // A nivel de reporte si todo el layout vive en una sola banda (ReporteRecepcionFruta):
   DataSource = new List<TDto> { datos };
   DataMember = null;
   ```
   Los nombres de propiedad del DTO deben coincidir exactamente con los alias que regresa el SP (ya es el criterio establecido para que el Field List del Diseñador y el binding en runtime apunten al mismo nombre sin traducción).

   Quedan con llenado manual de `.Text` en `CargarDatos` (nunca se convierten a `ExpressionBinding`):
   - Campos que requieren lógica de negocio o transformación que no es un mapeo directo de columna (traducir un enum/`byte` de estatus a texto, `bool` → "Sí"/"No", concatenar dos columnas con separador — estos SÍ se precalculan como propiedad plana del wrapper `VistaEncabezado` y se enlazan igual, ver abajo).
   - Campos sin columna real en ningún SP (ej. `_lblNoCortadores` en `ReporteRecepcionFruta`, hardcodeado en `0`).

   **Encabezado (una fila) + membrete de empresa (otro origen de datos) + detalle repetitivo (una lista) en el mismo reporte, todo declarativo — patrón `VistaEncabezado` + `DetailReportBand`.** `Band`/`XRControl` normales (`ReportHeaderBand`, `DetailBand`, etc.) no exponen `DataSource`/`DataMember` propio, solo `DataBindings` a nivel de reporte — confirmado por reflexión sobre `DevExpress.XtraReports.v26.1.dll`. Pero **`DetailReportBand`** (distinto de `DetailBand`) sí lo tiene: hereda de `XtraReportBase` (misma base que `XtraReport`) y trae su propia colección `Bands`, donde se anida su propio `DetailBand`. Eso permite que el reporte tenga un `DataSource` de una sola fila (para encabezado/membrete) mientras el `DetailReportBand` anidado tiene su propio `DataSource` de lista (para el detalle), sin conflicto:
   - `CargarDatos` arma un `record` privado `VistaEncabezado(TDto Datos, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo)` (o el nombre de propiedad que corresponda al DTO del reporte, ej. `Pallet`) que combina por referencia el DTO propio del reporte + `EmpresaConfiguracionDto`, sin aplanarlos, más los 2 campos de membrete que ya requerían formato en C# (`Rfc`, `TelefonoCorreo`) como propiedades planas precomputadas.
   - `DataSource = new List<VistaEncabezado> { vista }; DataMember = null;` a nivel de reporte — sirve encabezado y membrete con rutas anidadas (`[Datos.Campo]`, `[Empresa.RazonSocial]`, y planas para lo precomputado: `[Rfc]`, `[TelefonoCorreo]`).
   - El `DetailReportBand` (campo nuevo en el `.Designer.cs`, con el `DetailBand` de siempre anidado adentro vía `_detailReportBand.Bands.Add(_detailBand);`, y registrado en el `Bands.AddRange(...)` del reporte en el lugar donde antes iba `_detailBand` directo) recibe su propio `_detailReportBand.DataSource = detalle.ToList(); _detailReportBand.DataMember = null;` — el `DetailBand` interno conserva sus `ExpressionBindings` de siempre contra el DTO de detalle, sin cambios.
   - El logo (`byte[]`) también se enlaza declarativo contra `XRPictureBox.ExpressionBindings` con propiedad `"ImageSource"` (no `"Image"`) y expresión `"Iif(IsNullOrEmpty([Empresa.Logo]), Null, [Empresa.Logo])"` — confirmado por reflexión que `ImageSource` tiene constructor `(bool, byte[])` y `TypeConverter` registrado, el mecanismo que usa DevExpress para convertir el resultado de la expresión.
   - Reportes sin banda repetitiva (`ReporteRecepcionFruta`) no necesitan `DetailReportBand` — el wrapper `VistaEncabezado` sirve directo como `DataSource` del reporte y ya cubre encabezado + membrete + logo.
   - Referencia: `ReportePallet`, `ReporteIncidencias` (ambos con `DetailReportBand`) y `ReporteRecepcionFruta` (sin detalle repetitivo) ya siguen este patrón completo.

Excepción: un reporte piloto/base sin `CargarDatos` ni pantalla real detrás (para probar un control nuevo antes de que exista el módulo de negocio) no necesita nada de este patrón todavía — se agrega cuando el reporte pase a tener un SP real.
