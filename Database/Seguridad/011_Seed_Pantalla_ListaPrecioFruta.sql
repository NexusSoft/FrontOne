USE FrontOne;
GO

-- Módulo Acopio (nuevo) y su pantalla Lista de Precio Fruta, con permisos completos para el rol Administrador.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Acopio', 'Acopio de fruta y listas de precio'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Acopio');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'ListaPrecioFruta', 'Lista de precio de fruta por vigencia'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Acopio'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'ListaPrecioFruta');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Acopio'
  AND p.Nombre = 'ListaPrecioFruta'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
