USE FrontOne;
GO

-- Módulo Embarques (nuevo) y su primera pantalla, Pedidos, con permisos completos para el rol
-- Administrador. Mismo patrón exacto que 049_Seed_Modulo_Reempaques.sql. El botón _btnPedidos
-- ya vive en el Ribbon (pestaña "Embarques", grupo "Logística") sin permiso detrás; este script
-- lo habilita.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Embarques', 'Consulta de Pedidos de Venta capturados en SAP, previo a su embarque'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Embarques');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Pedidos', 'Pedidos de Venta (solo lectura, datos desde SAP)'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Embarques'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Pedidos');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Embarques'
  AND p.Nombre = 'Pedidos'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
