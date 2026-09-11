USE FrontOne;
GO

-- Regla dura (ver CLAUDE.md): el rol Administrador siempre tiene TODOS los permisos —
-- de escritorio, de reportes y del sitio web — sobre cualquier pantalla/reporte/página presente
-- O FUTURA, sin depender de que alguien recuerde otorgárselos a mano cada vez que se agrega un
-- módulo nuevo (bug real que motivó esto: el reporte "ValeRecepcion" se agregó sin que NINGÚN rol,
-- ni siquiera Administrador, tuviera permiso sobre él).
--
-- Este SP responde "¿el usuario tiene el rol Administrador?" — lo consume
-- FrontOne.Application.Services.PermissionService para saltarse por completo la tabla de permisos
-- correspondiente (Seguridad.Permiso/ReportePermiso/WebPermiso) cuando la respuesta es sí, en vez
-- de depender de que esas tablas tengan una fila por cada pantalla/reporte/página que exista.
CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_EsAdministrador
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (
        SELECT 1
        FROM Seguridad.UsuarioRol ur
        INNER JOIN Seguridad.Rol r ON r.Id = ur.RolId
        WHERE ur.UsuarioId = @UsuarioId AND r.Nombre = 'Administrador'
    ) THEN 1 ELSE 0 END AS BIT) AS EsAdministrador;
END
GO

-- Universo completo de permisos de escritorio (mismo shape Modulo/Pantalla/Accion que
-- sp_Usuario_ObtenerPermisos), cruzando TODAS las Pantallas (con su Modulo) contra TODAS las
-- Acciones existentes — sin filtrar por Rol. Solo lo usa PermissionService cuando ya se confirmó
-- que el usuario es Administrador (ver SP de arriba). Genera combinaciones sin sentido de negocio
-- real (ej. "Recuperar" en una pantalla que no lo usa) que quedan de más pero son inofensivas:
-- ninguna pantalla de la app consulta un permiso que no le corresponde.
CREATE OR ALTER PROCEDURE Seguridad.sp_Pantalla_ObtenerTodosLosPermisosPosibles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT m.Nombre AS Modulo, p.Nombre AS Pantalla, a.Nombre AS Accion
    FROM Seguridad.Pantalla p
    INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
    CROSS JOIN Seguridad.Accion a;
END
GO

-- Arreglo puntual de datos: otorga los 4 permisos de TODOS los reportes que existen hoy (incluido
-- "ValeRecepcion") al rol Administrador. Ya no es indispensable para que Administrador funcione
-- (PermissionService deja de consultar esta tabla para ese rol desde este cambio), pero deja el
-- dato consistente para quien consulte Seguridad.ReportePermiso directo o abra la pantalla
-- "Permisos de Reportes" y vea la matriz ya marcada.
INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
SELECT r.Id, codigos.Codigo, 1, 1, 1, 1
FROM Seguridad.Rol r
CROSS JOIN (VALUES
    ('RecepcionFruta'), ('ValeRecepcion'), ('Pallet'), ('Incidencias'), ('ProcesoLote'),
    ('LiquidacionProductor'), ('ContenedorCarga'), ('ContenedorDetalleLote'), ('ContenedorDetalleHuerta'),
    ('ContenedorResumenCalibre'), ('ContenedorResumenLote'), ('ContenedorResumenHuerta'),
    ('ContenedorResumenHuertaSinKg')
) AS codigos(Codigo)
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.ReportePermiso rp
      WHERE rp.RolId = r.Id AND rp.ReporteCodigo = codigos.Codigo);
GO
