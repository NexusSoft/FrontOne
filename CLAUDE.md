# FrontOne ERP — Convenciones de nombres

Reglas de nomenclatura para todo el proyecto. Aplican a partir de la Fase 3 en adelante, y ya reflejan el patrón usado en Fases 1-2 (Seguridad/Auditoría/Conexiones).

## Regla general
Sustantivos de negocio (entidades, DTOs, repos, servicios) en **español**, igual que en el documento de arquitectura (`ClienteService`, `IPalletRepository`, etc.). Sufijos técnicos (`Service`, `Repository`, `Form`, `Exception`, `Options`, `Dto`) siempre en inglés. Nada de abreviaturas.

## Regla dura: comentarios en español, mensajes al usuario en español
- **Todo comentario de código** (`//`, `/* */`, `///`, comentarios en `.sql`) va en **español**. Nombres de identificadores (clases, métodos, variables) siguen las reglas de nomenclatura de este documento (sufijos técnicos en inglés donde aplique), pero el texto explicativo siempre en español.
- **Todo mensaje visible para el usuario** va en español: `XtraMessageBox.Show(...)`, mensajes de `throw new ValidationException("...")`/`SqlRepositoryException` u otras excepciones que se muestren en UI, `Text`/`Caption`/`Title` de forms y controles, textos de botones, tooltips, `NullText` de `LookUpEdit`, etc.
- Mensajes de log (`ILogger`, `Serilog`) y textos técnicos que solo ve un desarrollador (no un usuario final de la app) pueden quedar en inglés si ya lo estaban — la regla dura aplica a comentarios de código y a todo lo que renderiza la UI.

## Por capa

| Elemento | Ubicación | Patrón | Ejemplo |
|---|---|---|---|
| Entidad | `Domain/Entities` | `{Entidad}` singular | `Cliente` |
| DTO | `Domain/DTOs` | `{Entidad}Dto` (record) | `ClienteDto` |
| Interfaz repositorio | `Domain/Interfaces` | `I{Entidad}Repository` | `IClienteRepository` |
| Repositorio | `Infrastructure.SqlServer` o `.SapB1` | `{Entidad}Repository` (hereda `SqlRepositoryBase`) | `ClienteRepository` |
| Servicio | `Application/Services` | `{Entidad}Service` | `ClienteService` |
| Validador | `Application/Validators` | `{Entidad}Validator` | `ClienteValidator` |
| Form de listado | `WinForms/Forms` | `{EntidadPlural}Form` | `ClientesForm` |
| Form de alta/edición | `WinForms/Forms` | `{Entidad}EditarForm` | `ClienteEditarForm` |
| Excepción | `Shared/Exceptions` | `{Motivo}Exception` | `ValidationException` |
| Options de config | `Shared/Configuration` | `{Concepto}Options` | `SqlOptions` |
| Enum de negocio | `Domain/Enums` | `{Concepto}` | `TipoAccionAuditoria` |
| Extensión DI por capa | `Extensions/ServiceCollectionExtensions.cs` | `Add{Capa}` | `AddSqlServerInfrastructure` |

## Stored Procedures
`{Schema}.sp_{Entidad}_{Accion}`, siempre en español, schema = módulo de negocio (`Catalogos`, `Produccion`, `Seguridad`, `Auditoria`...).

Acciones estándar de CRUD: `Obtener`, `Insertar`, `Actualizar`, `Eliminar`.

```
Catalogos.sp_Cliente_Obtener
Catalogos.sp_Cliente_Insertar
Catalogos.sp_Cliente_Actualizar
Catalogos.sp_Cliente_Eliminar
```

## Métodos de repositorio
Mismo verbo que el SP que llaman, en español + `Async`: `ObtenerAsync`, `InsertarAsync`, `ActualizarAsync`, `EliminarAsync`. Así el nombre del método delata qué SP ejecuta.

## Métodos de servicio (Application)
Verbo de negocio claro, `Async` al final. Puede ser español o inglés según el caso ya establecido (`LoginAsync`, `RegistrarAsync`, `TienePermisoAsync`, `TestSqlConnectionAsync`) — lo que se entienda mejor, pero consistente dentro del mismo servicio.

## Todo módulo nuevo repite este patrón
Entidad → DTO → Interfaz → Repositorio → SPs → Servicio → Validador → Form(s), igual que el módulo Clientes de la Fase 3.

## Regla dura: todo módulo/tabla nueva se agrega a `Database/Utilidades/Inicializar_Datos_Produccion.sql`
Cada vez que se crea una tabla operativa nueva (catálogo, módulo de negocio — cualquier tabla que no sea de `Seguridad`), hay que agregar su nombre `Schema.Tabla` al arreglo `@Tablas` de `Database/Utilidades/Inicializar_Datos_Produccion.sql`. Si la tabla tiene un folio/consecutivo por `SEQUENCE` (como `Acopio.SeqAcuerdoCorteFolio`), agregar también su `ALTER SEQUENCE ... RESTART WITH 1` al final del script.

`Seguridad.*` (Usuario, Rol, Permiso, UsuarioRol, Modulo, Pantalla, Accion) nunca se agrega — ese schema se deja intacto a propósito para que la base recién puesta en blanco en producción siga teniendo con qué iniciar sesión.

**Excepción — tablas singleton de configuración:** `Configuracion.Empresa` (datos de la empresa para membrete de reportes: razón social, domicilio, RFC, teléfono, correo, logo) tampoco se agrega. Es una tabla de una sola fila fija (`Id = 1`, `CHECK (Id = 1)`, sin `IDENTITY`) que la aplicación nunca inserta ni elimina, solo actualiza — un `TRUNCATE` la dejaría vacía y rompería `Configuracion.sp_Empresa_Obtener` (que siempre espera esa fila). Cualquier tabla singleton nueva del mismo tipo sigue este mismo criterio: no entra al arreglo `@Tablas`.

Este script nunca se ejecuta contra una base con datos reales que se quieran conservar — es exclusivamente para el día que se despliegue a producción con la base en blanco. No ejecutarlo como parte del flujo normal de desarrollo/pruebas.

## Regla dura: UI 100% DevExpress, sin excepción
DevExpress 26.1 está instalado en la máquina (feed NuGet local ya registrado: `DevExpress 26.1 Local`). Todo control visual en `FrontOne.WinForms` debe ser DevExpress — nunca WinForms nativo. Sin excepciones, ni siquiera en forms simples/utilitarios (login, configuración, diálogos).

Mapeo obligatorio WinForms nativo → DevExpress:

| Nativo (prohibido) | DevExpress (obligatorio) |
|---|---|
| `Form` | `DevExpress.XtraEditors.XtraForm` |
| `Button` | `DevExpress.XtraEditors.SimpleButton` |
| `TextBox` | `DevExpress.XtraEditors.TextEdit` |
| `Label` | `DevExpress.XtraEditors.LabelControl` |
| `CheckBox` | `DevExpress.XtraEditors.CheckEdit` |
| `ComboBox` | `DevExpress.XtraEditors.LookUpEdit` (listas de referencia) o `ComboBoxEdit` |
| `GroupBox` | `DevExpress.XtraEditors.GroupControl` |
| `DataGridView` | `DevExpress.XtraGrid.GridControl` + `GridView` |
| `MessageBox` | `DevExpress.XtraEditors.XtraMessageBox` |
| Menú principal / Ribbon | `DevExpress.XtraBars.Ribbon.RibbonForm` + `RibbonControl` |

Paquete NuGet: `DevExpress.Win` (meta-paquete, incluye Grid/Bars/Editors/Navigation) desde el feed local `DevExpress 26.1 Local`. Ya instalado en `FrontOne.WinForms.csproj`.

**Única excepción, aprobada explícitamente por el usuario**: el mapa de `HuertaEditarForm` (pin de ubicación de la huerta) usa `GMap.NET.WindowsForms.GMapControl` (paquetes `GMap.NET.Core`/`GMap.NET.WindowsForms`), no un control DevExpress. Se agotaron tres opciones de `DevExpress.XtraMap` (OpenStreetMap bloqueado con 403, Bing Maps descontinuado por Microsoft, Mapbox exigiendo tarjeta de pago para el alta) — ver detalle completo en `contexto.md`, sección "Mapa en `HuertaEditarForm`". No repetir este patrón en otros forms sin volver a consultar al usuario.

Notas de migración (ya aplicadas en todos los forms existentes):
- `GridView` (dentro de `GridControl`) ordena por columna de forma nativa con solo hacer click en el header — no hace falta código de sorting manual.
- Selección de fila: `gridView.GetFocusedRow()` (equivalente a `DataGridView.CurrentRow.DataBoundItem`).
- Combo con lista de referencia (ej. país): `LookUpEdit` con `Properties.DataSource/ValueMember/DisplayMember` + `Properties.Columns` explícitas, `Properties.TextEditStyle = TextEditStyles.DisableTextEditor` para que no permita texto libre. Selección/lectura vía `EditValue` (no `SelectedValue`).
- Password: `TextEdit` con `Properties.UseSystemPasswordChar = true`.

## Regla dura: todo `LookUpEdit` lleva NullText "Seleccionar" + flecha visible
Todo `LookUpEdit` del proyecto (sin excepción) debe declarar:
```csharp
_cmbX.Properties.NullText = "Seleccionar";
```
Si además lleva botón de alta rápida (`ButtonPredefines.Plus`) u otro botón custom, hay que agregar primero el botón `Combo` explícito — al agregar cualquier botón a `Properties.Buttons`, DevExpress deja de dibujar la flecha de despliegue automática, así que hay que declararla a mano:
```csharp
_cmbX.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
_cmbX.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
```
Si el `LookUpEdit` no tiene ningún botón custom, no hace falta agregar el `Combo` a mano (se muestra solo, `Buttons.Count == 0`).

## Regla dura: todo `LookUpEdit` busca por texto intermedio, no solo por inicio

Fijado en `ProductoTerminadoEditarForm` (`_cmbMateriaPrima`) — todo `LookUpEdit` del proyecto (sin excepción, incluidos los ya existentes) debe declarar, junto al `NullText`:
```csharp
_cmbX.Properties.NullText = "Seleccionar";
_cmbX.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
_cmbX.Properties.PopupFilterMode = PopupFilterMode.Contains;
```
Por defecto DevExpress filtra el desplegable solo por coincidencia al **inicio** del texto — con esto filtra por coincidencia en **cualquier parte** (ej. escribir "60" encuentra "EXP CAL 60 CAT 1" aunque no empiece con "60"). `PopupFilterMode` vive en `DevExpress.XtraEditors` (mismo namespace que `LookUpEdit`, no requiere `using` adicional); `SearchMode` vive en `DevExpress.XtraEditors.Controls` — si el archivo no importa ese namespace, usar el nombre completo como arriba en vez de agregar un `using` nuevo solo para esto.

## Regla dura: el botón `+` (`ButtonPredefines.Plus`) de un `LookUpEdit` siempre abre el listado completo del catálogo, nunca el diálogo de alta directo

El botón `+` de cualquier `LookUpEdit` debe abrir el **form de listado** del catálogo referenciado (`{EntidadPlural}Form`, con `Nuevo`/`Editar`/`Eliminar`/`Cerrar`), no el `{Entidad}EditarForm` de alta directamente. Así el usuario puede crear, corregir o borrar un registro del catálogo sin salir del flujo — abrir solo el diálogo de alta lo deja atascado si necesita editar o eliminar algo que ya existe.

```csharp
private async void CmbTipoPago_ButtonClick(object? sender, ButtonPressedEventArgs e)
{
    if (e.Button.Kind != ButtonPredefines.Plus) return;

    using var form = new TiposPagoForm(_tipoPagoService); // listado, NO TipoPagoEditarForm directo
    form.ShowDialog(this);
    await CargarTiposPagoAsync();
}
```

**Excepción documentada — catálogos sin listado propio (JefeAcopio):** `JefeAcopio` no tiene `{EntidadPlural}Form` con Nuevo/Editar/Eliminar/Cerrar — desde que se clonó el patrón de navegación de Productor, `JefesAcopioForm` quedó como picker puro (solo Buscar/Seleccionar/Cerrar) y todo el alta/edición vive en `JefeAcopioEditarForm`. Para este catálogo específico, el botón `+` abre `JefeAcopioEditarForm` directo — la pantalla ya arranca en blanco/modo Nuevo cuando se abre así (su `Load` llama `LimpiarFormulario()` sin navegar a ningún registro), así que el comportamiento es equivalente. No aplicar este patrón a otros catálogos sin volver a evaluar caso por caso — es válido solo porque `JefeAcopioEditarForm` ya arranca en blanco por diseño.

**Excepción — LookUpEdit sin botón `+` cuando el alta no tiene sentido embebida:** en `OrdenCorteEditarForm`, los combos de Huerta y de No. de Acuerdo no llevan `+`. Huerta se debe crear desde su flujo completo (multi-tab: domicilio, ubicación, certificaciones), no tiene sentido acortarlo desde un formulario de captura de Orden. Acuerdo de Corte es un flujo de negocio completo con su propio toggle Precio/Lista — abrirlo aquí obligaría a inyectar sus 11 servicios solo para un botón `+`. En ambos casos el catálogo ya tiene su propio botón en el Ribbon para darlo de alta antes de capturar la Orden.

Ya es el patrón establecido en `HuertaEditarForm`/`ProductorEditarForm` (los `+` de País/Estado/Municipio/Población/Producto/SistemaRiego/StatusHuerta abren `PaisesForm`/`EstadosForm`/`MunicipiosForm`/etc., nunca el `EditarForm` de alta). Si un catálogo referenciado desde un `LookUpEdit` con `+` todavía no tiene su form de listado, hay que crearlo — no vale usar el diálogo de alta como atajo.

## Regla dura: todo `LookUpEdit` sobre un catálogo editable lleva el botón `+`

Sin excepción — incluye combos de filtro en pantallas de listado (`_cmbFiltroPais`, `_cmbFiltroEstado`, etc.) y no solo los campos de captura de un registro. Si el catálogo referenciado tiene form de listado (`{EntidadPlural}Form`), el `LookUpEdit` que lo usa lleva `+` apuntando a ese listado, siguiendo la regla de arriba (abre el listado, nunca el alta directa) y recargando su propio `DataSource` al cerrar. Los combos de filtro que sintetizan una fila "Todos los X" (Id 0) al inicio deben volver a anteponerla al recargar después del `+`, no solo traer el catálogo crudo.

Único caso donde no aplica: catálogos sin form de listado propio (por ejemplo, si un combo referencia datos que no son un catálogo administrable por el usuario). Si eso ocurre, hay que crear el form de listado primero — no se vale dejar el `LookUpEdit` sin `+` como atajo.

## Regla dura: todo `LookUpColumnInfo` de un `LookUpEdit`/`RepositoryItemLookUpEdit` lleva ancho explícito, y el `PopupWidth` cubre la suma de columnas

Nunca usar el constructor de 2 parámetros `new LookUpColumnInfo(fieldName, caption)` — siempre el de 3, con un ancho que le entre cómodo al texto más largo esperable en esa columna, para que el desplegable no corte la información:
```csharp
_cmbX.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Nombre del catálogo"));
_cmbX.Properties.Columns.Add(new LookUpColumnInfo("Clave", 80, "Clave"));
_cmbX.Properties.PopupWidth = 320; // ≥ suma de anchos de columna + margen para el scrollbar
```
Guía de anchos de referencia (ajustar si el texto real del catálogo es más largo): nombre/descripción de catálogo ~200-220px, clave/código corto ~70-90px, columna booleana tipo "Sí/No" ~90-110px, descripción larga ~280-380px. `PopupWidth` siempre mayor o igual a la suma de anchos de columna, con margen extra (~20-30px) para que no aparezca scroll horizontal innecesario.

## Regla dura: todo `GridView` lleva el panel de búsqueda (Find) visible
Todo `GridView` del proyecto (sin excepción, incluidos los de forms nuevos) debe declarar en su `Designer.cs`:
```csharp
_gridView.OptionsFind.AlwaysVisible = true;
```
El panel filtra sobre los registros ya cargados en el grid. Los textos del panel salen en español vía `GridLocalizerEspanol` (`FrontOne.WinForms/Configuration/GridLocalizerEspanol.cs`), registrado una sola vez en `Program.cs` (`GridLocalizer.Active = new GridLocalizerEspanol();`) — no hay que configurar textos por grid. Si un texto de grid sale en inglés, se agrega su `GridStringId` al switch del localizer, nunca texto hardcodeado por form.

## Regla dura: todo buscador embebido de un catálogo grande carga un TOP 100 por defecto, nunca la tabla completa

Todo "buscador embebido" (picker: `TextEdit`/búsqueda + `SimpleButton` "Buscar" + `GridControl`/`GridView` de solo lectura + `Seleccionar`/`Cerrar`, el patrón de `ProductoresForm`/`HuertasForm`/`JefesAcopioForm`) debe cargar automáticamente un TOP 100 al abrirse (`Load`), para que el grid nunca se vea vacío — sin esperar a que el usuario escriba nada. La búsqueda por texto existente (mínimo 2 caracteres, TOP 500) no cambia; el TOP 100 es solo la carga inicial.

Implementación (ver `ProductoresForm`/`HuertasForm`/`JefesAcopioForm` como referencia):
- SP nuevo y dedicado por entidad, **sin** parámetro de filtro, `SELECT TOP 100 ... ORDER BY {columna de nombre}` — mismas columnas que el SP de búsqueda existente, nunca se toca ni se reutiliza el SP de búsqueda con `@Filtro = ''` (evita el `TOP 500` innecesario). Nombre: `sp_{Entidad}_ObtenerTop100`.
- `I{Entidad}Repository.ObtenerTop100Async()` → `{Entidad}Service.ObtenerTop100Async()` → llamado desde el handler `{Form}_Load`, wireado en `Designer.cs` (`Load += {Form}_Load;`), nunca en el constructor.
- El texto del form al cargar indica "100 más recientes — refina la búsqueda" (o equivalente), distinto del texto tras una búsqueda por texto ("N resultados" / "primeros 500").

Esta regla aplica a **cualquier catálogo con volumen alto de registros** (Productor, Huerta, Jefe de Acopio y cualquier catálogo nuevo con cientos o miles de filas esperables) — no aplica a catálogos chicos (País, Estado, Municipio, Zona, etc.) que ya cargan completos sin problema de desempeño.

## Regla dura: todo Form usa el patrón clásico de Visual Studio (Designer.cs)
Para que cualquier form se pueda abrir y editar con el diseñador visual de Visual Studio, **todo** `XtraForm` se separa en dos archivos:

- **`{Form}.Designer.cs`** — clase parcial sin modificador de acceso, contiene: `components`, `Dispose(bool)`, los controles declarados como **campos privados** (nunca variables locales — el diseñador solo puede editar controles que son campos), y `InitializeComponent()` con toda la construcción visual (posiciones vía `Location`/`Size`, textos fijos, wiring de eventos `Click`/etc). Nada de lógica de negocio acá.
- **`{Form}.cs`** — `public partial class {Form} : XtraForm`, con:
  - **Constructor sin parámetros** que solo llama `InitializeComponent()` — este es el que usa el diseñador de Visual Studio.
  - **Constructor con los servicios inyectados** (`: this()` para encadenar al de arriba), que guarda los servicios y dispara la carga de datos (`Load += async ...`, valores iniciales de edición, etc).
  - Los campos de servicios inyectados se declaran `private readonly {Tipo} _campo = null!;` (el `null!` es necesario porque el constructor sin parámetros no los asigna — al campo lo llena el constructor real).
  - El resto de la lógica (handlers de botones, validaciones, llamadas a Application) vive acá, nunca en Designer.cs.

Editores DevExpress con `.Properties` (`TextEdit`, `CheckEdit`, `LookUpEdit`) y `GridControl`/`GridView` necesitan `((ISupportInitialize)control.Properties).BeginInit()/EndInit()` alrededor de `InitializeComponent()` — así serializa bien el diseñador. `SimpleButton`/`LabelControl`/`GroupControl` no lo necesitan (`GroupControl` sí, por ser contenedor).

**Nunca lambdas inline dentro de `InitializeComponent()`** (ej. `_btnCerrar.Click += (_, _) => Close();`) — el diseñador de Visual Studio no puede parsear expresiones lambda ahí y tira error al abrir el form ("El diseñador no puede procesar el código..."). Todo `Click +=` en Designer.cs debe apuntar a un método con nombre (`_btnCerrar.Click += BtnCerrar_Click;`), y el método vive en el `.cs` (ej. `private void BtnCerrar_Click(object? sender, EventArgs e) => Close();`).

Todo módulo nuevo desde ahora sigue este patrón desde el principio — no se vuelve a construir un form 100% por código en el constructor único.

## Estándar de botones CRUD (fijado en `PaisesForm`/`PaisEditarForm`/`EstadosForm`/`EstadoEditarForm`)

Ajustado a mano en el diseñador de Visual Studio — este es el layout de referencia para todo módulo nuevo.

**Regla dura: altura 28px en todo botón de este estándar (`_btnNuevo`/`_btnEditar`/`_btnEliminar`/`_btnCerrar`/`_btnGuardar`/`_btnCancelar`) y en cualquier otro botón de acción del mismo tipo (buscadores embebidos, diálogos de captura, etc.), sin excepción.** El ancho varía según el texto (90 o 80px, ver tablas), pero la altura siempre es 28 — así el ícono de `ImageOptions.Image` se ve completo, sin recortarse como pasaba a 23px. Fijado 2026-09-04 al ajustar el módulo `Contenedor` (`ContenedoresForm`/`ContenedorEditarForm`/`ContenedorPalletAgregarForm`/`ContenedorPedidoBuscarForm`, todos migrados a 28px) — nuevo estándar para todo módulo desde ahora; los módulos viejos que sigan en 23px se migran la próxima vez que se toquen, no hace falta una pasada retroactiva dedicada.

**Form de listado** (`{EntidadPlural}Form`, ej. `PaisesForm`): grid arriba, botones abajo.

| Botón | Texto | Tamaño | Anchor | Orden (izq→der) |
|---|---|---|---|---|
| `_btnNuevo` | "Nuevo" | 90×28 | `Bottom, Left` | 1º |
| `_btnEditar` | "Editar" | 90×28 | `Bottom, Left` | 2º (6px de separación del anterior) |
| `_btnEliminar` | "Eliminar" | 90×28 | `Bottom, Left` | 3º (6px de separación) |
| `_btnCerrar` | "Cerrar" | 90×28 | `Bottom, Right` | pegado al borde derecho |

Los cuatro llevan ícono vía `ImageOptions.Image`.

**Form de alta/edición** (`{Entidad}EditarForm`, ej. `PaisEditarForm`): campos arriba, botones abajo, alineados a la derecha.

| Botón | Texto | Tamaño | Notas |
|---|---|---|---|
| `_btnGuardar` | "Guardar" | 80×28 | `AcceptButton` del form, con ícono |
| `_btnCancelar` | "Cancelar" | 80×28 | pegado a la derecha de Guardar (~10px), `DialogResult.Cancel` + `Close()` |

**Cómo agregar los botones en un módulo nuevo:** declarar los campos en `Designer.cs`, agregar cada `SimpleButton` con estas medidas/posiciones/anchors, y wirear el evento a un método con nombre en el `.cs` (nunca lambda inline, ver regla de arriba).

**Form maestro-detalle** (`{Entidad}EditarForm` con `DataNavigator`, ej. `ProductorEditarForm`/`HuertaEditarForm`): mismo criterio, cuatro botones abajo.

| Botón | Texto | Tamaño | Anchor | Orden (izq→der) |
|---|---|---|---|---|
| `_btnNuevo` | "Nuevo" | 90×28 | `Bottom, Left` | 1º |
| `_btnGuardar` | "Guardar" | 80×28 | `Bottom, Left` | 2º (6px de separación) |
| `_btnEliminar` | "Eliminar" | 90×28 | `Bottom, Left` | 3º (6px de separación) |
| `_btnCancelar` | "Cancelar" | 80×28 | `Bottom, Right` | pegado al borde derecho, solo |

Regla dura: **Nuevo/Guardar/Eliminar siempre agrupados a la izquierda en ese orden**; el botón de cierre (`Cancelar` en maestro-detalle, `Cerrar` en listados) siempre solo a la derecha, nunca mezclado con el grupo izquierdo. Ojo con el `Anchor` — tiene que ser `Bottom | Left` en los tres de la izquierda (si a alguno le queda `Bottom | Right` por error, se separa del grupo al redimensionar el form).

## Regla dura: todo botón `_btnNuevo`/`_btnEditar`/`_btnEliminar`/`_btnCerrar`/`_btnGuardar`/`_btnCancelar` lleva el mismo ícono en todo el proyecto

Dos fuentes únicas de verdad, cada una para su grupo de botones:

- `FrontOne.WinForms/Forms/Catalogos/EstadosForm.resx` (mismos íconos que `PaisesForm`) → botones `_btnNuevo`, `_btnEditar`, `_btnEliminar`, `_btnCerrar`.
- `FrontOne.WinForms/Forms/Catalogos/ProductoEditarForm.resx` → botones `_btnGuardar`, `_btnCancelar` (de **todo** `{Entidad}EditarForm`, incluidos los maestro-detalle como `HuertaEditarForm`/`ProductorEditarForm` que además tienen `_btnNuevo`/`_btnEliminar` — esos dos combinan íconos de ambas fuentes).

Todo form nuevo con alguno de estos seis botones debe llevar el ícono correspondiente, sin excepción — no vale dejar un botón sin ícono ni usar uno distinto al de la fuente única.

Los `.resx` de DevExpress son XML de **texto plano** (imagen en base64 dentro de un `<data>`), no hace falta Visual Studio para copiarlos:

1. En el `.resx` fuente (`EstadosForm.resx` o `ProductoEditarForm.resx` según el botón) cada botón tiene un bloque `<data name="_btnX.ImageOptions.Image" type="System.Drawing.Bitmap, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64"><value>...</value></data>`. Copiar el bloque completo (verbatim) del botón que corresponda al `.resx` del form nuevo (mismo nombre de archivo que el `.cs`, mismo namespace/carpeta — si no existe el `.resx`, crearlo con el mismo header/schema que cualquier `.resx` ya existente en el proyecto, terminando en `</root>`; si ya existe con otros bloques de datos, insertar antes de `</root>` sin tocar lo que ya había).
2. En el `Designer.cs` del form nuevo, agregar como primera línea de `InitializeComponent()` (si no está ya):
   ```csharp
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof({NombreDelForm}));
   ```
3. Justo después de `_btnX.Name = "_btnX";`, agregar:
   ```csharp
   _btnX.ImageOptions.Image = (Image)resources.GetObject("_btnX.ImageOptions.Image");
   ```

El `.resx` debe vivir en la **misma carpeta** que el `.cs`/`.Designer.cs` del form (mismo namespace) — el nombre del recurso embebido (manifest) se arma desde `{RootNamespace}.{RutaRelativa}`, así que si carpeta y namespace no coinciden, `resources.GetObject(...)` no encuentra el ícono en tiempo de ejecución.

## Regla dura: ningún registro de catálogo referenciado en otra pantalla se puede eliminar

Todo `FK` del proyecto se crea **sin** `ON DELETE CASCADE` ni `ON DELETE SET NULL` (default `NO ACTION`) — así SQL Server rechaza cualquier `DELETE` que dejaría una referencia huérfana, sin importar qué SP o pantalla lo dispare. Esto es estructural: no depende de que cada `{Entidad}Service.EliminarAsync` valide nada a mano, y aplica automáticamente a cualquier catálogo/tabla nueva mientras su `FK` no se cree con `CASCADE`/`SET NULL` explícito (no hacerlo sin consultar al usuario primero).

El error 547 de SQL Server (violación de `REFERENCE constraint`) se traduce a mensaje limpio en español en un solo punto: `SqlRepositoryBase.RunAsync` (`FrontOne.Infrastructure.SqlServer/Repositories/SqlRepositoryBase.cs`) detecta `SqlException.Number == 547` y lanza `SqlRepositoryException` con el texto "No se puede eliminar este registro porque ya está siendo utilizado en otra pantalla del sistema." — todas las pantallas de listado que ya capturan `catch (SqlRepositoryException ex)` y muestran `ex.Message` heredan el mensaje correcto sin tocar código propio. No agregar mensajes de hint hardcodeados por pantalla (`"probablemente tiene X asociados"`) — ya no hace falta, el mensaje real siempre es preciso.

## Regla dura: todo evento de eliminar registro pregunta antes de eliminar

Sin excepción — ningún botón/acción `Eliminar` (ni tecla Supr sobre un grid, ni "quitar fila") llama al método `EliminarAsync`/`EliminarPorFechaAsync` del servicio directo. Siempre primero:
```csharp
var confirmar = XtraMessageBox.Show(this, $"¿Eliminar {la entidad} '{nombre}'?", "FrontOne",
    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
if (confirmar != DialogResult.Yes)
{
    return;
}
```
y solo si `confirmar == DialogResult.Yes` se procede a borrar. Ya es el patrón en las 19 pantallas del proyecto que eliminan algo (todos los catálogos, `HuertaEditarForm`/`ProductorEditarForm` maestro-detalle, `ListaPrecioFrutaForm`, `AcuerdosCorteForm`, `ListaPrecioAcarreoForm`) — mantenerlo en todo módulo nuevo.

**Excepción real, no aparente**: quitar una fila **sin guardar todavía** de un grid de captura libre (como `ListaPrecioAcarreoForm`, donde `fila.Id == 0` porque nunca se insertó en BD) no necesita confirmación — no hay nada que perder en el servidor. La regla aplica a partir de que el registro ya existe en la base de datos.

## Regla dura: auditoría obligatoria en todo servicio Application con Crear/Actualizar/Eliminar

`Application` no puede depender de `WinForms` (viola capas), así que el usuario actual llega vía `ICurrentUserProvider` (interfaz en `FrontOne.Shared/Security`), implementada por `SessionContext` en WinForms y registrada en DI como `services.AddSingleton<ICurrentUserProvider>(sp => sp.GetRequiredService<SessionContext>());`.

Todo servicio Application con Crear/Actualizar/Eliminar inyecta `AuditService` + `ICurrentUserProvider` y registra:

- **Crear:** vuelve a leer el registro insertado (con Id real) y llama `RegistrarAsync(usuario, TipoAccionAuditoria.Crear, modulo, valoresAnteriores: null, valoresNuevos: JSON del registro)`.
- **Actualizar:** lee el registro *antes* de tocarlo, actualiza, vuelve a leer, y llama `RegistrarAsync(..., Modificar, JSON anterior, JSON nuevo)`.
- **Eliminar:** lee el registro antes de borrar, borra, llama `RegistrarAsync(..., Eliminar, JSON anterior, valoresNuevos: null)`.

`valoresAnteriores`/`valoresNuevos` = `JsonSerializer.Serialize(entidad)` completa (no armar diff campo por campo). Ojo: la entidad debe guardar el password ya **cifrado** (`PasswordEncriptado`), nunca el texto plano, para que no quede expuesto en el log de auditoría. `usuario` sale de `_currentUserProvider.NombreUsuario ?? "desconocido"`. `modulo` es una constante privada del servicio (ej. `"Catalogos"`).

Ya aplicado en `ProductorService`, `PaisService`, `EstadoService` — copiar el mismo patrón en cada servicio nuevo (Huertas incluido).

## Regla dura: todo módulo nuevo actualiza o crea su archivo en `contexto/`

El proyecto lleva memoria viva por módulo en `contexto/*.md`, indexada desde `contexto.md` (raíz del repo) — pensada para que una sesión nueva pueda leer solo el archivo del módulo que va a tocar en vez de cargar todo el proyecto. Si un módulo se construye pero nunca se documenta ahí, la siguiente sesión (propia o de otro dev) no tiene forma barata de reconstruir el contexto y termina releyendo código o repitiendo preguntas ya respondidas.

Cada vez que se agregue o cambie de forma significativa una funcionalidad (mismo criterio que la regla del tracker de QA de abajo: módulo nuevo, pantalla nueva, regla de negocio nueva, un rediseño, un bugfix real que valga la pena recordar):

- **Módulo nuevo** (schema SQL nuevo o sub-módulo grande sobre uno existente) → crear `contexto/{nombre-del-modulo}.md` siguiendo el formato de los archivos ya existentes (encabezado con link de vuelta a `contexto.md` y a `arquitectura.md`, decisiones de negocio confirmadas con el usuario, sección de Base de Datos con los scripts relevantes, capas C#, WinForms/Web, "Ver también" a módulos relacionados) y agregar su fila al índice de `contexto.md`.
- **Cambio dentro de un módulo ya documentado** → agregar una sección nueva al final del archivo existente (nunca editar/borrar una entrada vieja — mismo criterio append-only que ya rige la colaboración por git, ver la sección correspondiente en `contexto.md`).
- Si el cambio es transversal (no pertenece a un solo módulo — ej. una convención de código nueva, un fix de infraestructura compartida), va en `contexto/arquitectura.md`.
- Commitear el `contexto/*.md` **junto con el código que documenta** (mismo commit/PR) — no dejarlo suelto para "después".

El resto del flujo de trabajo con `contexto/` (cómo arrancar una sesión, `merge=union` en `.gitattributes`, disciplina append-only) está detallado en `contexto.md`, no se repite aquí.

## Regla dura: toda funcionalidad nueva actualiza el tracker de QA

El proyecto tiene un tracker de QA colaborativo en `docs/qa/qa-frontone-tracker.html` (publicado como Artifact, ver `docs/qa/README.md`) — un checklist secuencial por módulo/submódulo, con reporte de defectos directo a GitHub Issues. Si un módulo nuevo se construye pero nunca se agrega ahí, QA nunca se entera de que existe algo que revisar.

Cada vez que se agregue o cambie de forma significativa una funcionalidad (nuevo módulo, nueva pantalla, nueva regla de negocio, un fix de bug real que valga la pena volver a probar), hay que actualizar el arreglo `BLOQUES` de `docs/qa/qa-frontone-tracker.html`:

- **Módulo nuevo completo** → nuevo bloque al final del arreglo (siguiente `id` consecutivo), con `corto`/`nombre`/`pantallas`/`meta` y sus `casos` (mismo formato `["id.n","título","resultado esperado"]` que los bloques existentes — cada caso nace de una regla de negocio real del código, no genérico).
- **Pantalla o regla nueva dentro de un módulo ya cubierto** → agregar casos nuevos al bloque existente correspondiente (siguiente sufijo, ej. si el bloque 9 llega hasta `9.6`, el caso nuevo es `9.7`).
- **Nunca renumerar ni borrar casos/bloques ya existentes** — el `id` de cada caso es la clave con la que el equipo ya guardó su avance (`docs/qa/estado-qa-frontone.json` y el `localStorage` de cada quien); renumerar rompe ese historial.

Después de editar el arreglo, hay que **volver a publicar el mismo Artifact** (mismo `file_path`, mismo URL) para que el equipo lo vea actualizado — un caso nuevo en el HTML sin republicar no lo ve nadie. Este paso no es opcional: forma parte de terminar la funcionalidad, igual que compilar o desplegar el script SQL.

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

## Convenciones de `FrontOne.Web`

Sitio Blazor Web App (`net10.0`, render mode `InteractiveServer` global) que reutiliza tal cual las
mismas capas de `FrontOne.WinForms` (`Domain`, `Application`, `Infrastructure.SqlServer`,
`Infrastructure.SapB1`, `Shared`) — cero servicios duplicados. Ver `FrontOne.Web/Program.cs` y
`FrontOne.Web/Extensions/ServiceCollectionExtensions.cs` (`AddWeb`) para el detalle de host.

- **Páginas** viven en `Components/Pages/{Schema}/{EntidadPlural}.razor` (ej.
  `Components/Pages/Catalogos/Paises.razor`), igual que el schema SQL del módulo. Página de
  listado/CRUD simple = una sola página con `DxGrid` + `DxPopup` de edición, siguiendo
  `Paises.razor` como plantilla literal.
- **UI 100% DevExpress Blazor**, mismo espíritu que la regla dura de WinForms — nunca HTML/CSS
  crudo para controles de datos (grid, popup, form layout, tree view, drawer). Los únicos
  elementos HTML nativos permitidos son el `<form method="post">` de login/logout (tienen que
  posear fuera del circuito de Blazor, ver `LoginEndpoints.cs`) y contenedores de layout puros
  (`<div>`, `<header>`, `<main>`).
- **Autorización obligatoria por política en cada página nueva**: `@attribute
  [Authorize(Policy = "Permiso:{Pantalla}/{Accion}")]` a nivel de página para Consultar, y
  `<AuthorizeView Policy="Permiso:{Pantalla}/{Accion}">` alrededor de cada botón de
  Crear/Modificar/Eliminar. La política se resuelve sola vía `PermisoPolicyProvider` — no hace
  falta registrar nada en `Program.cs`. El `FallbackPolicy` global (`RequireAuthenticatedUser`)
  significa que una página sin `[Authorize]` explícito sigue exigiendo sesión, pero **no** filtra
  por pantalla — toda página de negocio real necesita su propio `[Authorize(Policy = "Permiso:...")]`.
- **Toda página nueva se registra en dos catálogos en código**, nunca solo en el router de Blazor:
  1. `FrontOne.Domain/Constants/PantallasWebDisponibles.cs` — agrega la `Definicion(Codigo, Modulo, Descripcion)`.
  2. `FrontOne.Web/Components/Layout/NavMenu.razor`, diccionario `RutasPorPantalla` — mapea el
     `Codigo` a la ruta real (`@page "..."`). Sin esto la página existe pero nunca aparece en el
     menú lateral aunque el usuario tenga el permiso.
  Además, agrega el seed correspondiente en `Database/Seguridad/0XX_Seed_Modulo_AplicacionWeb.sql`
  (o un script nuevo si es un módulo distinto de `AplicacionWeb`) para que el rol Administrador la
  reciba automáticamente, y actualiza `Database/Seguridad/046_Schema_SP_WebPermiso.sql` **no** —
  ese script no cambia por página nueva, es genérico.
- **Sin lógica de negocio en `.razor`**: los handlers de página llaman directo al
  `{Entidad}Service` inyectado (mismo servicio que usa WinForms), igual patrón que
  `Paises.razor` → `PaisService`. Nunca armar SQL, HTTP, ni reglas de validación dentro del
  componente — eso vive en `Application`/`Infrastructure`.
- **Eliminar siempre pregunta antes** (misma regla dura que WinForms): usar el componente
  compartido `Components/Shared/Confirmacion.razor` (`DxPopup` Sí/No), nunca borrar directo al
  click del botón Eliminar.
- **Módulo de permisos web** (`Seguridad.WebPermiso`) se administra **solo desde WinForms**
  (`PermisosAplicacionWebForm`, botón `[Permisos de Aplicación Web]` en la pestaña Seguridad) — el
  sitio nunca expone una pantalla para editar sus propios permisos.
- **Login/logout van por minimal API** (`/account/login`, `/account/logout` en
  `Security/LoginEndpoints.cs`), no por una página Blazor interactiva — firmar/borrar la cookie de
  autenticación requiere una respuesta HTTP real (`Set-Cookie`), algo que un componente
  `InteractiveServer` no puede hacer directo porque corre sobre WebSocket/SignalR. `Login.razor`
  solo renderiza el `<form>` estático que postea ahí.
- **Contraseñas**: nunca en `appsettings.json` del repo. `Sql:Password` llega por variable de
  entorno (`Sql__Password`, típicamente en el App Pool de IIS) o User Secrets en desarrollo
  (`dotnet user-secrets set "Sql:Password" "..."` dentro de `FrontOne.Web`) — `Program.cs` se
  niega a arrancar si falta.
