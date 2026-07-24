USE FrontOne;
GO

INSERT INTO Catalogos.Producto (Nombre, Activo)
SELECT v.Nombre, 1
FROM (VALUES ('MX HASS AVOCADO'), ('MX ORGANIC HASS AVOCADO')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Catalogos.Producto p WHERE p.Nombre = v.Nombre);
GO
