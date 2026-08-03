USE FrontOne;
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'ProductosTerminados', 'Catálogo de productos terminados'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Catalogos'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'ProductosTerminados');
GO

-- Se mantienen los 4 permisos estándar (Consultar/Crear/Editar/Eliminar) por consistencia con el
-- resto de pantallas del sistema, aunque el form de listado no tenga botón Eliminar: los productos
-- terminados solo se crean/eliminan vía sincronización con SAP, nunca desde la UI.
INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Catalogos'
  AND p.Nombre = 'ProductosTerminados'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
