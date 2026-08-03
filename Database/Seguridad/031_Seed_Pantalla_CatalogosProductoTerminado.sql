USE FrontOne;
GO

-- Faltaban las pantallas de los 5 catálogos de apoyo de Producto Terminado (Categoría, Tipo de
-- Producto, Calibre APEAM, Marca, Peso Estándar) — el botón "+" de cada LookUpEdit en
-- ProductoTerminadoEditarForm valida permiso "Crear" sobre estas pantallas exactas
-- (Categorias/TiposProducto/CalibresApeam/Marcas/PesosEstandar) y sin ellas siempre da
-- "No tienes permiso para acceder a este formulario.", aunque el Administrador tenga todo lo demás.
INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, x.Nombre, x.Descripcion
FROM Seguridad.Modulo m
CROSS JOIN (VALUES
    ('Categorias', 'Catálogo de categorías de producto terminado'),
    ('TiposProducto', 'Catálogo de tipos de producto terminado'),
    ('CalibresApeam', 'Catálogo de calibres APEAM'),
    ('Marcas', 'Catálogo de marcas de producto terminado'),
    ('PesosEstandar', 'Catálogo de pesos estándar de producto terminado')
) AS x(Nombre, Descripcion)
WHERE m.Nombre = 'Catalogos'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = x.Nombre);
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Catalogos'
  AND p.Nombre IN ('Categorias', 'TiposProducto', 'CalibresApeam', 'Marcas', 'PesosEstandar')
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
