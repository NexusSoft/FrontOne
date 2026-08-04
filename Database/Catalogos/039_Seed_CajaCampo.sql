USE FrontOne;
GO

INSERT INTO Catalogos.CajaCampo (Nombre, Activo)
SELECT v.Nombre, 1
FROM (VALUES ('ROJA'), ('AZUL'), ('BLANCA'), ('AMARILLA')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Catalogos.CajaCampo c WHERE c.Nombre = v.Nombre);
GO
