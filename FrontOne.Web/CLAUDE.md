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
