USE FrontOne;
GO

INSERT INTO Catalogos.LineaProduccion (Nombre, Activo)
SELECT v.Nombre, 1
FROM (VALUES ('CANADA'), ('ORGANICO'), ('USA')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Catalogos.LineaProduccion lp WHERE lp.Nombre = v.Nombre);
GO
