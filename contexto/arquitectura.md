# Arquitectura, convenciones y decisiones fundacionales

> Parte de la memoria viva del proyecto FrontOne — ver [contexto.md](../contexto.md) para el índice completo. Este archivo cubre lo fundacional (capas, infraestructura, convenciones, reglas de UI) que cambia poco. El detalle de cada módulo de negocio vive en su propio archivo dentro de esta carpeta.

## Arquitectura (capas)

```
FrontOne.Domain              Entidades, DTOs (records), interfaces de repositorio, enums
FrontOne.Application          Servicios de negocio (orquestan repos + validación + auditoría)
FrontOne.Infrastructure.SqlServer   Repositorios Dapper contra SQL Server (solo stored procedures)
FrontOne.Infrastructure.SapB1       Cliente del SAP Service Layer
FrontOne.Shared               Cross-cutting: Result/PagedResult, excepciones, CryptoService, Options, interfaces de seguridad
FrontOne.WinForms             UI (DevExpress), composition root (Program.cs + DI)
FrontOne.Tests                xUnit
```

Reglas de dependencia: `Application` no puede depender de `WinForms`. El puente para saber "qué usuario está haciendo esto" es `ICurrentUserProvider` (en `Shared/Security`), implementado por `SessionContext` en WinForms.


## Infraestructura / credenciales de desarrollo

- **SQL Server**: instancia `Lider-TI`, base `FrontOne`. Auth SQL (usuario `sa`, no Windows). Ejecutar scripts con `sqlcmd -S "Lider-TI" -d FrontOne -E -b -f 65001 -i archivo.sql` (el `-f 65001` es necesario para que los acentos no se corrompan).
- **SAP B1 Service Layer**: `https://fronterra.vdv.one:50000/b1s/v1`, compañía `TEST_PROD_FASA`, usuario `manager`.
- Ambas credenciales completas están en la memoria persistente de Claude (`sql_test_credentials.md`, `sap_test_credentials.md`), no se repiten aquí por seguridad.


## Estado por fase (todo lo marcado está construido, compilado y probado contra la BD real)

### Fase 1 — Esqueleto de capas
`Result`/`PagedResult`/excepciones/Options (Shared), `SqlRepositoryBase`/`ConnectionFactory` (Infra.SqlServer), `SapServiceLayerClient` (Infra.SapB1), extensiones de DI por capa, composition root en `Program.cs`.

### Fase 2 — Seguridad base + Auditoría + Conexiones
- Esquema `Seguridad`: `Usuario`, `Rol`, `Permiso` (Módulo → Pantalla → Acción), `UsuarioRol`.
- Esquema `Auditoria`: registro de Crear/Modificar/Eliminar por módulo, JSON antes/después.
- `CryptoService` (AES-256, clave derivada fija y portable — no atada a la máquina) usado para **todas** las contraseñas del sistema (conexiones, Usuario, Productor, SAP).
- `LoginForm`, `SessionContext` (sesión en memoria + permisos), `ConfiguracionConexionesForm` (prueba y guarda credenciales SQL/SAP), `RegistryConnectionStore`.


## Decisiones de UI ya tomadas (no volver a preguntar)

- **Ribbon de `MainForm`**: 3 pestañas por módulo — **Catálogos** (grupos "Ubicaciones": Países/Estados; "Socios de Negocio": Productores/Huertas), **Seguridad** (grupo "Usuarios y Roles": Usuarios/Roles/Permisos), **Sistema** (grupos "Configuración": Conexiones; "Aplicación": Salir). Botones en estilo `RibbonItemStyles.Large` (ícono arriba, texto abajo).
- **MDI con pestañas**: `MainForm` usa `DevExpress.XtraTabbedMdi.XtraTabbedMdiManager` — los forms MDI-child (Productores, Huertas) se muestran como pestañas de documento arriba, no como ventanas flotantes.
- **Flujo de catálogos vía lookupedit (regla fija)**: el botón "+" (quick-add) de un `LookUpEdit` **no** abre directo el form de alta — abre el **listado** del catálogo (`{EntidadPlural}Form`, con Nuevo/Editar/Eliminar/Cerrar), para poder editar y borrar desde ahí también, no solo crear. Excepción: Productor, cuyo "+" abre `ProductorEditarForm` directo porque ese form YA es maestro-detalle completo (navigator + Nuevo/Eliminar/Guardar), abrir el picker ahí sería un downgrade.
- **Todo `LookUpEdit` del proyecto** (regla dura, ver `CLAUDE.md`): `Properties.NullText = "Seleccionar"` siempre; si tiene botón custom (`Plus`, etc.) hay que agregar explícitamente `ButtonPredefines.Combo` antes, porque DevExpress oculta la flecha automática en cuanto agregás cualquier botón a `Properties.Buttons`.
- **Íconos de botones CRUD** (regla dura, ver `CLAUDE.md`): todo `_btnNuevo`/`_btnEditar`/`_btnEliminar`/`_btnCerrar` usa el mismo ícono en todo el proyecto, tomado de `FrontOne.WinForms/Forms/Catalogos/EstadosForm.resx`; todo `_btnGuardar`/`_btnCancelar` (de cualquier `{Entidad}EditarForm`, incluidos los maestro-detalle) usa el ícono de `FrontOne.WinForms/Forms/Catalogos/ProductoEditarForm.resx` — son dos fuentes distintas, no confundir. Los `.resx` de DevExpress son XML de texto plano (bitmap en base64), así que se pueden copiar los bloques `<data>` a mano sin abrir Visual Studio — el `.resx` tiene que vivir en la misma carpeta/namespace que su `.cs` o `resources.GetObject(...)` no encuentra el ícono en runtime.
- **Estructura de carpetas de `Forms/`**: subcarpetas por módulo — `Forms/Seguridad`, `Forms/Catalogos`, `Forms/Sistema` (cada una con su propio namespace `FrontOne.WinForms.Forms.{Carpeta}`). `MainForm` se queda en la raíz de `Forms/` por ser el shell de la app, no pertenece a ningún módulo.
- **Orden de botones** (regla dura, ver `CLAUDE.md`): en los forms maestro-detalle (`ProductorEditarForm`/`HuertaEditarForm`) el grupo izquierdo va **Nuevo, Guardar, Eliminar** en ese orden (los 3 con `Anchor = Bottom, Left`), y `Cancelar` siempre solo a la derecha (`Bottom, Right`). Mismo criterio en los listados: Nuevo/Editar/Eliminar a la izquierda, Cerrar a la derecha.


## Patrones de código establecidos (repetir en todo módulo nuevo)

- **Patrón clásico VS Designer** en todo `XtraForm`: `{Form}.Designer.cs` (parcial, controles como campos privados, `InitializeComponent()`) + `{Form}.cs` (ctor vacío para el diseñador + ctor con servicios inyectados `: this()`). Nunca lambdas inline en `Designer.cs` — rompe el diseñador de VS.
- **Maestro-detalle con `DataNavigator`**: `BindingSource` sobre `List<TDto>` cacheada, `CurrentChanged` recarga el form, "Nuevo" limpia campos sin tocar la posición del navigator, "Guardar" inserta/actualiza y reposiciona.
- **Auditoría obligatoria**: todo servicio Application con Crear/Actualizar/Eliminar inyecta `AuditService` + `ICurrentUserProvider`, relee antes/después, serializa a JSON completo (no diff campo por campo).
- **Todo módulo nuevo repite**: Entidad → DTO → Interfaz repo → Repositorio (`SqlRepositoryBase`) → SPs (`{Schema}.sp_{Entidad}_{Accion}`) → Servicio (con auditoría) → Validador → Form(s) (listado + editar, ambos con íconos e patrón clásico).
- **Permiso "quick-add" en lookups**: antes de abrir el listado/editor de un catálogo relacionado desde un "+", se valida `SessionContext.TienePermiso(modulo, pantalla, "Crear")`; si no tiene, `XtraMessageBox` de advertencia y no abre nada.


## Convenciones de nombres

Ver `CLAUDE.md` completo — resumen: sustantivos de negocio en español (`Cliente`, `Huerta`), sufijos técnicos en inglés (`Service`, `Repository`, `Form`, `Dto`), SPs `{Schema}.sp_{Entidad}_{Accion}` en español, DevExpress obligatorio sin excepción (ni en forms simples).


## Idioma: comentarios y mensajes al usuario (regla dura, ver `CLAUDE.md`)

Todo comentario de código (`//`, `/* */`, `///`, comentarios `.sql`) y todo mensaje visible para el usuario (`XtraMessageBox`, mensajes de excepción que llegan a UI, `Text`/`Caption`/`NullText`) va en **español**. Se hizo un barrido inicial sobre el código existente (el proyecto ya estaba mayormente en español; se corrigieron los pocos textos sueltos en inglés: "Password:" → "Contraseña:" en `ProductorEditarForm`/`UsuarioEditarForm`, "Status" → "Estatus" en el módulo de Huertas, y un comentario en `003_Seed_Manager.sql`). Cualquier cosa nueva que se agregue de acá en adelante debe nacer en español, no traducirse después.

**Nota de nomenclatura fijada en este barrido**: el campo de estado de vida de una huerta (catálogo `StatusHuerta`, valores tipo Nueva/Producción/etc.) se etiqueta en UI como **"Estatus"**, no "Estado" — porque el mismo form ya tiene un campo "Estado:" para el estado geográfico (`Catalogos.Estado`, ligado a País). Usar "Estado" para ambos generaba colisión visual. De paso se corrigió `_lblEstatusActivo` (el combo Activa/Baja) que estaba mal etiquetado como "Estado:" → ahora dice "Activo:". Los nombres de clase/tabla (`StatusHuerta`, `StatusHuertaService`, etc.) NO se tocaron — es solo el texto visible en pantalla el que cambió.

