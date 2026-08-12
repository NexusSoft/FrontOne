USE FrontOne;
GO

-- Módulo AplicacionMovil: una Pantalla por cada tarjeta de módulo que muestra
-- FrontOne.Android en Inicio (Pallets/Embarques/Acopio/Cajas de campo/Báscula/
-- Inocuidad/Calidad/Reportes). Igual que Seguridad.AccesoMovil, son permisos puros
-- sin pantalla propia en escritorio — se administran desde la pantalla Permisos ya
-- existente. Si el usuario tiene "Consultar" en una de estas pantallas, esa tarjeta
-- se le muestra en la app; si no, no aparece. Separado del módulo Seguridad a
-- propósito: AccesoMovil (¿puede entrar a la app?) es una decisión distinta de
-- "¿qué módulos ve una vez adentro?".

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'AplicacionMovil', 'Qué tarjetas de módulo ve cada usuario en FrontOne.Android (sin pantallas propias en escritorio)'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'AplicacionMovil');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, v.Nombre, 'Tarjeta "' + v.Nombre + '" en la pantalla de Inicio de FrontOne.Android'
FROM Seguridad.Modulo m
CROSS JOIN (VALUES
    ('Pallets'), ('Embarques'), ('Acopio'), ('Cajas de campo'),
    ('Báscula'), ('Inocuidad'), ('Calidad'), ('Reportes')
) AS v(Nombre)
WHERE m.Nombre = 'AplicacionMovil'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = v.Nombre);
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'AplicacionMovil'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
