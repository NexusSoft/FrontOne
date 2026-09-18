# Mapa técnico de FrontOne

Fecha de revisión: 2026-09-12. Referencia para preparar cambios posteriores.

## Alcance de esta revisión

Revisión estática de la estructura de la solución, dependencias, convenciones, documentación por módulo y puntos representativos de arranque, servicios, repositorios, seguridad y SQL. No constituye una auditoría línea por línea ni una validación funcional completa. No se compilaron aplicaciones, ejecutaron pruebas ni consultaron SQL Server o SAP. La existencia de un script local no confirma su despliegue.

Había modificaciones locales previas en Acopio, Web, WinForms, SQL, documentación y QA. Se conservaron. Este mapa es el único archivo creado por esta revisión.

## Arquitectura

ERP para una empacadora/exportadora de aguacate. La solución `FrontOne.slnx` contiene ocho proyectos .NET; Android se construye aparte con Gradle.

| Proyecto | Responsabilidad |
|---|---|
| FrontOne.Domain | Entidades, DTOs, enumeraciones, catálogos de permisos e interfaces de repositorio. |
| FrontOne.Application | Servicios de negocio, validaciones, mapeo y auditoría. Depende de Domain y Shared. |
| FrontOne.Infrastructure.SqlServer | Dapper y Microsoft.Data.SqlClient; repositorios que llaman stored procedures. |
| FrontOne.Infrastructure.SapB1 | Cliente HTTP de SAP Business One Service Layer, sesiones y repositorios de proveedores, artículos y pedidos. |
| FrontOne.Shared | Configuración, seguridad, excepciones, logging y utilidades como VoicePickCodeCalculator. |
| FrontOne.WinForms | Aplicación principal net10.0-windows; DevExpress 26.1.3, reportes, etiquetas y báscula. |
| FrontOne.Web | Blazor net10.0, InteractiveServer y DevExpress 26.1.3; reutiliza servicios e infraestructura del escritorio. |
| FrontOne.Tests | xUnit; se identificó CryptoServiceTests con dos pruebas de cifrado. No demuestra cobertura de los flujos operativos. |
| FrontOne.Android | Kotlin, Compose, Hilt y coroutines; módulos app/domain/data, puertos y casos de uso. SQL directo mediante jTDS y SPs compartidos. |

Ruta habitual de un cambio: pantalla → servicio Application → interfaz Domain → repositorio → SP en `Database/{Modulo}`. Una regla puede estar implementada también en SQL; revisar ambos extremos y los consumidores Android.

## Mapa funcional e impactos

| Área | Función y relaciones relevantes | Documentación |
|---|---|---|
| Catálogos | Productores, huertas, geografía, materias primas y productos terminados; alimentan el resto del ERP. | contexto/catalogos.md |
| Acopio | Precios, acuerdos, estimaciones, autorización, órdenes de corte y acarreo. Las órdenes enlazan recepción y movimientos de cajas. | contexto/acopio.md |
| Recepción | Orden de corte, pesos, descarga, cajas y ticket de báscula. La recepción incorporada a un lote tiene restricciones de edición. | contexto/recepcion.md |
| Lotes | Agrupa recepciones y establece trazabilidad; alimenta corridas. | contexto/lotes.md |
| Producción | Corridas, consumo de kilos, pallets, ajustes de merma/diferencia, báscula y códigos de etiquetas. | contexto/produccion.md |
| Reempaques | Desarma y redistribuye producto preservando el origen. Las salidas se integran en PalletDetalle con ReempaqueDetalleId. | contexto/reempaques.md |
| Almacenes | Cajas de campo y movimientos entre existencia, campo y producción, conectados a corte y recepción. | contexto/almacenes.md |
| Gastos | Fruta, cosecha y acarreo por lote con corrida finalizada; precios y ajustes repercuten en liquidación y reportes. | contexto/gastos.md |
| Embarques | Consulta pedidos SAP y los surte con pallets en contenedores; afecta disponibilidad y estatus de pallets. | contexto/embarques.md |
| Reportes | XtraReports, plantillas persistidas, diseñador, permisos e integración TEC-IT para códigos de barras. | contexto/reportes.md |

Web registra actualmente Países, Órdenes de Corte, Simulador de Bandas, Estimación, Autorización de Estimaciones y Lotes en `FrontOne.Web/Constants/RutasWebPantallas.cs`. Android incluye login, configuración de conexión, captura/consulta de pallets y calculadora de Acopio.

## Puntos de entrada

- Escritorio: `FrontOne.WinForms/Program.cs`, `Forms/MainForm.cs` y su Designer; composición DI, SessionContext, splash y login.
- Web: `FrontOne.Web/Program.cs`, `Extensions/ServiceCollectionExtensions.cs`, `Security/LoginEndpoints.cs`, políticas y claims. Login/logout usan HTTP para emitir cookies; los componentes interactivos comparten servicios por circuito.
- Registro de negocio: `FrontOne.Application/Extensions/ServiceCollectionExtensions.cs`.
- SQL: `FrontOne.Infrastructure.SqlServer/Repositories/SqlRepositoryBase.cs` y `Factories/ConnectionFactory.cs`; errores SQL 547 y 50000 tienen tratamiento central.
- SAP: `FrontOne.Infrastructure.SapB1/SapServiceLayerClient.cs` y sus repositorios. Revisar contratos y nombres exactos de propiedades al modificar peticiones.
- Android: `FrontOne.Android/app/src/main/kotlin/com/frontone/android/di/DataModule.kt`, casos de uso en domain y adaptadores en data.

## Convenciones que deben acompañar los cambios

- Leer `CLAUDE.md`, `contexto.md` y las reglas específicas de Web o Android antes de modificar el módulo correspondiente.
- Negocio en español y sufijos técnicos en inglés; comentarios y mensajes al usuario en español.
- WinForms conserva clases parciales `.cs`, `.Designer.cs` y `.resx`, controles DevExpress y patrones existentes de botones, búsquedas y formatos. Respetar excepciones documentadas y layouts ajustados por el usuario.
- Servicios de escritura registran auditoría con usuario y snapshots anterior/posterior. Revisar también efectos secundarios sobre inventario y trazabilidad.
- Web exige políticas por pantalla/acción; revisar catálogo Domain, rutas, navegación y permisos. La administración de permisos web reside en escritorio.
- Administrador obtiene permisos completos mediante PermissionService; no extrapolar automáticamente ese comportamiento al flujo móvil.
- SPs numerados por módulo, con contratos compatibles con DTOs y repositorios. Revisar la definición más reciente, no únicamente el script inicial.
- Folios, bloqueos, estatus y cálculos deben verificarse por entidad: existen decisiones históricas y variantes posteriores.
- Cambios relevantes se documentan agregando una entrada al archivo correspondiente de `contexto/`. El tracker QA solo se modifica cuando el usuario lo solicita explícitamente.

## Diferencias documentales que conviene recordar

- Las reglas Web mencionan un diccionario en NavMenu, pero el código actual centraliza las rutas en `RutasWebPantallas.PorPantalla`.
- El índice de embarques describe principalmente pedidos de consulta; el módulo ya contiene contenedores y surtido con pallets.
- La documentación es acumulativa: las secciones iniciales sobre reempaques, reportes, folios o interfaz pueden estar reemplazadas por notas posteriores.
- El arranque WinForms actual incluye pausas y un fade gestionado por SplashCommand, aunque la primera nota del splash describe otro comportamiento.

## Cómo retomar

1. Consultar el estado Git para distinguir trabajo previo de cambios nuevos.
2. Leer este mapa y la documentación del módulo; contrastarla con el código vigente.
3. Seguir el flujo completo entre interfaz, servicio, repositorio y SP, incluyendo reportes, permisos y otras plataformas afectadas.
4. Validar según el cambio. El comando documentado de compilación es `dotnet build FrontOne.slnx -p:UseAppHost=false`; una aplicación abierta puede bloquear los binarios. Android requiere su entorno Gradle propio.
5. Registrar la decisión y su validación en la documentación del módulo cuando corresponda.
