USE FrontOne;
GO

-- Módulo Acopio ya existe (ver 011_Seed_Pantalla_ListaPrecioFruta.sql) — clon del mismo patrón,
-- solo se agrega la pantalla y sus permisos.
INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'SimuladorBandas', 'Simulador de bandas de precio por Categoría/Calibre APEAM'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Acopio'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'SimuladorBandas');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Acopio'
  AND p.Nombre = 'SimuladorBandas'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
