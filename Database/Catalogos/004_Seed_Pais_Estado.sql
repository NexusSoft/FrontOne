USE FrontOne;
GO

-- Seed generado desde Catalogo_Paises_Estados.xlsx (idempotente)

INSERT INTO Catalogos.Pais (Clave, Nombre, Activo)
SELECT 'MEX', 'México', 1
WHERE NOT EXISTS (SELECT 1 FROM Catalogos.Pais WHERE Clave = 'MEX');
GO
INSERT INTO Catalogos.Pais (Clave, Nombre, Activo)
SELECT 'USA', 'Estados Unidos', 1
WHERE NOT EXISTS (SELECT 1 FROM Catalogos.Pais WHERE Clave = 'USA');
GO
INSERT INTO Catalogos.Pais (Clave, Nombre, Activo)
SELECT 'CAN', 'Canadá', 1
WHERE NOT EXISTS (SELECT 1 FROM Catalogos.Pais WHERE Clave = 'CAN');
GO

INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'AGU', 'Aguascalientes', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'AGU');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'BCN', 'Baja California', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'BCN');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'BCS', 'Baja California Sur', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'BCS');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'CAM', 'Campeche', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'CAM');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'CHP', 'Chiapas', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'CHP');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'CHH', 'Chihuahua', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'CHH');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'CMX', 'Ciudad de México', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'CMX');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'COA', 'Coahuila de Zaragoza', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'COA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'COL', 'Colima', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'COL');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'DUR', 'Durango', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'DUR');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'GUA', 'Guanajuato', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'GUA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'GRO', 'Guerrero', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'GRO');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'HID', 'Hidalgo', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'HID');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'JAL', 'Jalisco', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'JAL');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MEX', 'Estado de México', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MEX');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MIC', 'Michoacán de Ocampo', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MIC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MOR', 'Morelos', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MOR');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NAY', 'Nayarit', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NAY');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NLE', 'Nuevo León', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NLE');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'OAX', 'Oaxaca', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'OAX');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'PUE', 'Puebla', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'PUE');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'QUE', 'Querétaro', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'QUE');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'ROO', 'Quintana Roo', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'ROO');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'SLP', 'San Luis Potosí', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'SLP');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'SIN', 'Sinaloa', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'SIN');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'SON', 'Sonora', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'SON');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'TAB', 'Tabasco', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'TAB');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'TAM', 'Tamaulipas', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'TAM');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'TLA', 'Tlaxcala', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'TLA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'VER', 'Veracruz de Ignacio de la Llave', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'VER');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'YUC', 'Yucatán', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'YUC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'ZAC', 'Zacatecas', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'MEX'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'ZAC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'AL', 'Alabama', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'AL');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'AK', 'Alaska', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'AK');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'AZ', 'Arizona', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'AZ');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'AR', 'Arkansas', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'AR');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'CA', 'California', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'CA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'CO', 'Colorado', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'CO');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'CT', 'Connecticut', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'CT');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'DE', 'Delaware', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'DE');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'DC', 'District of Columbia', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'DC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'FL', 'Florida', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'FL');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'GA', 'Georgia', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'GA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'HI', 'Hawaii', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'HI');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'ID', 'Idaho', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'ID');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'IL', 'Illinois', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'IL');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'IN', 'Indiana', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'IN');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'IA', 'Iowa', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'IA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'KS', 'Kansas', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'KS');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'KY', 'Kentucky', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'KY');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'LA', 'Louisiana', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'LA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'ME', 'Maine', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'ME');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MD', 'Maryland', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MD');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MA', 'Massachusetts', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MI', 'Michigan', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MI');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MN', 'Minnesota', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MN');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MS', 'Mississippi', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MS');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MO', 'Missouri', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MO');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MT', 'Montana', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MT');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NE', 'Nebraska', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NE');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NV', 'Nevada', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NV');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NH', 'New Hampshire', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NH');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NJ', 'New Jersey', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NJ');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NM', 'New Mexico', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NM');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NY', 'New York', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NY');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NC', 'North Carolina', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'ND', 'North Dakota', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'ND');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'OH', 'Ohio', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'OH');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'OK', 'Oklahoma', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'OK');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'OR', 'Oregon', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'OR');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'PA', 'Pennsylvania', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'PA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'RI', 'Rhode Island', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'RI');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'SC', 'South Carolina', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'SC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'SD', 'South Dakota', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'SD');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'TN', 'Tennessee', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'TN');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'TX', 'Texas', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'TX');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'UT', 'Utah', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'UT');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'VT', 'Vermont', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'VT');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'VA', 'Virginia', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'VA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'WA', 'Washington', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'WA');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'WV', 'West Virginia', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'WV');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'WI', 'Wisconsin', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'WI');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'WY', 'Wyoming', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'USA'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'WY');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'AB', 'Alberta', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'AB');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'BC', 'British Columbia', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'BC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'MB', 'Manitoba', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'MB');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NB', 'New Brunswick', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NB');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NL', 'Newfoundland and Labrador', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NL');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NS', 'Nova Scotia', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NS');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NT', 'Northwest Territories', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NT');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'NU', 'Nunavut', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'NU');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'ON', 'Ontario', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'ON');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'PE', 'Prince Edward Island', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'PE');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'QC', 'Quebec', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'QC');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'SK', 'Saskatchewan', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'SK');
GO
INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
SELECT p.Id, 'YT', 'Yukon', 1
FROM Catalogos.Pais p
WHERE p.Clave = 'CAN'
  AND NOT EXISTS (SELECT 1 FROM Catalogos.Estado e WHERE e.PaisId = p.Id AND e.Clave = 'YT');
GO
