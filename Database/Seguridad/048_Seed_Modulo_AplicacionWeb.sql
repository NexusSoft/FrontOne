USE FrontOne;
GO

-- Módulo AplicacionWeb: una Pantalla por cada página real del sitio FrontOne.Web. Arranca con
-- "Paises" (módulo de ejemplo de esta fase) y crece con cada página nueva. Igual que
-- Seguridad.AccesoWeb, son permisos puros sin pantalla propia en escritorio — se administran
-- desde WinForms con el botón [Permisos de Aplicación Web]. Separado del módulo Seguridad a
-- propósito: AccesoWeb (¿puede entrar al sitio?) es una decisión distinta de "¿qué páginas ve una
-- vez adentro?".

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'AplicacionWeb', 'Qué páginas del sitio FrontOne.Web ve cada usuario (sin pantallas propias en escritorio)'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'AplicacionWeb');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, v.Nombre, 'Página "' + v.Nombre + '" del sitio FrontOne.Web'
FROM Seguridad.Modulo m
CROSS JOIN (VALUES
    ('Paises')
) AS v(Nombre)
WHERE m.Nombre = 'AplicacionWeb'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = v.Nombre);
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'AplicacionWeb'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
